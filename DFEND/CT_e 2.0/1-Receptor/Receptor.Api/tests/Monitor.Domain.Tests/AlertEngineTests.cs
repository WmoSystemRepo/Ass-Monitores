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
        var service = new ServiceStatusView("DFEND_CTe_Receptor", "Running", true, 0, "DEV", DateTimeOffset.UtcNow, "Receptor");
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

        Assert.Contains(alerts, a => a.Code == "SVC_EXECUTAR_OFF");
    }

    [Fact]
    public void Evaluate_BdDown_IsCritical()
    {
        var service = new ServiceStatusView("DFEND_CTe_Receptor", "Stopped", false, 0, null, null, null);
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
}
