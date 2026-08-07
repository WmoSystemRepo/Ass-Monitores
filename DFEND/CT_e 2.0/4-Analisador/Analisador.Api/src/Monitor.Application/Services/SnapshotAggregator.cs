using Monitor.Application.Abstractions;
using Monitor.Domain.Alerts;
using Monitor.Domain.Models;
using Microsoft.Extensions.Options;

namespace Monitor.Application.Services;

public sealed class MonitorOptions
{
    public const string SectionName = "Monitor";

    public string ConnectionString { get; set; } = string.Empty;
    /// <summary>Opcional: segundo banco (Sintético) para profundidade das filas destino / health ready.</summary>
    public string ConnectionStringAnalitico { get; set; } = string.Empty;
    public int CodServicoAnalisador { get; set; } = 6;
    public string WindowsServiceName { get; set; } = "DFEND_CTe_Analisador";

    public string ServiceId { get; set; } = "dfend-cte-monitor-analisador";
    public string DisplayName { get; set; } = "Monitor Analisador CT-e";
    public string Domain { get; set; } = "analisador";
    public string ApiVersion { get; set; } = "1.0";
    public string MonitoredService { get; set; } = "DFEND_CTe_Analisador";

    /// <summary>
    /// API key serviço-a-serviço (Orquestrador → Monitor). Header: X-Cte-Internal-Api-Key.
    /// Obrigatória em Development, Homologacao e Production (mesmo valor no Orquestrador).
    /// Homolog/Prod: defina via Monitor__InternalApiKey (secret store).
    /// </summary>
    public string InternalApiKey { get; set; } = string.Empty;

    /// <summary>Opcional. Se vazio, sobe pastas até achar raiz com dfend-cte-analisador-windowsservices + Analisador.Api.</summary>
    public string AnalisadorRootPath { get; set; } = string.Empty;
    /// <summary>
    /// Host POC (tools/Analisador.DevHost) — NÃO é o Windows Service original.
    /// Relativo à raiz do clone Analisador (pasta com windowsservices + Analisador.Api).
    /// </summary>
    public string AnalisadorExeRelativePath { get; set; } =
        @"tools\Analisador.DevHost\bin\Debug\Analisador.DevHost.exe";
    /// <summary>Nome do processo do host POC (GetProcessesByName).</summary>
    public string AnalisadorProcessName { get; set; } = "Analisador.DevHost";
    /// <summary>POC: inicia host local sem InstallUtil. Se false, tenta SCM primeiro.</summary>
    public bool PreferLocalProcess { get; set; } = true;
    public int SqlTimeoutSeconds { get; set; } = 3;
    public int SnapshotIntervalMs { get; set; } = 1000;
    public int LogsIntervalMs { get; set; } = 1000;
    public int ConfigCacheSeconds { get; set; } = 30;
    public int RecentDocumentsTake { get; set; } = 50;
    public int RecentLogsTake { get; set; } = 300;
    /// <summary>Opcional: caminho absoluto do monitor-live.log. Se vazio, resolve ao lado do DevHost.exe.</summary>
    public string LiveTracePath { get; set; } = string.Empty;
    public int LiveTraceTake { get; set; } = 80;
    public int TableDetailTake { get; set; } = 1000;
}

public sealed class SnapshotAggregator : ISnapshotAggregator
{
    private readonly IMonitorReadRepository _read;
    private readonly IWindowsServiceController _scm;
    private readonly ILiveTraceReader _liveTrace;
    private readonly MonitorOptions _options;
    private readonly AlertThresholdOptions _thresholds;
    private static readonly object TrendLock = new();
    private static readonly List<long> BrokerTrend = new();
    private IReadOnlyDictionary<string, string> _configCache = new Dictionary<string, string>();
    private DateTimeOffset? _sessionStartCache;
    private long _configQueryMs;
    private DateTimeOffset _configCacheAt = DateTimeOffset.MinValue;
    private static string? LastMainNsu;

    public SnapshotAggregator(
        IMonitorReadRepository read,
        IWindowsServiceController scm,
        ILiveTraceReader liveTrace,
        IOptions<MonitorOptions> options,
        IOptions<AlertThresholdOptions> thresholds)
    {
        _read = read;
        _scm = scm;
        _liveTrace = liveTrace;
        _options = options.Value;
        _thresholds = thresholds.Value;
    }

    public async Task<MonitorSnapshot> BuildSnapshotAsync(CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;
        var live = _liveTrace.ReadRecent(_options.LiveTraceTake);
        try
        {
            var serviceResult = await _read.GetServiceAsync(_options.CodServicoAnalisador, ct);
            var serviceRow = serviceResult.Row;
            var (configs, sessionStart, configMs) = await GetConfigsCachedAsync(ct);
            var queues = await _read.GetQueueCountsAsync(ct);
            var docsResult = await _read.GetRecentDocumentsAsync(_options.RecentDocumentsTake, ct);
            var docs = docsResult.Items;
            var logsResult = await _read.GetLogsAfterAsync(0, _options.RecentLogsTake, ct);
            var logs = logsResult.Items;

            lock (TrendLock)
            {
                BrokerTrend.Add(queues.BrokerDepth);
                while (BrokerTrend.Count > 10)
                {
                    BrokerTrend.RemoveAt(0);
                }
            }

            long[] trend;
            lock (TrendLock)
            {
                trend = BrokerTrend.ToArray();
            }

            var executar = GetInt(configs, "Executar", 0);
            var intervalo = GetInt(configs, "Intervalo", 60);
            var pacoteCompleto = GetInt(configs, "PacoteCompleto", 0);
            var reBuscar = GetInt(configs, "ReBuscar", 0);
            var threadsCfg = GetInt(configs, "Threads", 5);

            var scm = _scm.GetStatus(_options.WindowsServiceName);
            var serviceView = new ServiceStatusView(
                _options.WindowsServiceName,
                scm.Status,
                scm.Status.Equals("Running", StringComparison.OrdinalIgnoreCase),
                executar,
                serviceRow?.NomServidor,
                serviceRow?.DtcExecucao,
                serviceRow?.DesServico);

            var queueStats = new QueueStats(
                queues.TempBacklog,
                queues.BrokerDepth,
                queues.OldestTempAt,
                trend,
                queues.AnalisadorDepth,
                queues.IntegradorDepth,
                queues.CargaDepth);

            var threads = BuildThreads(configs, serviceRow?.NumSequencialUnico, logs);
            var idleAlerts = threads
                .Where(t => t.IsIdle && t.ThreadId is >= 2 and <= 4)
                .Select(t => new MonitorAlert(
                    "THREAD_IDLE",
                    AlertSeverity.Info,
                    $"Thread {t.ThreadId} ({t.Role}) ociosa — NSU \"0\" (RN-003).",
                    now))
                .ToList();

            var alerts = AlertEngine.Evaluate(
                serviceView,
                queueStats,
                docs,
                logs,
                intervalo,
                _thresholds,
                ConnectionHealth.Healthy,
                null,
                now).ToList();
            alerts.AddRange(idleAlerts);
            alerts = alerts
                .GroupBy(a => a.Code + (a.Code == "THREAD_IDLE" ? a.Message : string.Empty))
                .Select(g => g.First())
                .OrderByDescending(a => a.Severity)
                .ToList();

            var global = new GlobalStatus(
                serviceView,
                intervalo,
                pacoteCompleto,
                reBuscar,
                threadsCfg,
                serviceRow?.NumSequencialUnico,
                now);

            var configItems = configs
                .OrderBy(kv => kv.Key)
                .Select(kv => new ConfigItem(kv.Key, kv.Value))
                .ToList();

            var nsuAdvanced = !string.IsNullOrEmpty(serviceRow?.NumSequencialUnico)
                && LastMainNsu is not null
                && !string.Equals(LastMainNsu, serviceRow.NumSequencialUnico, StringComparison.Ordinal);
            if (!string.IsNullOrEmpty(serviceRow?.NumSequencialUnico))
            {
                LastMainNsu = serviceRow.NumSequencialUnico;
            }

            var tableHealth = TableHealthBuilder.Build(
                now,
                executar,
                intervalo,
                sessionStart,
                serviceRow,
                nsuAdvanced,
                serviceResult.QueryMs,
                configMs,
                queues,
                docs,
                logs,
                logsResult.QueryMs,
                trend,
                _thresholds);

            return new MonitorSnapshot(
                global,
                threads,
                queueStats,
                docs,
                alerts,
                configItems,
                ConnectionHealth.Healthy,
                null,
                live,
                sessionStart?.ToUniversalTime(),
                tableHealth);
        }
        catch (Exception ex)
        {
            var scm = _scm.GetStatus(_options.WindowsServiceName);
            var emptyService = new ServiceStatusView(
                _options.WindowsServiceName,
                scm.Status,
                false,
                0,
                null,
                null,
                null);

            return new MonitorSnapshot(
                new GlobalStatus(emptyService, 0, 0, 0, 0, null, now),
                Array.Empty<ThreadView>(),
                new QueueStats(0, 0, null, Array.Empty<long>()),
                Array.Empty<RecentDocument>(),
                AlertEngine.Evaluate(
                    emptyService,
                    new QueueStats(0, 0, null, Array.Empty<long>()),
                    Array.Empty<RecentDocument>(),
                    Array.Empty<LogEntry>(),
                    60,
                    _thresholds,
                    ConnectionHealth.Down,
                    ex.Message,
                    now),
                Array.Empty<ConfigItem>(),
                ConnectionHealth.Down,
                ex.Message,
                live,
                null,
                Array.Empty<TableHealthView>());
        }
    }

    private async Task<(IReadOnlyDictionary<string, string> Items, DateTimeOffset? SessionStart, long QueryMs)> GetConfigsCachedAsync(
        CancellationToken ct)
    {
        if ((DateTimeOffset.UtcNow - _configCacheAt).TotalSeconds < _options.ConfigCacheSeconds
            && _configCache.Count > 0)
        {
            return (_configCache, _sessionStartCache, _configQueryMs);
        }

        var result = await _read.GetConfigsAsync(_options.CodServicoAnalisador, ct);
        _configCache = result.Items;
        _sessionStartCache = result.ExecutarUpdatedAt;
        _configQueryMs = result.QueryMs;
        _configCacheAt = DateTimeOffset.UtcNow;
        return (_configCache, _sessionStartCache, _configQueryMs);
    }

    private static int GetInt(IReadOnlyDictionary<string, string> configs, string key, int fallback)
        => configs.TryGetValue(key, out var v) && int.TryParse(v, out var n) ? n : fallback;

    private static IReadOnlyList<ThreadView> BuildThreads(
        IReadOnlyDictionary<string, string> configs,
        string? mainNsu,
        IReadOnlyList<LogEntry> logs)
    {
        var configured = GetInt(configs, "Threads", 1);
        if (configured < 1)
        {
            configured = 1;
        }

        LogEntry? Latest(int id) => logs
            .Where(l => l.ThreadId == id)
            .OrderByDescending(l => l.SeqLog)
            .FirstOrDefault();

        var list = new List<ThreadView>(configured);
        for (var id = 1; id <= configured; id++)
        {
            var last = Latest(id);
            var isIdle = last is null
                || string.Equals(last.SeverityHint, "info", StringComparison.OrdinalIgnoreCase)
                   && (last.Mensagem?.Contains("não iniciado", StringComparison.OrdinalIgnoreCase) ?? false);
            list.Add(new ThreadView(
                id,
                id == 1 ? "Principal (pool)" : $"Worker {id}",
                "pool",
                0,
                id == 1 ? mainNsu : null,
                isIdle && id > 1 && last is null,
                false,
                last?.Mensagem ?? (id == 1 ? "Consome fila do Analisador" : "Worker do pool"),
                last?.DtcLog,
                last?.CStat,
                last?.SeverityHint));
        }

        return list;
    }
}

public static class TableHealthBuilder
{
    public static IReadOnlyList<TableHealthView> Build(
        DateTimeOffset nowUtc,
        int executar,
        int intervaloSeconds,
        DateTimeOffset? sessionStart,
        ServiceRow? service,
        bool nsuAdvanced,
        long servicoQueryMs,
        long configQueryMs,
        QueueReadResult queues,
        IReadOnlyList<RecentDocument> docs,
        IReadOnlyList<LogEntry> logs,
        long logQueryMs,
        IReadOnlyList<long> brokerTrend,
        AlertThresholdOptions thresholds)
    {
        var staleSeconds = Math.Max(
            thresholds.StaleMinutesFloor * 60,
            thresholds.StaleIntervalMultiplier * Math.Max(intervaloSeconds, 1));

        double? Age(DateTimeOffset? at) =>
            at is { } t ? Math.Max(0, (nowUtc - t.ToUniversalTime()).TotalSeconds) : null;

        // --- Serviço ---
        var heartbeatAge = Age(service?.DtcExecucao);
        var servicoStatus = "Ok";
        var servicoHint = nsuAdvanced
            ? "Heartbeat ativo neste ciclo."
            : "Heartbeat estável neste ciclo.";
        if (executar == 0)
        {
            servicoStatus = "Atencao";
            servicoHint = "Síntese desligada — serviço ocioso.";
        }
        else if (heartbeatAge.HasValue && heartbeatAge.Value > staleSeconds)
        {
            servicoStatus = "Critico";
            servicoHint = "Última batida no banco antiga com análise ligada.";
        }

        var servico = new TableHealthView(
            "servico",
            "Serviço",
            servicoStatus,
            $"Serviço analisador" + (nsuAdvanced ? " · ativo" : string.Empty),
            heartbeatAge,
            servicoQueryMs,
            servicoHint,
            "/tabelas/servico");

        // --- Configuração ---
        var configStatus = executar == 1 ? "Ok" : "Atencao";
        var configPrimary = executar == 1 ? "Síntese ligada" : "Síntese desligada";
        var config = new TableHealthView(
            "config",
            "Configuração",
            configStatus,
            configPrimary,
            Age(sessionStart),
            configQueryMs,
            executar == 1
                ? "Interruptor Executar = 1 (sessão atual)."
                : "Interruptor Executar = 0 — processo ocioso.",
            "/tabelas/config");

        // --- Temporária ---
        var errCount = docs.Count(d => d.HasError);
        var tmpAge = queues.TempBacklog > 0 ? Age(queues.OldestTempAt) : Age(docs.FirstOrDefault()?.DtcAtualizacao);
        var tmpStatus = "Ok";
        var tmpHint = queues.TempBacklog == 0
            ? "Nenhum lote parado na temporária."
            : "Lotes aguardando análise.";
        if (errCount > 0)
        {
            tmpStatus = "Atencao";
            tmpHint = $"{errCount} lote(s) com mensagem de erro.";
        }

        if (queues.TempBacklog >= thresholds.FilaAlta)
        {
            tmpStatus = "Critico";
            tmpHint = "Backlog alto na temporária.";
        }

        var tmp = new TableHealthView(
            "tmp",
            "Temporária",
            tmpStatus,
            queues.TempBacklog == 0
                ? "Na temporária: 0"
                : $"Na temporária: {queues.TempBacklog}" + (errCount > 0 ? $" · {errCount} erro(s)" : string.Empty),
            tmpAge,
            queues.TempQueryMs,
            tmpHint,
            "/tabelas/tmp");

        // --- Log ---
        var lastLog = logs.OrderByDescending(l => l.SeqLog).FirstOrDefault();
        var logAge = Age(lastLog?.DtcLog);
        var logStatus = "Ok";
        var logHint = lastLog is null ? "Ainda sem eventos no banco." : "Eventos chegando.";
        if (executar == 1 && logAge.HasValue && logAge.Value > staleSeconds)
        {
            logStatus = "Critico";
            logHint = "Síntese ligada sem log novo (silêncio).";
        }
        else if (executar == 0)
        {
            logStatus = "Atencao";
            logHint = "Síntese desligada — poucos eventos esperados.";
        }

        var log = new TableHealthView(
            "log",
            "Log",
            logStatus,
            lastLog?.DtcLog is { } dl
                ? $"Último evento · {dl.ToLocalTime():HH:mm:ss}"
                : "Sem eventos recentes",
            logAge,
            logQueryMs,
            logHint,
            "/tabelas/log");

        // --- Fila única ---
        var filaStatus = "Ok";
        var filaHint = "Fila de entrada estável.";
        if (queues.BrokerDepth >= thresholds.FilaAlta)
        {
            filaStatus = "Critico";
            filaHint = "Fila Analisador alta.";
        }
        else if (brokerTrend.Count >= thresholds.FilaCrescendoSnapshots
                 && queues.BrokerDepth >= thresholds.FilaCrescendoMinDepth
                 && brokerTrend.TakeLast(thresholds.FilaCrescendoSnapshots).Zip(
                     brokerTrend.TakeLast(thresholds.FilaCrescendoSnapshots).Skip(1),
                     (a, b) => b >= a).All(x => x))
        {
            filaStatus = "Atencao";
            filaHint = "Fila de entrada crescendo nos últimos ciclos.";
        }

        var fila = new TableHealthView(
            "fila",
            "Fila Analisador",
            filaStatus,
            $"Na fila: {queues.BrokerDepth}",
            null,
            queues.BrokerQueryMs,
            filaHint,
            "/tabelas/fila");

        return [servico, config, tmp, log, fila];
    }
}

public sealed class TableDetailService : ITableDetailService
{
    private readonly IMonitorReadRepository _read;
    private readonly ISnapshotAggregator _aggregator;
    private readonly MonitorOptions _options;
    private readonly AlertThresholdOptions _thresholds;

    public TableDetailService(
        IMonitorReadRepository read,
        ISnapshotAggregator aggregator,
        IOptions<MonitorOptions> options,
        IOptions<AlertThresholdOptions> thresholds)
    {
        _read = read;
        _aggregator = aggregator;
        _options = options.Value;
        _thresholds = thresholds.Value;
    }

    public async Task<TableDetailDto?> GetAsync(string key, int take, CancellationToken ct)
    {
        key = key.Trim().ToLowerInvariant();
        take = Math.Clamp(take <= 0 ? _options.TableDetailTake : take, 1, 1000);

        var snapshot = await _aggregator.BuildSnapshotAsync(ct);
        var health = snapshot.TableHealth?.FirstOrDefault(h => h.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
        if (health is null)
        {
            return null;
        }

        var sessionStart = snapshot.SessionStartUtc;
        var receptionOn = snapshot.Global.Service.Executar == 1;
        var banner = receptionOn
            ? null
            : "Síntese desligada — mostrando dados da última sessão (desde a última alteração de Executar).";

        TableDetailDto? detail = key switch
        {
            "servico" => await BuildServicoAsync(health, sessionStart, receptionOn, banner, take, ct),
            "config" or "configuracao" => await BuildConfigAsync(health, sessionStart, receptionOn, banner, ct),
            "tmp" or "temporaria" => await BuildTempAsync(health, sessionStart, receptionOn, banner, take, ct),
            "log" => await BuildLogAsync(health, sessionStart, receptionOn, banner, take, ct),
            "fila" or "fila_entrada" => BuildFila(
                health, sessionStart, receptionOn, banner, snapshot.Queues.ServiceBrokerDepth, snapshot.Queues.BrokerDepthTrend),
            _ => null
        };
        return detail is null ? null : WithTakeMeta(detail, take);
    }

    private async Task<TableDetailDto> BuildServicoAsync(
        TableHealthView health,
        DateTimeOffset? sessionStart,
        bool receptionOn,
        string? banner,
        int take,
        CancellationToken ct)
    {
        var service = await _read.GetServiceAsync(_options.CodServicoAnalisador, ct);
        var row = service.Row;
        var serviceRows = row is null
            ? Array.Empty<ServiceDetailRow>()
            : new[]
            {
                new ServiceDetailRow(
                    row.DesServico,
                    row.NomServidor,
                    row.NumSequencialUnico,
                    row.DtcExecucao,
                    row.DtcAtualizacao)
            };

        var logs = await _read.GetLogsSinceAsync(sessionStart, take, ct);
        var context = logs.Items
            .Where(l =>
                !string.IsNullOrEmpty(l.CStat)
                || (l.Mensagem?.Contains("NSU", StringComparison.OrdinalIgnoreCase) ?? false)
                || (l.Mensagem?.Contains("nsu", StringComparison.OrdinalIgnoreCase) ?? false))
            .OrderByDescending(l => l.SeqLog)
            .Take(take)
            .ToList();

        return new TableDetailDto(
            "servico",
            health.Label,
            sessionStart,
            receptionOn,
            banner,
            health with { QueryMs = service.QueryMs },
            serviceRows,
            null,
            null,
            null,
            null,
            context);
    }

    private async Task<TableDetailDto> BuildConfigAsync(
        TableHealthView health,
        DateTimeOffset? sessionStart,
        bool receptionOn,
        string? banner,
        CancellationToken ct)
    {
        var cfg = await _read.GetProcessConfigsAsync(_options.CodServicoAnalisador, ct);
        return new TableDetailDto(
            "configuracao",
            health.Label,
            cfg.ExecutarUpdatedAt?.ToUniversalTime() ?? sessionStart,
            receptionOn,
            banner,
            health with { QueryMs = cfg.QueryMs },
            null,
            cfg.Items,
            null,
            null,
            null,
            null);
    }

    private async Task<TableDetailDto> BuildTempAsync(
        TableHealthView health,
        DateTimeOffset? sessionStart,
        bool receptionOn,
        string? banner,
        int take,
        CancellationToken ct)
    {
        var docs = await _read.GetDocumentsSinceAsync(sessionStart, take, ct);
        return new TableDetailDto(
            "temporaria",
            health.Label,
            sessionStart,
            receptionOn,
            banner,
            health with { QueryMs = docs.QueryMs },
            null,
            null,
            docs.Items,
            null,
            null,
            null);
    }

    private async Task<TableDetailDto> BuildLogAsync(
        TableHealthView health,
        DateTimeOffset? sessionStart,
        bool receptionOn,
        string? banner,
        int take,
        CancellationToken ct)
    {
        var logs = await _read.GetLogsSinceAsync(sessionStart, take, ct);
        return new TableDetailDto(
            "log",
            health.Label,
            sessionStart,
            receptionOn,
            banner,
            health with { QueryMs = logs.QueryMs },
            null,
            null,
            null,
            logs.Items.OrderByDescending(l => l.SeqLog).ToList(),
            null,
            null);
    }

    private TableDetailDto BuildFila(
        TableHealthView health,
        DateTimeOffset? sessionStart,
        bool receptionOn,
        string? banner,
        long depth,
        IReadOnlyList<long> trend)
    {
        var hint = trend.Count >= 2 && trend[^1] > trend[0]
            ? "Tendência de alta nos últimos ciclos."
            : trend.Count >= 2 && trend[^1] < trend[0]
                ? "Tendência de queda nos últimos ciclos."
                : "Profundidade estável.";

        return new TableDetailDto(
            health.Key,
            health.Label,
            sessionStart,
            receptionOn,
            banner,
            health,
            null,
            null,
            null,
            null,
            new FilaDetailView(depth, trend, _thresholds.FilaAlta, hint),
            null);
    }
    private static TableDetailDto WithTakeMeta(TableDetailDto dto, int takeApplied) =>
        dto with
        {
            TakeApplied = takeApplied,
            RowCount = dto.TempRows?.Count
                ?? dto.LogRows?.Count
                ?? dto.ConfigRows?.Count
                ?? dto.ServiceRows?.Count
                ?? (dto.Fila is not null ? 1 : dto.ContextLogs?.Count ?? 0)
        };
}

public sealed class ServiceControlService
{
    private readonly IWindowsServiceController _scm;
    private readonly IMonitorWriteRepository _write;
    private readonly MonitorOptions _options;

    public ServiceControlService(
        IWindowsServiceController scm,
        IMonitorWriteRepository write,
        IOptions<MonitorOptions> options)
    {
        _scm = scm;
        _write = write;
        _options = options.Value;
    }

    public ServiceControlResult GetStatus() => _scm.GetStatus(_options.WindowsServiceName);

    public async Task<ServiceControlResult> StartAsync(CancellationToken ct)
    {
        var commandId = Guid.NewGuid().ToString("N");
        var status = _scm.GetStatus(_options.WindowsServiceName);
        if (status.Status.Equals("Running", StringComparison.OrdinalIgnoreCase))
        {
            await _write.SetExecutarAsync(_options.CodServicoAnalisador, 1, ct);
            return new ServiceControlResult(
                true,
                "Running",
                "Já em execução (idempotente).",
                commandId);
        }

        if (!status.Status.Equals("StartPending", StringComparison.OrdinalIgnoreCase))
        {
            var start = _scm.Start(_options.WindowsServiceName);
            if (!start.Success)
            {
                return start with { CommandId = commandId };
            }
        }

        await _write.SetExecutarAsync(_options.CodServicoAnalisador, 1, ct);
        var after = _scm.GetStatus(_options.WindowsServiceName);
        var running = after.Status.Equals("Running", StringComparison.OrdinalIgnoreCase);
        return new ServiceControlResult(
            true,
            running ? "Running" : "Starting",
            string.IsNullOrWhiteSpace(after.Message)
                ? "Analisador CT-e: start aceito."
                : after.Message,
            commandId);
    }

    public async Task<ServiceControlResult> StopAsync(CancellationToken ct)
    {
        var commandId = Guid.NewGuid().ToString("N");
        await _write.SetExecutarAsync(_options.CodServicoAnalisador, 0, ct);
        var stop = _scm.Stop(_options.WindowsServiceName);
        if (stop.Success && stop.Status.Equals("Stopped", StringComparison.OrdinalIgnoreCase))
        {
            return stop with { CommandId = commandId, Message = stop.Message ?? "Já parado (idempotente)." };
        }

        return stop with { CommandId = commandId, Status = string.IsNullOrWhiteSpace(stop.Status) ? "Stopping" : stop.Status };
    }
}
