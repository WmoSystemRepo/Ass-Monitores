using CTe.Modules.Monitors.Abstractions;

namespace CTe.Modules.Monitors.Infrastructure;

public sealed class MonitorModuleRegistry : IMonitorModuleRegistry
{
    private readonly IReadOnlyDictionary<string, IMonitorModule> _modules;

    public MonitorModuleRegistry(IEnumerable<IMonitorModule> modules)
    {
        _modules = modules
            .GroupBy(m => m.ServiceId, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);
    }

    public IMonitorModule? Get(string serviceId) =>
        !string.IsNullOrWhiteSpace(serviceId) && _modules.TryGetValue(serviceId, out var module)
            ? module
            : null;

    public IReadOnlyList<string> ServiceIds => _modules.Keys.ToList();
}
