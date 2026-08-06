using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orquestrador.Application.Abstractions;
using Orquestrador.Application.Options;
using Orquestrador.Domain.Models;

namespace Orquestrador.Application.Services;

public sealed class CascadeControlService
{
    private readonly IMonitorClient _client;
    private readonly IMonitorProcessLauncher _launcher;
    private readonly OrchestratorOptions _options;
    private readonly ILogger<CascadeControlService> _logger;
    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly object _stateLock = new();

    private CascadePhase _phase = CascadePhase.Idle;
    private string? _message;

    public CascadeControlService(
        IMonitorClient client,
        IMonitorProcessLauncher launcher,
        IOptions<OrchestratorOptions> options,
        ILogger<CascadeControlService> logger)
    {
        _client = client;
        _launcher = launcher;
        _options = options.Value;
        _logger = logger;
    }

    public (CascadePhase Phase, string? Message) GetStatus()
    {
        lock (_stateLock)
        {
            return (_phase, _message);
        }
    }

    /// <summary>
    /// Só stack (API + Angular) de todos os Enabled — sem ligar workers.
    /// Usado quando o front do Orquestrador sobe, para já deixar :4200/:4210/… no ar.
    /// </summary>
    public async Task<(bool Success, string Message, int ReadyCount, int TotalCount)> EnsureAllStacksAsync(
        CancellationToken ct)
    {
        var enabled = GetEnabledOrdered();
        if (enabled.Count == 0)
        {
            return (false, "Nenhum sistema habilitado no registry.", 0, 0);
        }

        var perSystemSec = Math.Max(60, _options.LocalDev.FrontendEnsureTimeoutSeconds);
        var overallSec = perSystemSec * Math.Max(1, enabled.Count) + 30;
        using var workCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        workCts.CancelAfter(TimeSpan.FromSeconds(overallSec));

        try
        {
            var prepResults = await EnsureAllEnabledStacksOnlineAsync(enabled, workCts.Token);
            var ready = prepResults.Count(r => r.Ready);
            var blocked = prepResults
                .Where(r => !r.Ready)
                .Select(r => $"{r.System.DisplayName}: {r.BlockReason}")
                .ToList();

            var msg = ready == enabled.Count
                ? $"API + Angular online em {ready}/{enabled.Count} monitor(es)."
                : $"API + Angular: {ready}/{enabled.Count} online." +
                  (blocked.Count > 0 ? " Pendentes: " + string.Join("; ", blocked) : string.Empty);

            return (ready > 0, msg, ready, enabled.Count);
        }
        catch (OperationCanceledException)
        {
            return (false, "Tempo esgotado ao subir API/Angular dos monitores.", 0, enabled.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha em EnsureAllStacksAsync");
            return (false, $"Erro ao subir stacks: {ex.Message}", 0, enabled.Count);
        }
    }

    public async Task<(bool Success, string? Message)> StartAsync(CancellationToken ct)
    {
        if (!await _gate.WaitAsync(0, ct))
        {
            return (false, "Cascata já em andamento.");
        }

        try
        {
            SetPhase(CascadePhase.Starting, "Ligando as filas…");

            var enabled = GetEnabledOrdered();
            if (enabled.Count == 0)
            {
                SetPhase(CascadePhase.Idle, "Nenhum sistema habilitado no registry.");
                return (false, _message);
            }

            var perSystemSec = Math.Max(
                _options.PollUntilSettledSeconds,
                Math.Max(60, _options.LocalDev.StartupTimeoutSeconds));
            var overallSec = perSystemSec * Math.Max(1, enabled.Count) * 2 + 60;
            using var workCts = new CancellationTokenSource(TimeSpan.FromSeconds(overallSec));
            var workCt = workCts.Token;

            // 0) DevHosts (.exe net47) — Sintetizador/Analisador etc. sem bin pré-compilado.
            SetPhase(CascadePhase.Starting, "Compilando Hosts POC (DevHost) se faltarem…");
            var (devOk, devMsg) = await _launcher.EnsureDevHostsBuiltAsync(workCt);
            if (!devOk)
            {
                _logger.LogWarning("DevHosts pré-build: {Msg}", devMsg);
                // Não aborta a cascata inteira — monitores que já têm .exe seguem;
                // os que falharam vão reportar no Start do worker.
            }
            else
            {
                _logger.LogInformation("DevHosts pré-build: {Msg}", devMsg);
            }

            // 1) Stack microserviço de TODOS os Enabled (registry N sistemas: R, A, futuros…).
            //    API + Angular em paralelo — independe da ordem DependsOn (só o worker é sequencial).
            SetPhase(
                CascadePhase.Starting,
                $"Subindo API + Angular de {enabled.Count} monitor(es) habilitado(s)…");

            var prepResults = await EnsureAllEnabledStacksOnlineAsync(enabled, workCt);
            var readySystems = prepResults.Where(r => r.Ready).Select(r => r.System).ToList();
            var prepBlocked = prepResults
                .Where(r => !r.Ready)
                .Select(r => $"{r.System.DisplayName} ({r.BlockReason})")
                .ToList();

            if (readySystems.Count == 0)
            {
                SetPhase(
                    CascadePhase.Idle,
                    "Nenhum monitor ficou com API e Angular online. Serviços não foram ligados. " +
                    string.Join(" ", prepBlocked));
                return (false, _message);
            }

            if (prepBlocked.Count > 0)
            {
                _logger.LogWarning(
                    "Prep parcial: {Ready}/{Total} stacks online. Bloqueados: {Blocked}",
                    readySystems.Count,
                    enabled.Count,
                    string.Join("; ", prepBlocked));
            }

            SetPhase(
                CascadePhase.Starting,
                $"API/Angular ok ({readySystems.Count}/{enabled.Count}) — ligando workers na ordem do registry…");

            // 2) Workers em ordem Order/DependsOn — só quem já tem stack online.
            var started = new List<string>();
            var startedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var startFailed = new List<string>();
            var readyIds = readySystems.Select(s => s.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var system in enabled)
            {
                workCt.ThrowIfCancellationRequested();

                if (!readyIds.Contains(system.Id))
                {
                    continue;
                }

                var depBlock = ValidateDependenciesStarted(system, startedIds);
                if (depBlock is not null)
                {
                    prepBlocked.Add($"{system.DisplayName} ({depBlock})");
                    SetPhase(
                        CascadePhase.Starting,
                        $"{depBlock} — cascata interrompida (fail-fast).");
                    break;
                }

                SetPhase(
                    CascadePhase.Starting,
                    $"API e Angular do {system.DisplayName} online — ligando filas (processo + Executar=1)…");

                var result = await _client.StartAsync(system, workCt);
                if (!result.Success)
                {
                    _logger.LogWarning(
                        "Start {MonitorId} falhou: {Msg} CommandId={CommandId}",
                        system.Id,
                        result.Message,
                        result.CommandId);
                    startFailed.Add(system.DisplayName);
                    SetPhase(
                        CascadePhase.Starting,
                        $"Falha ao ligar filas do {system.DisplayName} — cascata interrompida (já ligados permanecem).");
                    break;
                }

                SetPhase(CascadePhase.Starting, $"Aguardando {system.DisplayName} em execução…");
                var settled = await PollUntilSettledAsync(system, wantRunning: true, workCt);
                if (!settled)
                {
                    startFailed.Add($"{system.DisplayName} (timeout/{OfficialMonitorState.Failed})");
                    SetPhase(
                        CascadePhase.Starting,
                        $"{system.DisplayName} não entrou em execução a tempo — cascata interrompida.");
                    break;
                }

                started.Add(system.DisplayName);
                startedIds.Add(system.Id);
                await DelayAsync(workCt);
            }

            var summary = BuildStartSummary(started, startFailed, prepBlocked);
            var ok = started.Count > 0 && startFailed.Count == 0 && prepBlocked.Count == 0;
            if (started.Count > 0 && (startFailed.Count > 0 || prepBlocked.Count > 0))
            {
                // Estado parcial: algum ligado, cascata parou — fase Running parcial.
                SetPhase(CascadePhase.Running, summary);
                return (true, _message);
            }

            SetPhase(ok ? CascadePhase.Running : CascadePhase.Idle, summary);
            return (ok, _message);
        }
        catch (OperationCanceledException)
        {
            SetPhase(CascadePhase.Idle, "Ligar as filas cancelado ou expirou o tempo de espera.");
            return (false, _message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na cascata de start");
            SetPhase(CascadePhase.Idle, $"Erro ao ligar as filas: {ex.Message}");
            return (false, _message);
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task<(bool Success, string? Message)> StopAsync(CancellationToken ct)
    {
        if (!await _gate.WaitAsync(0, ct))
        {
            return (false, "Cascata já em andamento.");
        }

        try
        {
            SetPhase(CascadePhase.Stopping, "Desligando filas…");

            var enabled = GetEnabledOrdered();
            enabled.Reverse();
            if (enabled.Count == 0)
            {
                SetPhase(CascadePhase.Idle, "Nenhum sistema habilitado no registry.");
                return (false, _message);
            }

            foreach (var system in enabled)
            {
                ct.ThrowIfCancellationRequested();
                SetPhase(CascadePhase.Stopping, $"Parando filas do {system.DisplayName}…");

                // Sempre tenta stop (Executar=0 + processo). Não pular por health —
                // Desligar filas precisa funcionar mesmo com SQL/API instável.
                var result = await _client.StopAsync(system, ct);
                if (!result.Success)
                {
                    _logger.LogWarning(
                        "Stop {MonitorId} falhou: {Msg} CommandId={CommandId}",
                        system.Id,
                        result.Message,
                        result.CommandId);
                    SetPhase(CascadePhase.Stopping, $"Falha ao parar {system.DisplayName} — seguindo.");
                }
                else
                {
                    await PollUntilSettledAsync(system, wantRunning: false, ct);
                }

                await DelayAsync(ct);
            }

            SetPhase(CascadePhase.Idle, "Filas desligadas.");
            return (true, _message);
        }
        catch (OperationCanceledException)
        {
            SetPhase(CascadePhase.Idle, "Desligar filas cancelado.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na cascata de stop");
            SetPhase(CascadePhase.Idle, $"Erro ao desligar filas: {ex.Message}");
            return (false, _message);
        }
        finally
        {
            _gate.Release();
        }
    }

    private List<OrchestratorSystemOptions> GetEnabledOrdered()
    {
        return _options.Systems
            .Where(s => s.Enabled && s.InCascade)
            .Select((s, index) => (System: s, Index: index))
            .OrderBy(x => x.System.Order > 0 ? x.System.Order : x.Index + 1)
            .ThenBy(x => x.Index)
            .Select(x => x.System)
            .ToList();
    }

    private static string? ValidateDependenciesStarted(
        OrchestratorSystemOptions system,
        HashSet<string> alreadyReadyInThisCascade)
    {
        if (system.DependsOn is null || system.DependsOn.Count == 0)
        {
            return null;
        }

        foreach (var dep in system.DependsOn)
        {
            if (!alreadyReadyInThisCascade.Contains(dep))
            {
                return $"Dependência '{dep}' ainda não está pronta para {system.Id}";
            }
        }

        return null;
    }

    private async Task<bool> PollUntilSettledAsync(
        OrchestratorSystemOptions system,
        bool wantRunning,
        CancellationToken ct)
    {
        var deadline = DateTime.UtcNow.AddSeconds(Math.Max(5, _options.PollUntilSettledSeconds));
        var interval = TimeSpan.FromMilliseconds(Math.Max(500, _options.PollIntervalMs));

        while (DateTime.UtcNow < deadline)
        {
            ct.ThrowIfCancellationRequested();
            var statusResult = await _client.GetStatusAsync(system, ct);
            if (statusResult.ErrorKind is null && statusResult.Value is not null)
            {
                var running = IsEffectivelyRunning(statusResult.Value);
                if (wantRunning && running)
                {
                    return true;
                }

                if (!wantRunning && !running)
                {
                    return true;
                }
            }

            await Task.Delay(interval, ct);
        }

        return false;
    }

    private static bool IsEffectivelyRunning(MonitorServiceStatusDto dto)
    {
        if (dto.IsRunning == true)
        {
            return true;
        }

        if (dto.Executar == 1)
        {
            return true;
        }

        var status = dto.Status ?? dto.ScmStatus ?? string.Empty;
        return status.Contains("Running", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Garante API + Angular de todos os monitores Enabled do registry em paralelo.
    /// Escala para N sistemas (hoje R+A; amanhã S/An/I/C com Enabled=true + paths).
    /// </summary>
    private async Task<IReadOnlyList<(OrchestratorSystemOptions System, bool Ready, string BlockReason)>> EnsureAllEnabledStacksOnlineAsync(
        IReadOnlyList<OrchestratorSystemOptions> enabled,
        CancellationToken ct)
    {
        var tasks = enabled.Select(async system =>
        {
            ct.ThrowIfCancellationRequested();
            var (ready, blockReason) = await EnsureMonitorStackOnlineAsync(system, ct);
            if (!ready)
            {
                _logger.LogWarning(
                    "Prep stack {MonitorId} falhou: {Reason}",
                    system.Id,
                    blockReason);
            }
            else
            {
                _logger.LogInformation(
                    "Stack {MonitorId} online (API{Front})",
                    system.Id,
                    string.IsNullOrWhiteSpace(system.ResolveFrontendUrl()) ? "" : " + Angular");
            }

            return (system, ready, blockReason);
        });

        return await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Stack microserviço 100%: Monitor.Api + Angular (quando há FrontendUrl).
    /// Sem front ready o Ligar não assume o estágio — evita “ligado” com UI morta.
    /// Rapidez vem do pré-aquecimento no boot (AutoStartMonitors) + Docker (nginx).
    /// </summary>
    private async Task<(bool Ready, string BlockReason)> EnsureMonitorStackOnlineAsync(
        OrchestratorSystemOptions system,
        CancellationToken ct)
    {
        var apiReady = await EnsureMonitorApiOnlineAsync(system, ct);
        if (!apiReady)
        {
            return (false, $"Não foi possível deixar a API do {system.DisplayName} online");
        }

        var frontendUrl = system.ResolveFrontendUrl();
        if (string.IsNullOrWhiteSpace(frontendUrl))
        {
            return (true, string.Empty);
        }

        system.FrontendUrl ??= frontendUrl;

        var frontReady = await EnsureMonitorFrontendOnlineAsync(system, ct);
        if (!frontReady)
        {
            return (
                false,
                $"Angular do {system.DisplayName} ainda offline em {frontendUrl} " +
                "(aguarde o boot / npm start, ou use docker compose para stack rápida)");
        }

        return (true, string.Empty);
    }

    private async Task<bool> EnsureMonitorApiOnlineAsync(
        OrchestratorSystemOptions system,
        CancellationToken ct)
    {
        if (_options.LocalDev.EnsureBeforeCascade)
        {
            return await _launcher.EnsureApiReadyAsync(system, ct);
        }

        return await _client.PingReadyAsync(system, ct);
    }

    private async Task<bool> EnsureMonitorFrontendOnlineAsync(
        OrchestratorSystemOptions system,
        CancellationToken ct)
    {
        if (_options.LocalDev.EnsureBeforeCascade)
        {
            return await _launcher.EnsureFrontendAsync(system, ct);
        }

        return await _launcher.IsFrontendReachableAsync(system.ResolveFrontendUrl(), ct);
    }

    private static string BuildStartSummary(
        IReadOnlyList<string> started,
        IReadOnlyList<string> startFailed,
        IReadOnlyList<string> prepBlocked)
    {
        var parts = new List<string>();

        if (started.Count > 0)
        {
            parts.Add($"Filas ligadas (processo + Executar=1): {string.Join(", ", started)}.");
        }

        if (prepBlocked.Count > 0)
        {
            parts.Add(
                $"API/Angular indisponível — filas não ligadas: {string.Join("; ", prepBlocked)}.");
        }

        if (startFailed.Count > 0)
        {
            parts.Add($"Falha ao ligar filas: {string.Join(", ", startFailed)}.");
        }

        if (parts.Count == 0)
        {
            return "Nenhuma fila foi ligada.";
        }

        return string.Join(" ", parts);
    }

    private void SetPhase(CascadePhase phase, string? message)
    {
        lock (_stateLock)
        {
            _phase = phase;
            _message = message;
        }
    }

    private Task DelayAsync(CancellationToken ct)
    {
        var ms = Math.Max(0, _options.CascadeDelayMs);
        return ms == 0 ? Task.CompletedTask : Task.Delay(ms, ct);
    }
}
