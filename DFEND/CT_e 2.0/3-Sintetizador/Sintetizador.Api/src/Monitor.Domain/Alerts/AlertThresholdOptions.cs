namespace Monitor.Domain.Alerts;

public sealed class AlertThresholdOptions
{
    public const string SectionName = "AlertThresholds";

    public int FilaAlta { get; set; } = 100;
    public int FilaCrescendoMinDepth { get; set; } = 20;
    public int FilaCrescendoSnapshots { get; set; } = 3;
    public int TmpErroWindowSize { get; set; } = 50;
    public int StaleMinutesFloor { get; set; } = 5;
    public int StaleIntervalMultiplier { get; set; } = 3;
}
