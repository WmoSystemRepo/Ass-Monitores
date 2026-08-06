using System.Collections.Concurrent;
using System.Text.Json;
using CTe.Modules.Monitors.Abstractions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace Orquestrador.Api.Realtime;

/// <summary>
/// Push SignalR por grupo de serviço — espelha CT_e 2.0 <c>MonitorPushHostedService</c>,
/// mas só para monitores com cliente conectado no hub unificado.
/// </summary>
public sealed class MonitorPushHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<MonitorHub> _hub;
    private readonly MonitorHubSubscriptions _subscriptions;
    private readonly MonitorPushOptions _options;
    private readonly ILogger<MonitorPushHostedService> _logger;
    private readonly ConcurrentDictionary<string, long> _lastLogSeq = new(StringComparer.OrdinalIgnoreCase);

    public MonitorPushHostedService(
        IServiceScopeFactory scopeFactory,
        IHubContext<MonitorHub> hub,
        MonitorHubSubscriptions subscriptions,
        IOptions<MonitorPushOptions> options,
        ILogger<MonitorPushHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _hub = hub;
        _subscriptions = subscriptions;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var snapshotDelay = TimeSpan.FromMilliseconds(Math.Max(500, _options.SnapshotIntervalMs));
        var logsDelay = TimeSpan.FromMilliseconds(Math.Max(500, _options.LogsIntervalMs));
        var nextSnapshot = DateTime.UtcNow;
        var nextLogs = DateTime.UtcNow;

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            try
            {
                if (!_subscriptions.HasAny)
                {
                    await Task.Delay(400, stoppingToken);
                    continue;
                }

                if (now >= nextSnapshot)
                {
                    await PushSnapshotsAsync(stoppingToken);
                    nextSnapshot = DateTime.UtcNow + snapshotDelay;
                }

                if (now >= nextLogs)
                {
                    await PushLogsAsync(stoppingToken);
                    nextLogs = DateTime.UtcNow + logsDelay;
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Falha no push SignalR dos monitores unificados");
                nextSnapshot = DateTime.UtcNow + snapshotDelay;
                nextLogs = DateTime.UtcNow + logsDelay;
            }

            await Task.Delay(200, stoppingToken);
        }
    }

    private async Task PushSnapshotsAsync(CancellationToken ct)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var registry = scope.ServiceProvider.GetRequiredService<IMonitorModuleRegistry>();

        foreach (var serviceId in _subscriptions.ActiveServiceIds)
        {
            var module = registry.Get(serviceId);
            if (module is null) continue;

            try
            {
                var snapshot = await module.GetSnapshotAsync(ct);
                if (snapshot is null) continue;
                await _hub.Clients.Group(serviceId).SendAsync("snapshot", snapshot, ct);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogDebug(ex, "Push snapshot falhou para {ServiceId}", serviceId);
            }
        }
    }

    private async Task PushLogsAsync(CancellationToken ct)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var registry = scope.ServiceProvider.GetRequiredService<IMonitorModuleRegistry>();

        foreach (var serviceId in _subscriptions.ActiveServiceIds)
        {
            var module = registry.Get(serviceId);
            if (module is null) continue;

            try
            {
                var after = _lastLogSeq.GetOrAdd(serviceId, 0);
                var payload = await module.GetLogsAsync(after, _options.RecentLogsTake, ct);
                var (entries, maxSeq) = ExtractLogs(payload, after);
                if (entries is null || entries.Count == 0) continue;

                if (maxSeq > after)
                {
                    _lastLogSeq[serviceId] = maxSeq;
                }

                await _hub.Clients.Group(serviceId).SendAsync("logsAppend", entries, ct);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogDebug(ex, "Push logs falhou para {ServiceId}", serviceId);
            }
        }
    }

    /// <summary>
    /// Aceita lista tipada, <see cref="JsonElement"/> array, ou wrapper — normaliza para lista enviável.
    /// </summary>
    private static (List<object>? Entries, long MaxSeq) ExtractLogs(object? payload, long afterSeq)
    {
        if (payload is null) return (null, afterSeq);

        if (payload is JsonElement el)
        {
            if (el.ValueKind != JsonValueKind.Array) return (null, afterSeq);
            return FromJsonArray(el, afterSeq);
        }

        if (payload is IEnumerable<object> objs)
        {
            var list = objs.ToList();
            if (list.Count == 0) return (null, afterSeq);
            var max = Math.Max(afterSeq, list.Max(GetSeqLog));
            return (list, max);
        }

        // Anonymous/DTO arrays via JSON round-trip (shape já serializável).
        try
        {
            var json = JsonSerializer.SerializeToElement(payload);
            if (json.ValueKind == JsonValueKind.Array)
            {
                return FromJsonArray(json, afterSeq);
            }
        }
        catch
        {
            /* ignore */
        }

        return (null, afterSeq);
    }

    private static (List<object>? Entries, long MaxSeq) FromJsonArray(JsonElement array, long afterSeq)
    {
        var list = new List<object>();
        long max = afterSeq;
        foreach (var item in array.EnumerateArray())
        {
            var seq = ReadSeq(item);
            if (seq > max) max = seq;
            list.Add(item);
        }

        return list.Count == 0 ? (null, afterSeq) : (list, max);
    }

    private static long GetSeqLog(object entry)
    {
        try
        {
            var el = JsonSerializer.SerializeToElement(entry);
            return ReadSeq(el);
        }
        catch
        {
            return 0;
        }
    }

    private static long ReadSeq(JsonElement item)
    {
        if (item.ValueKind != JsonValueKind.Object) return 0;
        if (item.TryGetProperty("seqLog", out var camel) && camel.TryGetInt64(out var a)) return a;
        if (item.TryGetProperty("SeqLog", out var pascal) && pascal.TryGetInt64(out var b)) return b;
        return 0;
    }
}
