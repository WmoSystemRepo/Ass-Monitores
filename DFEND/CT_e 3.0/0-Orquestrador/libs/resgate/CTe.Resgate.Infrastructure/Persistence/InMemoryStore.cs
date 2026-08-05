using System.Collections.Concurrent;
using CTe.Resgate.Application.Abstractions;
using CTe.Resgate.Domain;

namespace CTe.Resgate.Infrastructure.Persistence;

public sealed class InMemoryResgateStore : IResgateStore
{
    private readonly ConcurrentDictionary<long, LoteResgate> _lotes = new();
    private readonly ConcurrentDictionary<long, ItemResgate> _itens = new();
    private readonly ConcurrentBag<EventoResgate> _eventos = new();
    private long _loteSeq;
    private long _itemSeq;
    private long _evtSeq;

    public Task<LoteResgate> CreateLoteAsync(string usuario, IReadOnlyList<string> chaves, CancellationToken ct)
    {
        var loteId = Interlocked.Increment(ref _loteSeq);
        var lote = new LoteResgate
        {
            Id = loteId,
            Usuario = usuario,
            Status = LoteStatus.Aberto,
            Total = chaves.Count,
            CorrelationId = Guid.NewGuid()
        };
        _lotes[loteId] = lote;

        foreach (var chave in chaves)
        {
            var itemId = Interlocked.Increment(ref _itemSeq);
            _itens[itemId] = new ItemResgate
            {
                Id = itemId,
                LoteId = loteId,
                Chave = chave,
                Status = ItemStatus.Pendente,
                PassoAtual = PassoResgate.P0
            };
        }

        AppendEvento(new EventoResgate
        {
            LoteId = loteId,
            Mensagem = $"Lote {loteId} criado com {chaves.Count} chaves",
            Passo = PassoResgate.P0
        });

        return Task.FromResult(lote);
    }

    public Task<LoteResgate?> GetLoteAsync(long id, CancellationToken ct)
        => Task.FromResult(_lotes.TryGetValue(id, out var l) ? Clone(l) : null);

    public Task<IReadOnlyList<ItemResgate>> GetItensAsync(long loteId, int skip, int take, CancellationToken ct)
    {
        var list = _itens.Values.Where(i => i.LoteId == loteId)
            .OrderBy(i => i.Id).Skip(skip).Take(take)
            .Select(Clone).ToList();
        return Task.FromResult<IReadOnlyList<ItemResgate>>(list);
    }

    public Task<IReadOnlyList<EventoResgate>> GetEventosAsync(long loteId, int take, CancellationToken ct)
    {
        var list = _eventos.Where(e => e.LoteId == loteId).OrderByDescending(e => e.Id).Take(take)
            .OrderBy(e => e.Id).ToList();
        return Task.FromResult<IReadOnlyList<EventoResgate>>(list);
    }

    public Task<ItemResgate?> ClaimNextPendenteAsync(CancellationToken ct)
    {
        foreach (var item in _itens.Values.OrderBy(i => i.Id))
        {
            if (item.Status != ItemStatus.Pendente) continue;
            lock (item)
            {
                if (item.Status != ItemStatus.Pendente) continue;
                item.Status = ItemStatus.EmProcessamento;
                item.PassoAtual = PassoResgate.P1;
                item.AtualizadoEm = DateTime.UtcNow;
                item.Tentativas++;
                if (_lotes.TryGetValue(item.LoteId, out var lote))
                {
                    lote.Status = LoteStatus.Processando;
                    lote.ChaveAtual = item.Chave;
                    lote.PassoAtualLote = PassoResgate.P1;
                }
                return Task.FromResult<ItemResgate?>(Clone(item));
            }
        }
        return Task.FromResult<ItemResgate?>(null);
    }

    public Task UpdateItemAsync(ItemResgate item, CancellationToken ct)
    {
        if (_itens.TryGetValue(item.Id, out var cur))
        {
            cur.Status = item.Status;
            cur.PassoAtual = item.PassoAtual;
            cur.Motivo = item.Motivo;
            cur.Tentativas = item.Tentativas;
            cur.AtualizadoEm = DateTime.UtcNow;
            cur.TempoMs = item.TempoMs;
        }
        return Task.CompletedTask;
    }

    public Task UpdateLoteAsync(LoteResgate lote, CancellationToken ct)
    {
        if (_lotes.TryGetValue(lote.Id, out var cur))
        {
            cur.Status = lote.Status;
            cur.Recuperados = lote.Recuperados;
            cur.Existentes = lote.Existentes;
            cur.NaoLocalizados = lote.NaoLocalizados;
            cur.Erros = lote.Erros;
            cur.ChaveAtual = lote.ChaveAtual;
            cur.PassoAtualLote = lote.PassoAtualLote;
        }
        return Task.CompletedTask;
    }

    public Task AppendEventoAsync(EventoResgate evt, CancellationToken ct)
    {
        AppendEvento(evt);
        return Task.CompletedTask;
    }

    public Task RequeueStaleProcessingAsync(TimeSpan staleAfter, CancellationToken ct)
    {
        var cutoff = DateTime.UtcNow - staleAfter;
        foreach (var item in _itens.Values)
        {
            if (item.Status == ItemStatus.EmProcessamento && item.AtualizadoEm < cutoff)
            {
                item.Status = ItemStatus.Pendente;
                item.PassoAtual = PassoResgate.P0;
                item.Motivo = "Retomada após stale EmProcessamento";
            }
        }
        return Task.CompletedTask;
    }

    private void AppendEvento(EventoResgate evt)
    {
        evt.Id = Interlocked.Increment(ref _evtSeq);
        evt.Horario = DateTime.UtcNow;
        _eventos.Add(evt);
    }

    private static LoteResgate Clone(LoteResgate l) => new()
    {
        Id = l.Id, Usuario = l.Usuario, CriadoEm = l.CriadoEm, Status = l.Status,
        Total = l.Total, Recuperados = l.Recuperados, Existentes = l.Existentes,
        NaoLocalizados = l.NaoLocalizados, Erros = l.Erros, ChaveAtual = l.ChaveAtual,
        PassoAtualLote = l.PassoAtualLote, CorrelationId = l.CorrelationId
    };

    private static ItemResgate Clone(ItemResgate i) => new()
    {
        Id = i.Id, LoteId = i.LoteId, Chave = i.Chave, Status = i.Status,
        PassoAtual = i.PassoAtual, Motivo = i.Motivo, Tentativas = i.Tentativas,
        AtualizadoEm = i.AtualizadoEm, TempoMs = i.TempoMs
    };
}

public sealed class InMemoryDocumentoRepository : IDocumentoRepository
{
    private readonly ConcurrentDictionary<string, string> _docs = new(StringComparer.Ordinal);

    public Task<bool> ExistsAsync(string chave, CancellationToken ct)
        => Task.FromResult(_docs.ContainsKey(chave));

    public Task InsertIfAbsentAsync(string chave, string xml, string? protocolo, string? nsu, CancellationToken ct)
    {
        _docs.TryAdd(chave, xml);
        return Task.CompletedTask;
    }
}
