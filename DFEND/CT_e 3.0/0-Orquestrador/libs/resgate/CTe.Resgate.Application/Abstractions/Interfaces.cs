namespace CTe.Resgate.Application.Abstractions;

using CTe.Resgate.Domain;

public sealed record CargaEnqueueResult(
    int Enfileirados,
    IReadOnlyList<long> Ids,
    int PendentesTemp,
    int ProfundidadeFilaBroker,
    double? IdadeMaxTempMinutos,
    string Usuario);

public interface ICargaDownloadEnqueue
{
    Task<CargaEnqueueResult> EnfileirarAsync(string usuario, IReadOnlyList<string> chaves, CancellationToken ct);
    Task<object> GetFilaStatusAsync(CancellationToken ct);
    Task<object> GetStatusChavesAsync(IReadOnlyList<string> chaves, CancellationToken ct);
}

/// <summary>Legado — fila própria lote_resgate_* (não usado no modo Carga-download).</summary>
public interface IResgateStore
{
    Task<LoteResgate> CreateLoteAsync(string usuario, IReadOnlyList<string> chaves, CancellationToken ct);
    Task<LoteResgate?> GetLoteAsync(long id, CancellationToken ct);
    Task<IReadOnlyList<ItemResgate>> GetItensAsync(long loteId, int skip, int take, CancellationToken ct);
    Task<IReadOnlyList<EventoResgate>> GetEventosAsync(long loteId, int take, CancellationToken ct);
    Task<ItemResgate?> ClaimNextPendenteAsync(CancellationToken ct);
    Task UpdateItemAsync(ItemResgate item, CancellationToken ct);
    Task UpdateLoteAsync(LoteResgate lote, CancellationToken ct);
    Task AppendEventoAsync(EventoResgate evt, CancellationToken ct);
    Task RequeueStaleProcessingAsync(TimeSpan staleAfter, CancellationToken ct);
}

public interface IDocumentoRepository
{
    Task<bool> ExistsAsync(string chave, CancellationToken ct);
    Task InsertIfAbsentAsync(string chave, string xml, string? protocolo, string? nsu, CancellationToken ct);
}

public sealed record AnConsultaResult(bool Sucesso, bool Encontrado, string? Xml, string? Codigo, string? Mensagem, bool Retryable);

public interface IAnConsultaClient
{
    Task<AnConsultaResult> ConsultarPorChaveAsync(string chave, CancellationToken ct);
}

public interface IResgateService
{
    Task<(CargaEnqueueResult? Result, IReadOnlyList<string> Errors)> EnfileirarDownloadAsync(
        string usuario, IEnumerable<string> chaves, CancellationToken ct);

    Task<object> GetFilaStatusAsync(CancellationToken ct);
    Task<object> GetStatusChavesAsync(IReadOnlyList<string> chaves, CancellationToken ct);
}
