namespace Monitor.Domain.Alerts;

public sealed record MonitorAlert(
    string Code,
    AlertSeverity Severity,
    string Message,
    DateTimeOffset DetectedAtUtc);
