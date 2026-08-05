using CTe.Resgate.Application.Abstractions;
using CTe.Resgate.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CTe.Resgate.Infrastructure.Workers;

/// <summary>
/// LEGADO — worker com SOAP próprio e tabelas lote_resgate_*.
/// Desligado no DI: o Resgate atual só informa chaves à Carga (ProcessarDownload).
/// Não registrar AddHostedService&lt;ResgateWorker&gt; no caminho feliz.
/// </summary>
public sealed class ResgateWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<ResgateWorker> logger) : BackgroundService
{
    private static readonly int[] BackoffMs = [2000, 4000, 8000];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("ResgateWorker iniciado");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var store = scope.ServiceProvider.GetRequiredService<IResgateStore>();
                var an = scope.ServiceProvider.GetRequiredService<IAnConsultaClient>();
                var docs = scope.ServiceProvider.GetRequiredService<IDocumentoRepository>();

                await store.RequeueStaleProcessingAsync(TimeSpan.FromMinutes(5), stoppingToken);
                var item = await store.ClaimNextPendenteAsync(stoppingToken);
                if (item is null)
                {
                    await Task.Delay(500, stoppingToken);
                    continue;
                }

                await ProcessItemAsync(store, an, docs, item, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro no loop do Worker");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    private async Task ProcessItemAsync(
        IResgateStore store, IAnConsultaClient an, IDocumentoRepository docs,
        ItemResgate item, CancellationToken ct)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var lote = await store.GetLoteAsync(item.LoteId, ct);
        if (lote is null) return;

        async Task Evt(string msg, string passo)
        {
            await store.AppendEventoAsync(new EventoResgate
            {
                LoteId = item.LoteId,
                ItemId = item.Id,
                Mensagem = msg,
                Passo = passo
            }, ct);
        }

        await Evt($"Pegou chave {ChaveAccessRules.Mask(item.Chave)}", PassoResgate.P1);

        AnConsultaResult? result = null;
        for (var attempt = 0; attempt < 3; attempt++)
        {
            item.PassoAtual = PassoResgate.P2;
            item.Tentativas = attempt + 1;
            lote.PassoAtualLote = PassoResgate.P2;
            lote.ChaveAtual = item.Chave;
            await store.UpdateItemAsync(item, ct);
            await store.UpdateLoteAsync(lote, ct);
            await Evt("Consultando Ambiente Nacional", PassoResgate.P2);

            result = await an.ConsultarPorChaveAsync(item.Chave, ct);
            item.PassoAtual = PassoResgate.P3;
            await store.UpdateItemAsync(item, ct);
            await Evt($"Resposta AN: {result.Codigo}", PassoResgate.P3);

            if (result.Sucesso || !result.Retryable) break;
            if (attempt < 2)
                await Task.Delay(BackoffMs[attempt], ct);
        }

        item.PassoAtual = PassoResgate.P4;
        await store.UpdateItemAsync(item, ct);
        await Evt("Verificando no banco DEV", PassoResgate.P4);

        if (result is null || (!result.Sucesso && result.Retryable))
        {
            await Finish(ItemStatus.Erro, PassoResgate.P5d, result?.Mensagem ?? "Falha AN", store, lote, item, sw, Evt, ct);
            return;
        }

        if (!result.Encontrado)
        {
            await Finish(ItemStatus.NaoLocalizado, PassoResgate.P5c, result.Mensagem ?? "Não localizado", store, lote, item, sw, Evt, ct);
            return;
        }

        var exists = await docs.ExistsAsync(item.Chave, ct);
        if (exists)
        {
            await Finish(ItemStatus.Existente, PassoResgate.P5a, "Já existia — sem sobrescrita", store, lote, item, sw, Evt, ct);
            return;
        }

        item.PassoAtual = PassoResgate.P5b;
        await store.UpdateItemAsync(item, ct);
        await Evt("Gravando XML no banco", PassoResgate.P5b);
        try
        {
            await docs.InsertIfAbsentAsync(item.Chave, result.Xml ?? "", null, null, ct);
            // Reconcile: se outro processo inseriu, Exists → Existente
            if (await docs.ExistsAsync(item.Chave, ct))
                await Finish(ItemStatus.Recuperado, PassoResgate.P5b, "Recuperado e gravado", store, lote, item, sw, Evt, ct);
            else
                await Finish(ItemStatus.Erro, PassoResgate.P5d, "Falha ao gravar documento", store, lote, item, sw, Evt, ct);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Falha persistência");
            if (await docs.ExistsAsync(item.Chave, ct))
                await Finish(ItemStatus.Existente, PassoResgate.P5a, "Reconciliado: já existia", store, lote, item, sw, Evt, ct);
            else
                await Finish(ItemStatus.Erro, PassoResgate.P5d, "Erro gravação", store, lote, item, sw, Evt, ct);
        }
    }

    private static async Task Finish(
        string status, string passo, string motivo,
        IResgateStore store, LoteResgate lote, ItemResgate item,
        System.Diagnostics.Stopwatch sw,
        Func<string, string, Task> evt,
        CancellationToken ct)
    {
        item.Status = status;
        item.PassoAtual = passo;
        item.Motivo = motivo;
        item.TempoMs = (int)sw.ElapsedMilliseconds;
        await store.UpdateItemAsync(item, ct);
        await evt($"Resultado: {status} — {motivo}", PassoResgate.P6);
        item.PassoAtual = PassoResgate.P7;
        await store.UpdateItemAsync(item, ct);

        switch (status)
        {
            case ItemStatus.Recuperado: lote.Recuperados++; break;
            case ItemStatus.Existente: lote.Existentes++; break;
            case ItemStatus.NaoLocalizado: lote.NaoLocalizados++; break;
            default: lote.Erros++; break;
        }

        var itens = await store.GetItensAsync(lote.Id, 0, 1000, ct);
        var pending = itens.Count(i => i.Status is ItemStatus.Pendente or ItemStatus.EmProcessamento);
        lote.PassoAtualLote = PassoResgate.P7;
        if (pending == 0)
        {
            lote.Status = LoteStatus.Concluido;
            lote.ChaveAtual = null;
            await evt($"Lote {lote.Id} concluído", PassoResgate.P7);
        }
        await store.UpdateLoteAsync(lote, ct);
    }
}
