namespace Orquestrador.Domain.Models;

/// <summary>
/// Estados oficiais do monitor (contrato Orquestrador ↔ Monitor.Api ↔ UI).
/// Fonte: Worker → Banco → Monitor.Api → Orquestrador (Orq só consulta).
/// </summary>
public enum OfficialMonitorState
{
    Disabled,
    Offline,
    Starting,
    Running,
    Stopping,
    Stopped,
    Failed,
    Unknown
}

/// <summary>Alias legado — preferir <see cref="OfficialMonitorState"/>.</summary>
public enum SystemRuntimeStatus
{
    Off = OfficialMonitorState.Stopped,
    Starting = OfficialMonitorState.Starting,
    Running = OfficialMonitorState.Running,
    Stopping = OfficialMonitorState.Stopping,
    Error = OfficialMonitorState.Failed,
    SemMonitor = OfficialMonitorState.Disabled,
    Offline = OfficialMonitorState.Offline,
    Unknown = OfficialMonitorState.Unknown
}

public enum CascadePhase
{
    Idle,
    Starting,
    Running,
    Stopping
}

public sealed record ChainSystemView(
    string Id,
    string Symbol,
    string Label,
    string Status,
    int Executar,
    string? ScmStatus,
    bool Agora,
    string MetricPill,
    string Hint,
    string? LastError,
    bool Enabled,
    string? FrontendUrl,
    string? Version = null,
    string? UiIcon = null,
    string? UiColor = null,
    /// <summary>Há itens na fila/staging/temporária deste sistema.</summary>
    bool HasQueueWork = false,
    /// <summary>Profundidade relevante (broker, staging ou temp).</summary>
    long QueueDepth = 0,
    /// <summary>Thread/processo ativo quando a telemetria expõe (role + hint).</summary>
    string? ProcessHint = null);

public sealed record LastLoteView(
    long? Nsu,
    long? NsuFinal,
    int? QtdDocumento,
    DateTimeOffset? At);

public sealed record ChainSnapshot(
    IReadOnlyList<ChainSystemView> Systems,
    IReadOnlyList<string> ActiveIds,
    string CascadePhase,
    LastLoteView? LastLote,
    IReadOnlyList<string> Alerts,
    string? CascadeMessage,
    DateTimeOffset SnapshotAtUtc,
    bool BeltMoving);
