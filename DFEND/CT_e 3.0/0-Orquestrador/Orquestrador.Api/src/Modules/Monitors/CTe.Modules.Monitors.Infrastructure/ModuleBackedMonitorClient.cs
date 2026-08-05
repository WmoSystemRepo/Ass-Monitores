using System.Text.Json;
using CTe.Modules.Monitors.Abstractions;
using Microsoft.Extensions.Logging;
using Orquestrador.Application.Abstractions;
using Orquestrador.Application.Options;

namespace CTe.Modules.Monitors.Infrastructure;

/// <summary>
/// W3: <see cref="IMonitorClient"/> backed by <see cref="IMonitorModuleRegistry"/> (in-process).
/// Cascade Ligar/Desligar deixa de depender do Monitor.Api HTTP para os 6 monitores.
/// </summary>
public sealed class ModuleBackedMonitorClient : IMonitorClient
{
    private readonly IMonitorModuleRegistry _registry;
    private readonly ILogger<ModuleBackedMonitorClient> _logger;

    public ModuleBackedMonitorClient(
        IMonitorModuleRegistry registry,
        ILogger<ModuleBackedMonitorClient> logger)
    {
        _registry = registry;
        _logger = logger;
    }

    public async Task<bool> PingReadyAsync(OrchestratorSystemOptions system, CancellationToken ct)
    {
        var module = _registry.Get(system.Id);
        if (module is null)
        {
            return false;
        }

        try
        {
            var health = await module.GetHealthAsync(ct);
            if (health is null)
            {
                return false;
            }

            if (health is JsonElement el && el.ValueKind == JsonValueKind.Object)
            {
                var status = TryGetString(el, "status");
                return status is null
                    || status.Equals("ok", StringComparison.OrdinalIgnoreCase)
                    || status.Equals("ready", StringComparison.OrdinalIgnoreCase);
            }

            var prop = health.GetType().GetProperty("status") ?? health.GetType().GetProperty("Status");
            var value = prop?.GetValue(health)?.ToString();
            return value is null
                || value.Equals("ok", StringComparison.OrdinalIgnoreCase)
                || value.Equals("ready", StringComparison.OrdinalIgnoreCase)
                || value.Equals("unhealthy", StringComparison.OrdinalIgnoreCase); // módulo responde = reachable
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "PingReady in-process falhou para {Id}", system.Id);
            return false;
        }
    }

    public async Task<MonitorFetchResult<JsonDocument>> GetSnapshotAsync(
        OrchestratorSystemOptions system,
        CancellationToken ct)
    {
        var module = _registry.Get(system.Id);
        if (module is null)
        {
            return MonitorFetchResult<JsonDocument>.Fail("offline", "Módulo não registrado.", null);
        }

        try
        {
            var raw = await module.GetSnapshotAsync(ct);
            if (raw is null)
            {
                return MonitorFetchResult<JsonDocument>.Fail("error", "Snapshot vazio.", null);
            }

            var json = JsonSerializer.SerializeToUtf8Bytes(raw);
            var doc = JsonDocument.Parse(json);
            return MonitorFetchResult<JsonDocument>.Ok(doc);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "GetSnapshot in-process falhou para {Id}", system.Id);
            return MonitorFetchResult<JsonDocument>.Fail("error", ex.Message, null);
        }
    }

    public async Task<MonitorFetchResult<MonitorServiceStatusDto>> GetStatusAsync(
        OrchestratorSystemOptions system,
        CancellationToken ct)
    {
        var module = _registry.Get(system.Id);
        if (module is null)
        {
            return MonitorFetchResult<MonitorServiceStatusDto>.Fail("offline", "Módulo não registrado.", null);
        }

        try
        {
            var raw = await module.GetServiceStatusAsync(ct);
            if (raw is MonitorServiceStatusDto dto)
            {
                return MonitorFetchResult<MonitorServiceStatusDto>.Ok(dto);
            }

            if (raw is null)
            {
                return MonitorFetchResult<MonitorServiceStatusDto>.Fail("error", "Status vazio.", null);
            }

            return MonitorFetchResult<MonitorServiceStatusDto>.Ok(MapStatus(raw));
        }
        catch (Exception ex)
        {
            return MonitorFetchResult<MonitorServiceStatusDto>.Fail("error", ex.Message, null);
        }
    }

    public async Task<MonitorActionResult> StartAsync(OrchestratorSystemOptions system, CancellationToken ct)
    {
        var module = _registry.Get(system.Id);
        if (module is null)
        {
            return new MonitorActionResult(false, "NotFound", "Módulo não registrado.");
        }

        var raw = await module.StartAsync(ct);
        return MapAction(raw);
    }

    public async Task<MonitorActionResult> StopAsync(OrchestratorSystemOptions system, CancellationToken ct)
    {
        var module = _registry.Get(system.Id);
        if (module is null)
        {
            return new MonitorActionResult(false, "NotFound", "Módulo não registrado.");
        }

        var raw = await module.StopAsync(ct);
        return MapAction(raw);
    }

    private static MonitorServiceStatusDto MapStatus(object raw)
    {
        if (raw is JsonElement el)
        {
            return new MonitorServiceStatusDto(
                TryGetBool(el, "success") ?? true,
                TryGetString(el, "status") ?? TryGetString(el, "scmStatus"),
                TryGetString(el, "message"),
                TryGetBool(el, "isRunning"),
                TryGetString(el, "scmStatus") ?? TryGetString(el, "status"),
                TryGetInt(el, "executar"),
                TryGetString(el, "commandId"));
        }

        var t = raw.GetType();
        return new MonitorServiceStatusDto(
            (bool?)t.GetProperty("Success")?.GetValue(raw) ?? true,
            (string?)t.GetProperty("Status")?.GetValue(raw),
            (string?)t.GetProperty("Message")?.GetValue(raw),
            (bool?)t.GetProperty("IsRunning")?.GetValue(raw),
            (string?)t.GetProperty("ScmStatus")?.GetValue(raw),
            (int?)t.GetProperty("Executar")?.GetValue(raw),
            (string?)t.GetProperty("CommandId")?.GetValue(raw));
    }

    private static MonitorActionResult MapAction(object? raw)
    {
        if (raw is null)
        {
            return new MonitorActionResult(false, "Error", "Sem resposta do módulo.");
        }

        if (raw is MonitorActionResult already)
        {
            return already;
        }

        if (raw is JsonElement el)
        {
            return new MonitorActionResult(
                TryGetBool(el, "success") ?? true,
                TryGetString(el, "status"),
                TryGetString(el, "message"),
                TryGetString(el, "commandId"));
        }

        var t = raw.GetType();
        return new MonitorActionResult(
            (bool?)t.GetProperty("Success")?.GetValue(raw) ?? true,
            (string?)t.GetProperty("Status")?.GetValue(raw),
            (string?)t.GetProperty("Message")?.GetValue(raw),
            (string?)t.GetProperty("CommandId")?.GetValue(raw));
    }

    private static string? TryGetString(JsonElement root, string name)
    {
        foreach (var prop in root.EnumerateObject())
        {
            if (!prop.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            return prop.Value.ValueKind == JsonValueKind.String ? prop.Value.GetString() : prop.Value.ToString();
        }

        return null;
    }

    private static bool? TryGetBool(JsonElement root, string name)
    {
        foreach (var prop in root.EnumerateObject())
        {
            if (!prop.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            return prop.Value.ValueKind switch
            {
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.String when bool.TryParse(prop.Value.GetString(), out var b) => b,
                _ => null
            };
        }

        return null;
    }

    private static int? TryGetInt(JsonElement root, string name)
    {
        foreach (var prop in root.EnumerateObject())
        {
            if (!prop.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (prop.Value.TryGetInt32(out var i))
            {
                return i;
            }
        }

        return null;
    }
}
