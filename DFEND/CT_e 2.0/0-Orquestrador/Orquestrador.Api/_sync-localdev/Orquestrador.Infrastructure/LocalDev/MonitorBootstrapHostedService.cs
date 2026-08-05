using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orquestrador.Application.Abstractions;
using Orquestrador.Application.Options;

namespace Orquestrador.Infrastructure.LocalDev;

/// <summary>
/// No boot (Development), sobe Monitor.Api habilitados que ainda não respondem /health/ready.
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

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task BootstrapAsync(CancellationToken ct)
    {
        try
        {
            var enabled = _options.Value.Systems
                .Where(s => s.Enabled && !string.IsNullOrWhiteSpace(s.ProjectPath))
                .ToList();

            if (enabled.Count == 0)
            {
                return;
            }

            _logger.LogInformation(
                "LocalDev: auto-start de {Count} Monitor.Api…",
                enabled.Count);

            foreach (var system in enabled)
            {
                ct.ThrowIfCancellationRequested();
                var ok = await _launcher.EnsureApiReadyAsync(system, ct);
                if (ok)
                {
                    _logger.LogInformation("Monitor.Api {Id} ready em {BaseUrl}", system.Id, system.BaseUrl);
                }
                else
                {
                    _logger.LogWarning(
                        "Monitor.Api {Id} não ficou ready ({BaseUrl}). Ligar cadeia pode falhar até SQL/API subirem.",
                        system.Id,
                        system.BaseUrl);
                }
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha no auto-start dos Monitor.Api");
        }
    }
}
