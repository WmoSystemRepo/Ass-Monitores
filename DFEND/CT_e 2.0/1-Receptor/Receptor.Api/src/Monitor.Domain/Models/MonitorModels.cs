using Monitor.Domain.Alerts;

namespace Monitor.Domain.Models;

public enum ConnectionHealth
{
    Healthy,
    Degraded,
    Down
}

public sealed record ServiceStatusView(
    string WindowsServiceName,
    string ScmStatus,
    bool IsRunning,
    int Executar,
    string? NomServidor,
    DateTimeOffset? DtcExecucao,
    string? DesServico);

public sealed record ThreadView(
    int ThreadId,
    string Role,
    string NsuSource,
    int IndDFe,
    string? NsuAtual,
    bool IsIdle,
    bool OutsideDatabase,
    string? LastActivityHint,
    DateTimeOffset? LastActivityAt = null,
    string? LastCStat = null,
    string? LastSeverityHint = null);

public sealed record QueueStats(
    long TempBacklog,
    long ServiceBrokerDepth,
    DateTimeOffset? OldestTempAt,
    IReadOnlyList<long> BrokerDepthTrend);

public sealed record RecentDocument(
    long Nsu,
    long? NsuFinal,
    int QtdDocumento,
    DateTimeOffset? DtcDocumento,
    DateTimeOffset? DtcAtualizacao,
    string? MensagemErro,
    bool HasError);

public sealed record LogEntry(
    long SeqLog,
    DateTimeOffset? DtcLog,
    string? Mensagem,
    int? ThreadId,
    string? CStat,
    string SeverityHint);

public sealed record ConfigItem(string Key, string Value);

public sealed record GlobalStatus(
    ServiceStatusView Service,
    int IntervaloSeconds,
    int PacoteCompleto,
    int ReBuscar,
    int ConfiguredThreads,
    string? MainNsu,
    DateTimeOffset SnapshotAtUtc);

public sealed record MonitorSnapshot(
    GlobalStatus Global,
    IReadOnlyList<ThreadView> Threads,
    QueueStats Queues,
    IReadOnlyList<RecentDocument> RecentDocuments,
    IReadOnlyList<MonitorAlert> Alerts,
    IReadOnlyList<ConfigItem> Config,
    ConnectionHealth ConnectionHealth,
    string? ConnectionError,
    IReadOnlyList<LiveTraceLine> LiveTrace,
    DateTimeOffset? SessionStartUtc = null,
    IReadOnlyList<TableHealthView>? TableHealth = null);

/// <summary>Saúde/latência de uma tabela (ou fila) no snapshot em tempo real.</summary>
public sealed record TableHealthView(
    string Key,
    string Label,
    string Status,
    string PrimaryValue,
    double? DataAgeSeconds,
    long QueryMs,
    string Hint,
    string Route);

/// <summary>Detalhe sob demanda de uma tabela na sessão atual.</summary>
public sealed record TableDetailDto(
    string Key,
    string Label,
    DateTimeOffset? SessionStartUtc,
    bool ReceptionOn,
    string? BannerMessage,
    TableHealthView Health,
    IReadOnlyList<ServiceDetailRow>? ServiceRows,
    IReadOnlyList<ConfigDetailRow>? ConfigRows,
    IReadOnlyList<RecentDocument>? TempRows,
    IReadOnlyList<LogEntry>? LogRows,
    FilaDetailView? Fila,
    IReadOnlyList<LogEntry>? ContextLogs,
    int TakeApplied = 0,
    int RowCount = 0);

public sealed record ServiceDetailRow(
    string? DesServico,
    string? NomServidor,
    string? Nsu,
    DateTimeOffset? DtcExecucao,
    DateTimeOffset? DtcAtualizacao);

public sealed record ConfigDetailRow(
    string Key,
    string Value,
    DateTimeOffset? DtcAtualizacao,
    bool IsProcessKey);

public sealed record FilaDetailView(
    long Depth,
    IReadOnlyList<long> DepthTrend,
    int HighThreshold,
    string TrendHint);

/// <summary>Linha do monitor-live.log (Debug.WriteLine do DevHost/Receptor).</summary>
public sealed record LiveTraceLine(
    DateTimeOffset At,
    string Message,
    string? Step,
    string Source);
