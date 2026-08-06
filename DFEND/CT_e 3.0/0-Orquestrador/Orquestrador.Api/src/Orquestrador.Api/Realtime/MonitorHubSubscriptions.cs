using System.Collections.Concurrent;

namespace Orquestrador.Api.Realtime;

/// <summary>
/// Conta assinantes por serviço no hub unificado — o push só consulta módulos com cliente ativo.
/// </summary>
public sealed class MonitorHubSubscriptions
{
    private readonly ConcurrentDictionary<string, int> _counts = new(StringComparer.OrdinalIgnoreCase);

    public void Join(string serviceId)
    {
        if (string.IsNullOrWhiteSpace(serviceId)) return;
        _counts.AddOrUpdate(Normalize(serviceId), 1, static (_, n) => n + 1);
    }

    public void Leave(string serviceId)
    {
        if (string.IsNullOrWhiteSpace(serviceId)) return;
        var key = Normalize(serviceId);
        _counts.AddOrUpdate(key, 0, static (_, n) => Math.Max(0, n - 1));
        if (_counts.TryGetValue(key, out var remaining) && remaining <= 0)
        {
            _counts.TryRemove(key, out _);
        }
    }

    public IReadOnlyList<string> ActiveServiceIds =>
        _counts.Where(kv => kv.Value > 0).Select(kv => kv.Key).ToList();

    public bool HasAny => _counts.Any(kv => kv.Value > 0);

    private static string Normalize(string serviceId) => serviceId.Trim().ToLowerInvariant();
}
