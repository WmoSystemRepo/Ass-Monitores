namespace Orquestrador.Api.Realtime;

public sealed class MonitorPushOptions
{
    public const string SectionName = "MonitorPush";

    /// <summary>Intervalo de push de snapshot por serviço com assinante (ms). Dev: 1000 como CT_e 2.0.</summary>
    public int SnapshotIntervalMs { get; set; } = 1000;

    /// <summary>Intervalo de push incremental de logs (ms).</summary>
    public int LogsIntervalMs { get; set; } = 1000;

    public int RecentLogsTake { get; set; } = 100;
}
