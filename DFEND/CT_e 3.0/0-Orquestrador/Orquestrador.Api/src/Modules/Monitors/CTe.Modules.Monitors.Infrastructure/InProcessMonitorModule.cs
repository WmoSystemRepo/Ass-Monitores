using System.Runtime.Versioning;
using CTe.Modules.Monitors.Abstractions;
using CTe.Modules.Monitors.WindowsControl;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Orquestrador.Application.Abstractions;

namespace CTe.Modules.Monitors.Infrastructure;

/// <summary>
/// Implementação in-process de <see cref="IMonitorModule"/> (W3 — SDD Monitor Unificado):
/// start/stop/status/info/health não dependem do Monitor.Api do serviço estar de pé — falam
/// direto com o Windows Service/DevHost (<see cref="WindowsMonitorControlAdapter"/>) e com o SQL
/// (flag <c>Executar</c> + ping de saúde).
/// <para>
/// Regra operacional: <b>Ligar as filas</b> = subir processo + <c>Executar=1</c>;
/// <b>Desligar filas</b> = <c>Executar=0</c> + parar processo. Não há estado “pausado” na cascata.
/// </para>
/// <para>
/// Snapshot/logs/tables: paridade completa exigiria copiar o SqlMonitorRepository/SnapshotAggregator
/// de cada Monitor.Infrastructure (schemas diferentes por serviço). Enquanto isso, devolvem um
/// payload estruturado com status operacional + Executar (quando há ConnectionString), OU
/// (opt-in, <c>Monitors:{id}:UseHttpFallback=true</c>) delegam para o Monitor.Api via HTTP.
/// </para>
/// </summary>
[SupportedOSPlatform("windows")]
public sealed class InProcessMonitorModule : IMonitorModule
{
    private readonly MonitorControlOptions _options;
    private readonly IMonitorModule? _httpFallback;
    private readonly WindowsMonitorControlAdapter _control;
    private readonly ILogger<InProcessMonitorModule> _logger;

    public InProcessMonitorModule(
        MonitorControlOptions options,
        IMonitorModule? httpFallback,
        ILogger<InProcessMonitorModule> logger,
        params string?[] repoRootSearchStarts)
    {
        _options = options;
        _httpFallback = httpFallback;
        _logger = logger;
        _control = new WindowsMonitorControlAdapter(options, repoRootSearchStarts);
    }

    public string ServiceId => _options.ServiceId;

    public Task<object?> GetInfoAsync(CancellationToken ct) => Task.FromResult<object?>(new
    {
        serviceId = _options.ServiceId,
        displayName = _options.DisplayName,
        domain = _options.Domain,
        monitoredService = _options.MonitoredService,
        mode = "in-process",
        windowsServiceName = _options.WindowsServiceName,
        codServico = _options.CodServico,
        hasConnectionString = !string.IsNullOrWhiteSpace(_options.ConnectionString),
        useHttpFallback = _options.UseHttpFallback,
        endpoints = new[]
        {
            "/api/monitores/{servico}/snapshot",
            "/api/monitores/{servico}/logs",
            "/api/monitores/{servico}/tables/{key}",
            "/api/monitores/{servico}/service/status",
            "/api/monitores/{servico}/service/start",
            "/api/monitores/{servico}/service/stop",
            "/api/monitores/{servico}/health",
            "/api/monitores/{servico}/info"
        }
    });

    public async Task<object?> GetServiceStatusAsync(CancellationToken ct)
    {
        var result = _control.GetStatus();
        var processUp = result.Status.Equals("Running", StringComparison.OrdinalIgnoreCase);
        var executar = await ResolveExecutarAsync(processUp, ct);
        return ToStatusDto(result, executar);
    }

    public async Task<object?> StartAsync(CancellationToken ct)
    {
        var result = _control.Start();
        LogResult("start", result);
        if (!result.Success)
        {
            return ToActionResult(result);
        }

        var (ok, err) = await ExecutarFlagSql.SetAsync(
            _options.ConnectionString,
            _options.Domain,
            _options.CodServico,
            value: 1,
            _options.SqlTimeoutSeconds,
            _logger,
            ct);

        var msg = result.Message ?? $"{_options.DisplayName} ligado.";
        if (ok)
        {
            msg = $"{msg} Filas em execução (Executar=1).";
        }
        else if (!string.IsNullOrWhiteSpace(err))
        {
            msg = $"{msg} Aviso: {err}";
            _logger.LogWarning(
                "Start {ServiceId}: processo ok, mas Executar=1 falhou — {Err}",
                ServiceId,
                err);
        }

        return new MonitorActionResult(true, "Running", msg, result.CommandId);
    }

    public async Task<object?> StopAsync(CancellationToken ct)
    {
        var (ok, err) = await ExecutarFlagSql.SetAsync(
            _options.ConnectionString,
            _options.Domain,
            _options.CodServico,
            value: 0,
            _options.SqlTimeoutSeconds,
            _logger,
            ct);

        var result = _control.Stop();
        LogResult("stop", result);

        var msg = result.Message ?? $"{_options.DisplayName} desligado.";
        if (ok)
        {
            msg = $"{msg} Filas paradas (Executar=0).";
        }
        else if (!string.IsNullOrWhiteSpace(err))
        {
            msg = $"{msg} Aviso: {err}";
        }

        return new MonitorActionResult(
            result.Success,
            result.Status,
            msg,
            result.CommandId);
    }

    public async Task<object?> GetHealthAsync(CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(_options.ConnectionString))
        {
            var scm = _control.GetStatus();
            return new
            {
                status = "ok",
                mode = "in-process",
                scm = scm.Status,
                message = $"Monitors:{ServiceId}:ConnectionString vazio — health não valida SQL " +
                          "(não bloqueia o ready global do Orquestrador)."
            };
        }

        try
        {
            await using var conn = new SqlConnection(_options.ConnectionString);
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            linkedCts.CancelAfter(TimeSpan.FromSeconds(Math.Max(1, _options.SqlTimeoutSeconds)));
            await conn.OpenAsync(linkedCts.Token);
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT 1";
            cmd.CommandTimeout = Math.Max(1, _options.SqlTimeoutSeconds);
            await cmd.ExecuteScalarAsync(linkedCts.Token);
            return new { status = "ready", mode = "in-process", primary = true };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Health SQL falhou para {ServiceId}", ServiceId);
            return new { status = "unhealthy", mode = "in-process", primary = false, detail = ex.Message };
        }
    }

    public async Task<object?> GetSnapshotAsync(CancellationToken ct)
    {
        if (_httpFallback is not null)
        {
            return await _httpFallback.GetSnapshotAsync(ct);
        }

        return await BuildLimitedSnapshotAsync(ct);
    }

    public async Task<object?> GetLogsAsync(long afterSeq, int take, CancellationToken ct)
    {
        if (_httpFallback is not null)
        {
            return await _httpFallback.GetLogsAsync(afterSeq, take, ct);
        }

        var logs = await MonitorTelemetrySql.ReadLogsOnlyAsync(
            _options.ConnectionString,
            _options.Domain,
            afterSeq,
            take,
            _options.SqlTimeoutSeconds,
            ct);
        return logs;
    }

    public Task<object?> GetTableAsync(string key, int take, CancellationToken ct) =>
        _httpFallback is not null
            ? _httpFallback.GetTableAsync(key, take, ct)
            : Task.FromResult<object?>(null);

    /// <summary>
    /// Snapshot operacional + telemetria SQL (filas/docs/logs/config) para animações do pipeline
    /// com paridade CT_e 2.0 — sem depender do Monitor.Api HTTP.
    /// </summary>
    private async Task<object> BuildLimitedSnapshotAsync(CancellationToken ct)
    {
        var status = _control.GetStatus();
        var processUp = status.Status.Equals("Running", StringComparison.OrdinalIgnoreCase);
        var hasSql = !string.IsNullOrWhiteSpace(_options.ConnectionString);

        var telemetry = await MonitorTelemetrySql.ReadSnapshotAsync(
            _options.ConnectionString,
            _options.Domain,
            _options.CodServico,
            _options.SqlTimeoutSeconds,
            _logger,
            ct);

        var executar = telemetry.Executar
            ?? await ResolveExecutarAsync(processUp, ct);
        var intervalo = MonitorTelemetrySql.ResolveIntervaloSeconds(telemetry.Configs);
        _ = telemetry.Configs.TryGetValue("PacoteCompleto", out var pacoteRaw);
        _ = telemetry.Configs.TryGetValue("ReBuscar", out var reBuscarRaw);
        _ = telemetry.Configs.TryGetValue("Threads", out var threadsRaw);
        _ = int.TryParse(pacoteRaw, out var pacoteCompleto);
        _ = int.TryParse(reBuscarRaw, out var reBuscar);
        _ = int.TryParse(threadsRaw, out var configuredThreads);

        var configItems = telemetry.Configs
            .OrderBy(kv => kv.Key, StringComparer.OrdinalIgnoreCase)
            .Select(kv => new { key = kv.Key, value = kv.Value })
            .ToList();

        return new
        {
            mode = hasSql ? "in-process" : "in-process-limited",
            limitation = hasSql
                ? null
                : $"Snapshot completo requer Monitors:{ServiceId}:ConnectionString (filas/docs/logs/Executar).",
            global = new
            {
                service = new
                {
                    windowsServiceName = _options.WindowsServiceName,
                    desServico = telemetry.Service?.DesServico ?? _options.DisplayName,
                    nomServidor = telemetry.Service?.NomServidor,
                    dtcExecucao = telemetry.Service?.DtcExecucao,
                    scmStatus = status.Status,
                    isRunning = processUp,
                    executar,
                    executarKnown = executar is not null
                },
                intervaloSeconds = intervalo,
                pacoteCompleto,
                reBuscar,
                configuredThreads = configuredThreads > 0 ? configuredThreads : (int?)null,
                mainNsu = telemetry.Service?.MainNsu,
                snapshotAtUtc = DateTimeOffset.UtcNow
            },
            queues = new
            {
                tempBacklog = telemetry.TempBacklog,
                serviceBrokerDepth = telemetry.BrokerDepth,
                oldestTempAt = telemetry.OldestTempAt,
                brokerDepthTrend = Array.Empty<long>()
            },
            connectionHealth = !hasSql
                ? "SemDados"
                : telemetry.Service is not null || telemetry.Configs.Count > 0
                    ? "Healthy"
                    : "Down",
            codServico = _options.CodServico,
            threads = Array.Empty<object>(),
            recentDocuments = telemetry.RecentDocs,
            liveTrace = Array.Empty<object>(),
            alerts = BuildHealthAlerts(
                processUp,
                executar,
                hasSql,
                telemetry.TempBacklog,
                telemetry.BrokerDepth,
                telemetry.Service?.DtcExecucao,
                telemetry.Service?.NomServidor),
            config = configItems,
            sessionStartUtc = (DateTimeOffset?)null
        };
    }

    private static List<object> BuildHealthAlerts(
        bool processUp,
        int? executar,
        bool hasSql,
        long tempBacklog,
        long brokerDepth,
        DateTimeOffset? dtcExecucao,
        string? servidor)
    {
        var now = DateTimeOffset.UtcNow;
        var list = new List<object>();

        void Add(string code, string severity, string message) =>
            list.Add(new
            {
                code,
                severity,
                message,
                detectedAtUtc = now
            });

        if (!hasSql)
        {
            Add("SQL_CFG", "Alerta", "Sem connection string — filas e lotes não podem ser lidos.");
        }

        if (!processUp)
        {
            Add("PROC_OFF", "Atenção", "Processo parado. Use Ligar o fluxo no monitor para começar a processar a fila.");
        }
        else if (executar is 0)
        {
            Add("EXEC_0", "Atenção", "Processo no ar, mas Executar=0 — a fila não está consumindo trabalho.");
        }

        if (dtcExecucao is not null)
        {
            var age = now - dtcExecucao.Value.ToUniversalTime();
            if (age.TotalHours > 2)
            {
                var host = string.IsNullOrWhiteSpace(servidor) ? "servidor" : servidor;
                Add(
                    "SVC_STALE",
                    "Alerta",
                    $"Última batida em {host} desatualizada (há {(int)age.TotalHours}h). Verifique se o serviço está vivo.");
            }
        }

        if (tempBacklog > 0)
        {
            Add("TEMP_BACKLOG", "Info", $"{tempBacklog} documento(s) na temporária aguardando.");
        }

        if (brokerDepth > 0)
        {
            Add("FILA_BACKLOG", "Info", $"{brokerDepth} item(ns) na fila do Service Broker.");
        }

        if (list.Count == 0 && processUp)
        {
            Add("OK", "Info", "Sem alertas — serviço ligado e telemetria estável.");
        }

        return list;
    }

    /// <summary>
    /// Lê Executar no SQL; se processo está no ar e SQL ausente/falhou, assume 1
    /// (Ligar as filas = processo + trabalho — sem estado pausado na cascata).
    /// </summary>
    private async Task<int?> ResolveExecutarAsync(bool processUp, CancellationToken ct)
    {
        var fromSql = await ExecutarFlagSql.TryGetAsync(
            _options.ConnectionString,
            _options.Domain,
            _options.CodServico,
            _options.SqlTimeoutSeconds,
            ct);

        if (fromSql is not null)
        {
            return fromSql;
        }

        // Sem SQL: processo no ar ⇒ filas ligadas (não reportar “pausado”).
        return processUp ? 1 : 0;
    }

    private void LogResult(string operation, ServiceControlResult result)
    {
        if (result.Success)
        {
            _logger.LogInformation(
                "Monitor {ServiceId} {Operation}: {Status} — {Message}",
                ServiceId,
                operation,
                result.Status,
                result.Message);
        }
        else
        {
            _logger.LogWarning(
                "Monitor {ServiceId} {Operation} falhou: {Status} — {Message}",
                ServiceId,
                operation,
                result.Status,
                result.Message);
        }
    }

    private static MonitorServiceStatusDto ToStatusDto(ServiceControlResult result, int? executar)
    {
        var running = result.Status.Equals("Running", StringComparison.OrdinalIgnoreCase);
        return new(
            result.Success,
            result.Status,
            result.Message,
            running,
            result.Status,
            Executar: executar,
            result.CommandId);
    }

    private static MonitorActionResult ToActionResult(ServiceControlResult result) => new(
        result.Success,
        result.Status,
        result.Message,
        result.CommandId);
}
