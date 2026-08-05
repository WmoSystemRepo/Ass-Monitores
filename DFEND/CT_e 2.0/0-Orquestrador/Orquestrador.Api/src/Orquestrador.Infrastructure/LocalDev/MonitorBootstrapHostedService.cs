using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orquestrador.Application.Abstractions;
using Orquestrador.Application.Options;

namespace Orquestrador.Infrastructure.LocalDev;

/// <summary>
/// No boot (Development), sobe Monitor.Api + Angular de TODOS os monitores Enabled do registry
/// (hoje Receptor + Arquivador; amanhã qualquer um com Enabled=true + ProjectPath/FrontendProjectPath).
/// Pré-aquece a stack para o Ligar/clique serem rápidos depois do 1º compile do Nx.
/// </summary>
public sealed class MonitorBootstrapHostedService : IHostedService
{
    private readonly IMonitorProcessLauncher _launcher;
    private readonly IOptions<OrchestratorOptions> _options;
    private readonly IHostEnvironment _env;
    private readonly ILogger<MonitorBootstrapHostedService> _logger;

    public MonitorBootstrapHostedService(
        IMonitorProcessLauncher launcher,
        IOptions<OrchestratorOptions> options,
        IHostEnvironment env,
        ILogger<MonitorBootstrapHostedService> logger)
    {
        _launcher = launcher;
        _options = options;
        _env = env;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_env.IsDevelopment() || !_options.Value.LocalDev.AutoStartMonitors)
        {
            return Task.CompletedTask;
        }

        _ = Task.Run(() => BootstrapAsync(cancellationToken), CancellationToken.None);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Libera DLLs dos Monitor.Api spawnados — senão F5/rebuild no Receptor/Arquivador falha com file lock.
        _launcher.KillLaunchedChildren();
        return Task.CompletedTask;
    }

    private async Task BootstrapAsync(CancellationToken ct)
    {
        try
        {
            var enabled = _options.Value.Systems
                .Where(s => s.Enabled)
                .Where(s =>
                    !string.IsNullOrWhiteSpace(s.ProjectPath) ||
                    !string.IsNullOrWhiteSpace(s.FrontendProjectPath))
                .ToList();

            if (enabled.Count == 0)
            {
                return;
            }

            _logger.LogInformation(
                "LocalDev: auto-start paralelo de {Count} stack(s) do registry (API + Angular)…",
                enabled.Count);

            // 0) Compila DevHosts faltantes (S/An costumam não ter .exe no clone fresco).
            try
            {
                var (ok, msg) = await _launcher.EnsureDevHostsBuiltAsync(ct);
                if (ok)
                    _logger.LogInformation("LocalDev bootstrap DevHosts: {Msg}", msg);
                else
                    _logger.LogWarning("LocalDev bootstrap DevHosts: {Msg}", msg);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "LocalDev: falha ao pré-compilar DevHosts (seguindo com API/Angular).");
            }

            // 1) APIs em paralelo (rápido) — não espera Angular do R para subir API do A.
            var readyApis = new System.Collections.Concurrent.ConcurrentBag<OrchestratorSystemOptions>();
            var apiTasks = enabled
                .Where(s => !string.IsNullOrWhiteSpace(s.ProjectPath))
                .Select(async system =>
                {
                    ct.ThrowIfCancellationRequested();
                    var apiOk = await _launcher.EnsureApiReadyAsync(system, ct);
                    if (apiOk)
                    {
                        readyApis.Add(system);
                        _logger.LogInformation(
                            "Monitor.Api {Id} ready em {BaseUrl}",
                            system.Id,
                            system.ResolveApiBase());
                    }
                    else
                    {
                        _logger.LogWarning(
                            "Monitor.Api {Id} não ficou ready ({BaseUrl}). Ligar pode falhar até SQL/API subirem.",
                            system.Id,
                            system.ResolveApiBase());
                    }
                });

            await Task.WhenAll(apiTasks);

            // Abre Swagger UI assim que as APIs estão ready (confirmação visual; não espera Angular).
            if (_options.Value.LocalDev.OpenSwaggerOnReady)
            {
                foreach (var system in readyApis.OrderBy(s => s.Order).ThenBy(s => s.Id))
                {
                    var swaggerUrl = system.ResolveSwaggerUrl();
                    if (string.IsNullOrWhiteSpace(swaggerUrl))
                    {
                        continue;
                    }

                    if (LocalDevBrowser.TryOpen(swaggerUrl, out var openError))
                    {
                        _logger.LogInformation(
                            "Swagger {Id} aberto: {Url}",
                            system.Id,
                            swaggerUrl);
                    }
                    else
                    {
                        _logger.LogWarning(
                            "Não foi possível abrir Swagger {Id} ({Url}): {Error}",
                            system.Id,
                            swaggerUrl,
                            openError);
                    }
                }
            }

            // 2) Angular em paralelo (lento na 1ª vez) — processo npm já sobe; Ligar espera ready.
            var frontTasks = enabled
                .Where(s =>
                    !string.IsNullOrWhiteSpace(s.ResolveFrontendUrl()) &&
                    !string.IsNullOrWhiteSpace(s.FrontendProjectPath))
                .Select(async system =>
                {
                    ct.ThrowIfCancellationRequested();
                    var frontUrl = system.ResolveFrontendUrl();
                    system.FrontendUrl ??= frontUrl;
                    var frontOk = await _launcher.EnsureFrontendAsync(system, ct);
                    if (frontOk)
                    {
                        _logger.LogInformation("Angular {Id} ready em {Url}", system.Id, frontUrl);
                    }
                    else
                    {
                        _logger.LogWarning(
                            "Angular {Id} ainda offline em {Url} — Ligar/clique só liberam quando o front responder. " +
                            "Confira se npm install foi rodado em Frontend e se a porta está livre.",
                            system.Id,
                            frontUrl);
                    }
                });

            await Task.WhenAll(frontTasks);
            _logger.LogInformation("LocalDev: pré-aquecimento da stack concluído.");
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha no auto-start da stack LocalDev");
        }
    }
}
