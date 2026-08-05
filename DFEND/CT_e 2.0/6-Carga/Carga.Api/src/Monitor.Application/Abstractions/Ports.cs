using Monitor.Domain.Models;

namespace Monitor.Application.Abstractions;

public interface IMonitorReadRepository
{
    Task<ServiceReadResult> GetServiceAsync(int codServico, CancellationToken ct);
    Task<ConfigReadResult> GetConfigsAsync(int codServico, CancellationToken ct);
    Task<QueueReadResult> GetQueueCountsAsync(CancellationToken ct);
    Task<DocumentsReadResult> GetRecentDocumentsAsync(int take, CancellationToken ct);
    Task<LogsReadResult> GetLogsAfterAsync(long afterSeq, int take, CancellationToken ct);
    Task<DocumentsReadResult> GetDocumentsSinceAsync(DateTimeOffset? sessionStart, int take, CancellationToken ct);
    Task<LogsReadResult> GetLogsSinceAsync(DateTimeOffset? sessionStart, int take, CancellationToken ct);
    Task<ConfigDetailReadResult> GetProcessConfigsAsync(int codServico, CancellationToken ct);
    /// <summary>Health ping: SELECT 1 on ConnectionString.</summary>
    Task<bool> PingAsync(CancellationToken ct);
    /// <summary>Health ping on ConnectionStringSintetico when configured; otherwise true.</summary>
    Task<bool> PingSinteticoAsync(CancellationToken ct);
}

public interface IMonitorWriteRepository
{
    Task SetExecutarAsync(int codServico, int value, CancellationToken ct);
}

public interface IWindowsServiceController
{
    ServiceControlResult GetStatus(string serviceName);
    ServiceControlResult Start(string serviceName);
    ServiceControlResult Stop(string serviceName);
}

public interface ISnapshotAggregator
{
    Task<MonitorSnapshot> BuildSnapshotAsync(CancellationToken ct);
}

public interface ITableDetailService
{
    Task<TableDetailDto?> GetAsync(string key, int take, CancellationToken ct);
}

public interface ILiveTraceReader
{
    IReadOnlyList<LiveTraceLine> ReadRecent(int take = 80);
}

public sealed record ServiceRow(
    string? DesServico,
    string? NomServidor,
    DateTimeOffset? DtcExecucao,
    string? NumSequencialUnico,
    DateTimeOffset? DtcAtualizacao = null);

public sealed record ServiceReadResult(ServiceRow? Row, long QueryMs);

public sealed record ConfigReadResult(
    IReadOnlyDictionary<string, string> Items,
    DateTimeOffset? ExecutarUpdatedAt,
    long QueryMs);

public sealed record QueueReadResult(
    long TempBacklog,
    long BrokerDepth,
    DateTimeOffset? OldestTempAt,
    long TempQueryMs,
    long BrokerQueryMs,
    long CargaDepth = 0,
    long AnalisadorDepth = 0,
    long IntegradorDepth = 0);

public sealed record DocumentsReadResult(IReadOnlyList<RecentDocument> Items, long QueryMs);

public sealed record LogsReadResult(IReadOnlyList<LogEntry> Items, long QueryMs);

public sealed record ConfigDetailReadResult(
    IReadOnlyList<ConfigDetailRow> Items,
    DateTimeOffset? ExecutarUpdatedAt,
    int ExecutarValue,
    long QueryMs);

public sealed record ServiceControlResult(
    bool Success,
    string Status,
    string? Message,
    string? CommandId = null);
