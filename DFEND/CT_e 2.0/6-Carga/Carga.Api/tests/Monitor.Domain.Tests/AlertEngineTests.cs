using Monitor.Domain.Alerts;
using Monitor.Domain.Models;
using Xunit;

namespace Monitor.Domain.Tests;

public class AlertEngineTests
{
    [Fact]
    public void ParseLogMessage_ExtractsThreadAndCStat()
    {
        var (thread, cStat) = AlertEngine.ParseLogMessage("Thread 2 processou cStat 118 com sucesso");
        Assert.Equal(2, thread);
        Assert.Equal("118", cStat);
    }

    [Fact]
    public void Evaluate_ExecutarOff_RaisesAtenção()
    {
        var service = new ServiceStatusView("DFEND_CTe_Carga", "Running", true, 0, "DEV", DateTimeOffset.UtcNow, "Carga");
        var alerts = AlertEngine.Evaluate(
            service,
            new QueueStats(0, 0, null, Array.Empty<long>()),
            Array.Empty<RecentDocument>(),
            Array.Empty<LogEntry>(),
            60,
            new AlertThresholdOptions(),
            ConnectionHealth.Healthy,
            null,
            DateTimeOffset.UtcNow);

        Assert.Contains(alerts, a => a.Code == "SVC_EXECUTAR_OFF"
            && a.Message.Contains("carga ociosa", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Evaluate_BdDown_IsCritical()
    {
        var service = new ServiceStatusView("DFEND_CTe_Carga", "Stopped", false, 0, null, null, null);
        var alerts = AlertEngine.Evaluate(
            service,
            new QueueStats(0, 0, null, Array.Empty<long>()),
            Array.Empty<RecentDocument>(),
            Array.Empty<LogEntry>(),
            60,
            new AlertThresholdOptions(),
            ConnectionHealth.Down,
            "timeout",
            DateTimeOffset.UtcNow);

        Assert.Contains(alerts, a => a.Code == "BD_DOWN" && a.Severity == AlertSeverity.Critico);
    }

    [Fact]
    public void Evaluate_DoesNotEmitReceptorOnlyAlerts()
    {
        var service = new ServiceStatusView("DFEND_CTe_Carga", "Running", true, 1, "DEV", DateTimeOffset.UtcNow, "Carga");
        var logs = new[]
        {
            new LogEntry(1, DateTimeOffset.UtcNow, "cStat 108 manutenção", 1, "108", "warning"),
            new LogEntry(2, DateTimeOffset.UtcNow, "cStat 285 certificado", 1, "285", "warning"),
            new LogEntry(3, DateTimeOffset.UtcNow, "PacoteCompleto não gravado", 1, null, "info"),
            new LogEntry(4, DateTimeOffset.UtcNow, "NSU inconsistente menor", 1, null, "info"),
        };

        var alerts = AlertEngine.Evaluate(
            service,
            new QueueStats(0, 0, null, Array.Empty<long>()),
            Array.Empty<RecentDocument>(),
            logs,
            60,
            new AlertThresholdOptions(),
            ConnectionHealth.Healthy,
            null,
            DateTimeOffset.UtcNow);

        Assert.DoesNotContain(alerts, a => a.Code is "CSTAT_108" or "CSTAT_285" or "PACOTE_BLOQUEADO" or "NSU_INCONSISTENTE");
    }
}
