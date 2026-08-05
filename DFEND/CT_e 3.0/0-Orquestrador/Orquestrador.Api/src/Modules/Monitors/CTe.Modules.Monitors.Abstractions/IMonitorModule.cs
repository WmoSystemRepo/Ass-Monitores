namespace CTe.Modules.Monitors.Abstractions;

/// <summary>
/// Contrato unificado que todo monitor (in-process ou HTTP bridge) implementa para ser exposto
/// via <c>/api/monitores/{servico}/*</c> pelo Orquestrador (SDD Monitor Unificado).
/// Os retornos são <see cref="object"/> propositalmente (payload já pronto para serialização,
/// tipicamente um <see cref="System.Text.Json.JsonElement"/> ou um record do próprio monitor) —
/// o endpoint mapper apenas repassa/normaliza, sem conhecer o shape de cada serviço.
/// </summary>
public interface IMonitorModule
{
    /// <summary>Id do serviço (receptor, arquivador, sintetizador, analisador, integrador, carga).</summary>
    string ServiceId { get; }

    Task<object?> GetInfoAsync(CancellationToken ct);

    Task<object?> GetSnapshotAsync(CancellationToken ct);

    Task<object?> GetLogsAsync(long afterSeq, int take, CancellationToken ct);

    Task<object?> GetTableAsync(string key, int take, CancellationToken ct);

    Task<object?> GetServiceStatusAsync(CancellationToken ct);

    Task<object?> StartAsync(CancellationToken ct);

    Task<object?> StopAsync(CancellationToken ct);

    Task<object?> GetHealthAsync(CancellationToken ct);
}
