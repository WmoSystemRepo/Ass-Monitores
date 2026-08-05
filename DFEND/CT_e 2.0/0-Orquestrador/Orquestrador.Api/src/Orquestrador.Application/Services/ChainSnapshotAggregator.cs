using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orquestrador.Application.Abstractions;
using Orquestrador.Application.Options;
using Orquestrador.Domain.Models;

namespace Orquestrador.Application.Services;

public sealed class ChainSnapshotAggregator
{
    private readonly IMonitorClient _client;
    private readonly CascadeControlService _cascade;
    private readonly OrchestratorOptions _options;
    private readonly ILogger<ChainSnapshotAggregator> _logger;

    public ChainSnapshotAggregator(
        IMonitorClient client,
        CascadeControlService cascade,
        IOptions<OrchestratorOptions> options,
        ILogger<ChainSnapshotAggregator> logger)
    {
        _client = client;
        _cascade = cascade;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<ChainSnapshot> BuildAsync(CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;
        var systems = new List<ChainSystemView>();
        var alerts = new List<string>();
        LastLoteView? lastLote = null;

        var anyRunning = false;
        var anyTraffic = false;

        foreach (var cfg in _options.Systems)
        {
            // Resgate e outros sidecars: API sobe no boot, mas não aparecem no fluxograma R→C.
            if (!cfg.InCascade)
            {
                continue;
            }

            if (!cfg.Enabled)
            {
                systems.Add(DisabledView(cfg));
                continue;
            }

            try
            {
                var ready = await _client.PingReadyAsync(cfg, ct);
                if (!ready)
                {
                    systems.Add(ReachabilityView(cfg, "offline", "Monitor não ready (/health/ready)."));
                    alerts.Add($"{cfg.DisplayName}: monitor indisponível.");
                    continue;
                }

                var snapshotResult = await _client.GetSnapshotAsync(cfg, ct);
                if (snapshotResult.ErrorKind is "unauthorized")
                {
                    systems.Add(ReachabilityView(cfg, "unauthorized", snapshotResult.Message));
                    alerts.Add($"{cfg.DisplayName}: autenticação interna rejeitada.");
                    snapshotResult.Value?.Dispose();
                    continue;
                }

                if (snapshotResult.ErrorKind is "offline")
                {
                    systems.Add(ReachabilityView(cfg, "offline", snapshotResult.Message));
                    alerts.Add($"{cfg.DisplayName}: monitor indisponível.");
                    continue;
                }

                var statusResult = await _client.GetStatusAsync(cfg, ct);
                if (statusResult.ErrorKind is "unauthorized")
                {
                    systems.Add(ReachabilityView(cfg, "unauthorized", statusResult.Message));
                    alerts.Add($"{cfg.DisplayName}: autenticação interna rejeitada.");
                    snapshotResult.Value?.Dispose();
                    continue;
                }

                using (snapshotResult.Value)
                {
                    var status = statusResult.ErrorKind is null ? statusResult.Value : null;
                    var parsed = ParseMonitorPayload(cfg, snapshotResult.Value, status);
                    systems.Add(parsed.View);

                    if (parsed.IsRunning)
                    {
                        anyRunning = true;
                    }

                    if (parsed.HasTraffic)
                    {
                        anyTraffic = true;
                    }

                    if (cfg.Id.Equals("receptor", StringComparison.OrdinalIgnoreCase) && parsed.LastLote is not null)
                    {
                        lastLote = parsed.LastLote;
                    }

                    if (!string.IsNullOrWhiteSpace(parsed.View.LastError))
                    {
                        alerts.Add($"{cfg.DisplayName}: {parsed.View.LastError}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha ao agregar {Id}", cfg.Id);
                systems.Add(ReachabilityView(cfg, "offline", ex.Message));
                alerts.Add($"{cfg.DisplayName}: {ex.Message}");
            }
        }

        // AGORA = ligado com fila; ActiveIds também inclui quem só tem backlog (NA FILA).
        var activeIds = systems
            .Where(s => s.Agora || s.HasQueueWork)
            .Select(s => s.Id)
            .ToList();
        var (phase, message) = _cascade.GetStatus();

        // Se cascade idle mas algum sistema running → refletir Running no phase display
        var phaseText = ToPhaseString(phase);
        if (phase == CascadePhase.Idle && anyRunning)
        {
            phaseText = ToPhaseString(CascadePhase.Running);
        }

        var anyQueue = systems.Any(s => s.HasQueueWork);

        return new ChainSnapshot(
            systems,
            activeIds,
            phaseText,
            lastLote,
            alerts,
            message,
            now,
            BeltMoving: (anyRunning && anyTraffic) || (anyRunning && anyQueue));
    }

    private static ChainSystemView DisabledView(OrchestratorSystemOptions cfg) =>
        new(
            cfg.Id,
            cfg.Symbol,
            string.IsNullOrWhiteSpace(cfg.DisplayName) ? cfg.Id : cfg.DisplayName,
            ToOfficialStatus(OfficialMonitorState.Disabled),
            Executar: 0,
            ScmStatus: null,
            Agora: false,
            MetricPill: "disabled",
            Hint: "Monitor ainda não disponível neste estágio.",
            LastError: null,
            Enabled: false,
            FrontendUrl: NullIfWhiteSpace(cfg.ResolveFrontendUrl()),
            Version: cfg.Version,
            UiIcon: cfg.Ui?.Icon,
            UiColor: cfg.Ui?.Color);

    /// <summary>reachability: offline | unauthorized</summary>
    private static ChainSystemView ReachabilityView(OrchestratorSystemOptions cfg, string reachability, string? lastError) =>
        new(
            cfg.Id,
            cfg.Symbol,
            string.IsNullOrWhiteSpace(cfg.DisplayName) ? cfg.Id : cfg.DisplayName,
            reachability == "unauthorized"
                ? ToOfficialStatus(OfficialMonitorState.Failed)
                : ToOfficialStatus(OfficialMonitorState.Offline),
            Executar: 0,
            ScmStatus: null,
            Agora: false,
            MetricPill: reachability,
            Hint: reachability switch
            {
                "unauthorized" => "API key interna inválida ou ausente no monitor.",
                _ => "Sem resposta do monitor."
            },
            LastError: lastError,
            Enabled: true,
            FrontendUrl: NullIfWhiteSpace(cfg.ResolveFrontendUrl()),
            Version: cfg.Version,
            UiIcon: cfg.Ui?.Icon,
            UiColor: cfg.Ui?.Color);

    private static ParsedSystem ParseMonitorPayload(
        OrchestratorSystemOptions cfg,
        JsonDocument? snapshotDoc,
        MonitorServiceStatusDto? status)
    {
        var root = snapshotDoc?.RootElement;

        var executar = TryGetExecutar(root, status);
        var isRunning = TryGetIsRunning(root, status);
        var scmStatus = TryGetScmStatus(root, status);
        var mainNsu = TryGetMainNsu(root);
        var brokerDepth = TryGetInt64(root, "queues", "serviceBrokerDepth") ?? 0;
        var tempBacklog = TryGetInt64(root, "queues", "tempBacklog") ?? 0;
        var stagingDepth = TryGetInt64(root, "queues", "stagingDepth") ?? 0;
        var hasRecentDocs = HasRecentDocuments(root);
        var lastLote = TryGetLastLote(root);
        var processHint = TryGetProcessHint(root);

        var scmLooksRunning =
            !string.IsNullOrWhiteSpace(scmStatus) &&
            (scmStatus.Contains("Running", StringComparison.OrdinalIgnoreCase) ||
             scmStatus.Contains("ligado", StringComparison.OrdinalIgnoreCase));

        var effectivelyRunning = isRunning == true || scmLooksRunning;
        var queueDepth = ResolveQueueDepth(cfg.Id, brokerDepth, stagingDepth, tempBacklog);
        var hasQueueWork = queueDepth > 0;
        // AGORA = serviço ligado E com trabalho na fila/trânsito (igual destaque dos monitores).
        var agora = IsAgoraActive(effectivelyRunning, executar, hasQueueWork, hasRecentDocs);

        var runtime = EffectiveRuntime(effectivelyRunning, status);

        var (pill, hint) = BuildMetric(cfg.Id, mainNsu, brokerDepth, stagingDepth, tempBacklog, processHint, hasQueueWork);

        var view = new ChainSystemView(
            cfg.Id,
            cfg.Symbol,
            string.IsNullOrWhiteSpace(cfg.DisplayName) ? cfg.Id : cfg.DisplayName,
            ToStatusString(runtime),
            executar,
            scmStatus,
            agora,
            MetricPill: string.IsNullOrWhiteSpace(pill) ? "online" : pill,
            hint,
            status is { Success: false } ? status.Message : null,
            Enabled: true,
            FrontendUrl: NullIfWhiteSpace(cfg.ResolveFrontendUrl()),
            Version: cfg.Version,
            UiIcon: cfg.Ui?.Icon,
            UiColor: cfg.Ui?.Color,
            HasQueueWork: hasQueueWork,
            QueueDepth: queueDepth,
            ProcessHint: processHint);

        var hasTraffic = hasQueueWork || hasRecentDocs;

        return new ParsedSystem(view, effectivelyRunning && executar == 1, hasTraffic, lastLote);
    }

    private static bool IsAgoraActive(
        bool effectivelyRunning,
        int executar,
        bool hasQueueWork,
        bool hasRecentDocs) =>
        effectivelyRunning && executar == 1 && (hasQueueWork || hasRecentDocs);

    private static long ResolveQueueDepth(string id, long brokerDepth, long stagingDepth, long tempBacklog)
    {
        if (id.Equals("integrador", StringComparison.OrdinalIgnoreCase))
        {
            return stagingDepth > 0 ? stagingDepth : brokerDepth;
        }

        if (id.Equals("receptor", StringComparison.OrdinalIgnoreCase))
        {
            return tempBacklog > 0 ? tempBacklog : brokerDepth;
        }

        return brokerDepth > 0 ? brokerDepth : tempBacklog;
    }

    private static string? TryGetProcessHint(JsonElement? root)
    {
        if (root is null ||
            !TryGetPropertyIgnoreCase(root.Value, "threads", out var arr) ||
            arr.ValueKind != JsonValueKind.Array)
        {
            return null;
        }

        string? fallback = null;
        foreach (var t in arr.EnumerateArray())
        {
            var isIdle = false;
            if (TryGetPropertyIgnoreCase(t, "isIdle", out var idleEl))
            {
                isIdle = idleEl.ValueKind == JsonValueKind.True ||
                         (idleEl.ValueKind == JsonValueKind.String &&
                          bool.TryParse(idleEl.GetString(), out var b) && b);
            }

            var role = TryGetStringProp(t, "role") ?? TryGetStringProp(t, "Role");
            var activity = TryGetStringProp(t, "lastActivityHint") ?? TryGetStringProp(t, "LastActivityHint");
            var nsu = TryGetStringProp(t, "nsuAtual") ?? TryGetStringProp(t, "NsuAtual");

            if (string.IsNullOrWhiteSpace(role) && string.IsNullOrWhiteSpace(activity))
            {
                continue;
            }

            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(role))
            {
                parts.Add(role.Trim());
            }

            if (!string.IsNullOrWhiteSpace(nsu))
            {
                parts.Add($"NSU {nsu.Trim()}");
            }
            else if (!string.IsNullOrWhiteSpace(activity))
            {
                var shortAct = activity.Trim();
                if (shortAct.Length > 72)
                {
                    shortAct = shortAct[..69] + "…";
                }

                parts.Add(shortAct);
            }

            var text = string.Join(" · ", parts);
            if (!isIdle)
            {
                return text;
            }

            fallback ??= text;
        }

        return fallback;
    }

    private static string? TryGetStringProp(JsonElement el, string name)
    {
        if (!TryGetPropertyIgnoreCase(el, name, out var p) ||
            p.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return null;
        }

        return p.ValueKind == JsonValueKind.String ? p.GetString() : p.ToString();
    }

    private static string? NullIfWhiteSpace(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static SystemRuntimeStatus EffectiveRuntime(bool effectivelyRunning, MonitorServiceStatusDto? status)
    {
        if (status?.Status is { } s)
        {
            if (s.Contains("start", StringComparison.OrdinalIgnoreCase) &&
                !s.Contains("Running", StringComparison.OrdinalIgnoreCase))
            {
                return SystemRuntimeStatus.Starting;
            }

            if (s.Contains("stop", StringComparison.OrdinalIgnoreCase) &&
                !s.Contains("Running", StringComparison.OrdinalIgnoreCase))
            {
                return SystemRuntimeStatus.Stopping;
            }
        }

        return effectivelyRunning ? SystemRuntimeStatus.Running : SystemRuntimeStatus.Off;
    }

    private static (string Pill, string Hint) BuildMetric(
        string id,
        string? mainNsu,
        long brokerDepth,
        long stagingDepth = 0,
        long tempBacklog = 0,
        string? processHint = null,
        bool hasQueueWork = false)
    {
        string pill;
        string hint;

        if (id.Equals("receptor", StringComparison.OrdinalIgnoreCase))
        {
            var nsu = string.IsNullOrWhiteSpace(mainNsu) ? "—" : mainNsu;
            pill = $"NSU {nsu}";
            hint = tempBacklog > 0
                ? $"Temporária {tempBacklog} · processando lote"
                : "Último NSU principal do Receptor.";
        }
        else if (id.Equals("arquivador", StringComparison.OrdinalIgnoreCase))
        {
            pill = $"fila {brokerDepth}";
            hint = hasQueueWork
                ? $"Processando fila Service Broker ({brokerDepth})"
                : "Profundidade da fila Service Broker.";
        }
        else if (id.Equals("sintetizador", StringComparison.OrdinalIgnoreCase))
        {
            pill = $"fila {brokerDepth}";
            hint = hasQueueWork
                ? $"Processando fila do Sintetizador ({brokerDepth})"
                : "Profundidade da fila do Sintetizador.";
        }
        else if (id.Equals("analisador", StringComparison.OrdinalIgnoreCase))
        {
            pill = $"fila {brokerDepth}";
            hint = hasQueueWork
                ? $"Processando fila do Analisador ({brokerDepth})"
                : "Profundidade da fila do Analisador.";
        }
        else if (id.Equals("integrador", StringComparison.OrdinalIgnoreCase))
        {
            pill = $"staging {stagingDepth}";
            hint = hasQueueWork
                ? $"Processando staging / Netezza ({stagingDepth})"
                : "Profundidade do staging / Netezza.";
        }
        else if (id.Equals("carga", StringComparison.OrdinalIgnoreCase))
        {
            pill = $"fila {brokerDepth}";
            hint = hasQueueWork
                ? $"Processando fila da Carga ({brokerDepth})"
                : "Profundidade da fila da Carga.";
        }
        else
        {
            pill = "—";
            hint = "Aguardando telemetria do monitor.";
        }

        if (!string.IsNullOrWhiteSpace(processHint))
        {
            hint = processHint!;
        }

        return (pill, hint);
    }

    private static int TryGetExecutar(JsonElement? root, MonitorServiceStatusDto? status)
    {
        if (status?.Executar is int e)
        {
            return e;
        }

        var fromGlobal = TryGetInt32(root, "global", "service", "executar");
        if (fromGlobal.HasValue)
        {
            return fromGlobal.Value;
        }

        return TryGetInt32(root, "global", "service", "Executar") ?? 0;
    }

    private static bool? TryGetIsRunning(JsonElement? root, MonitorServiceStatusDto? status)
    {
        if (status?.IsRunning is bool b)
        {
            return b;
        }

        if (TryGetBool(root, "global", "service", "isRunning") is bool fromGlobal)
        {
            return fromGlobal;
        }

        return TryGetBool(root, "isRunning");
    }

    private static string? TryGetScmStatus(JsonElement? root, MonitorServiceStatusDto? status)
    {
        if (!string.IsNullOrWhiteSpace(status?.ScmStatus))
        {
            return status.ScmStatus;
        }

        if (!string.IsNullOrWhiteSpace(status?.Status))
        {
            return status.Status;
        }

        return TryGetString(root, "global", "service", "scmStatus")
               ?? TryGetString(root, "global", "service", "status");
    }

    private static string? TryGetMainNsu(JsonElement? root) =>
        TryGetString(root, "global", "mainNsu") ?? TryGetString(root, "global", "MainNsu");

    private static bool HasRecentDocuments(JsonElement? root)
    {
        if (root is null)
        {
            return false;
        }

        if (!TryGetPropertyIgnoreCase(root.Value, "recentDocuments", out var arr) ||
            arr.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        return arr.GetArrayLength() > 0;
    }

    private static LastLoteView? TryGetLastLote(JsonElement? root)
    {
        if (root is null)
        {
            return null;
        }

        if (!TryGetPropertyIgnoreCase(root.Value, "recentDocuments", out var arr) ||
            arr.ValueKind != JsonValueKind.Array ||
            arr.GetArrayLength() == 0)
        {
            return null;
        }

        var first = arr[0];
        return new LastLoteView(
            TryGetInt64Prop(first, "nsu"),
            TryGetInt64Prop(first, "nsuFinal"),
            (int?)TryGetInt64Prop(first, "qtdDocumento"),
            TryGetDateProp(first, "dtcDocumento") ?? TryGetDateProp(first, "dtcAtualizacao") ?? TryGetDateProp(first, "at"));
    }

    private static int? TryGetInt32(JsonElement? root, params string[] path)
    {
        var el = Navigate(root, path);
        if (el is null)
        {
            return null;
        }

        if (el.Value.ValueKind == JsonValueKind.Number && el.Value.TryGetInt32(out var n))
        {
            return n;
        }

        if (el.Value.ValueKind == JsonValueKind.String &&
            int.TryParse(el.Value.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed;
        }

        return null;
    }

    private static long? TryGetInt64(JsonElement? root, params string[] path)
    {
        var el = Navigate(root, path);
        if (el is null)
        {
            return null;
        }

        if (el.Value.ValueKind == JsonValueKind.Number && el.Value.TryGetInt64(out var n))
        {
            return n;
        }

        if (el.Value.ValueKind == JsonValueKind.String &&
            long.TryParse(el.Value.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed;
        }

        return null;
    }

    private static bool? TryGetBool(JsonElement? root, params string[] path)
    {
        var el = Navigate(root, path);
        if (el is null)
        {
            return null;
        }

        return el.Value.ValueKind switch
        {
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.String when bool.TryParse(el.Value.GetString(), out var b) => b,
            _ => null
        };
    }

    private static string? TryGetString(JsonElement? root, params string[] path)
    {
        var el = Navigate(root, path);
        if (el is null || el.Value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return null;
        }

        return el.Value.ValueKind == JsonValueKind.String ? el.Value.GetString() : el.Value.ToString();
    }

    private static JsonElement? Navigate(JsonElement? root, params string[] path)
    {
        if (root is null)
        {
            return null;
        }

        var current = root.Value;
        foreach (var segment in path)
        {
            if (!TryGetPropertyIgnoreCase(current, segment, out var next))
            {
                return null;
            }

            current = next;
        }

        return current;
    }

    private static bool TryGetPropertyIgnoreCase(JsonElement element, string name, out JsonElement value)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            value = default;
            return false;
        }

        foreach (var prop in element.EnumerateObject())
        {
            if (prop.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                value = prop.Value;
                return true;
            }
        }

        value = default;
        return false;
    }

    private static long? TryGetInt64Prop(JsonElement el, string name)
    {
        if (!TryGetPropertyIgnoreCase(el, name, out var p))
        {
            return null;
        }

        if (p.ValueKind == JsonValueKind.Number && p.TryGetInt64(out var n))
        {
            return n;
        }

        if (p.ValueKind == JsonValueKind.String &&
            long.TryParse(p.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed;
        }

        return null;
    }

    private static DateTimeOffset? TryGetDateProp(JsonElement el, string name)
    {
        if (!TryGetPropertyIgnoreCase(el, name, out var p))
        {
            return null;
        }

        if (p.ValueKind == JsonValueKind.String &&
            DateTimeOffset.TryParse(p.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dto))
        {
            return dto;
        }

        return null;
    }

    public static string ToOfficialStatus(OfficialMonitorState status) => status switch
    {
        OfficialMonitorState.Disabled => "disabled",
        OfficialMonitorState.Offline => "offline",
        OfficialMonitorState.Starting => "starting",
        OfficialMonitorState.Running => "running",
        OfficialMonitorState.Stopping => "stopping",
        OfficialMonitorState.Stopped => "stopped",
        OfficialMonitorState.Failed => "failed",
        OfficialMonitorState.Unknown => "unknown",
        _ => "unknown"
    };

    /// <summary>Compatível com UI legado (off/error/sem_monitor) + estados oficiais.</summary>
    public static string ToStatusString(SystemRuntimeStatus status) => status switch
    {
        SystemRuntimeStatus.Off => "stopped",
        SystemRuntimeStatus.Starting => "starting",
        SystemRuntimeStatus.Running => "running",
        SystemRuntimeStatus.Stopping => "stopping",
        SystemRuntimeStatus.Error => "failed",
        SystemRuntimeStatus.SemMonitor => "disabled",
        SystemRuntimeStatus.Offline => "offline",
        SystemRuntimeStatus.Unknown => "unknown",
        _ => "unknown"
    };

    public static string ToPhaseString(CascadePhase phase) => phase switch
    {
        CascadePhase.Idle => "idle",
        CascadePhase.Starting => "starting",
        CascadePhase.Running => "running",
        CascadePhase.Stopping => "stopping",
        _ => "idle"
    };

    private sealed record ParsedSystem(
        ChainSystemView View,
        bool IsRunning,
        bool HasTraffic,
        LastLoteView? LastLote);
}
