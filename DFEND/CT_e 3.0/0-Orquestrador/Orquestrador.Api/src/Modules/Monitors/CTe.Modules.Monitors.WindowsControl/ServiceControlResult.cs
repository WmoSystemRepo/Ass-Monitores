namespace CTe.Modules.Monitors.WindowsControl;

/// <summary>Resultado de status/start/stop — mesmo shape do ServiceControlResult dos Monitor.Api.</summary>
public sealed record ServiceControlResult(bool Success, string Status, string? Message, string? CommandId = null);
