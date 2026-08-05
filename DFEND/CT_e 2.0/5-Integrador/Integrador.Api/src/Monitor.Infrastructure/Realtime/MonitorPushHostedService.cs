using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monitor.Application.Abstractions;
using Monitor.Application.Services;
using Monitor.Domain.Models;

namespace Monitor.Infrastructure.Realtime;

public sealed class MonitorHub : Hub
{
}

public sealed class MonitorPushHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<MonitorHub> _hub;
    private readonly MonitorOptions _options;
    private readonly ILogger<MonitorPushHostedService> _logger;
    private long _lastLogSeq;

    public MonitorPushHostedService(
        IServiceScopeFactory scopeFactory,
        IHubContext<MonitorHub> hub,
        IOptions<MonitorOptions> options,
        ILogger<MonitorPushHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _hub = hub;
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
                if (now >= nextSnapshot)
                {
                    await using var scope = _scopeFactory.CreateAsyncScope();
                    var aggregator = scope.ServiceProvider.GetRequiredService<ISnapshotAggregator>();
                    var snapshot = await aggregator.BuildSnapshotAsync(stoppingToken);
                    await _hub.Clients.All.SendAsync("snapshot", snapshot, stoppingToken);
                    nextSnapshot = DateTime.UtcNow + snapshotDelay;
                }

                if (now >= nextLogs)
                {
                    await using var scope = _scopeFactory.CreateAsyncScope();
                    var repo = scope.ServiceProvider.GetRequiredService<IMonitorReadRepository>();
                    var logsResult = await repo.GetLogsAfterAsync(_lastLogSeq, _options.RecentLogsTake, stoppingToken);
                    var logs = logsResult.Items;
                    if (logs.Count > 0)
                    {
                        _lastLogSeq = logs.Max(l => l.SeqLog);
                        await _hub.Clients.All.SendAsync("logsAppend", logs, stoppingToken);
                    }

                    nextLogs = DateTime.UtcNow + logsDelay;
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha no push SignalR do monitor");
                nextSnapshot = DateTime.UtcNow + snapshotDelay;
                nextLogs = DateTime.UtcNow + logsDelay;
            }

            await Task.Delay(200, stoppingToken);
        }
    }
}
