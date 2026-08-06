using CTe.Modules.Monitors.Infrastructure;
using Microsoft.AspNetCore.SignalR;

namespace Orquestrador.Api.Realtime;

/// <summary>
/// Hub unificado dos monitores (paridade CT_e 2.0 <c>/hubs/monitor</c>).
/// Cliente chama <see cref="JoinService"/> para receber <c>snapshot</c> / <c>logsAppend</c> do serviço.
/// </summary>
public sealed class MonitorHub : Hub
{
    private const string ServiceItemKey = "monitorServiceId";
    private readonly MonitorHubSubscriptions _subscriptions;

    public MonitorHub(MonitorHubSubscriptions subscriptions)
    {
        _subscriptions = subscriptions;
    }

    public async Task JoinService(string servico)
    {
        if (!IsKnown(servico))
        {
            return;
        }

        var next = Normalize(servico);
        await LeaveCurrentAsync();
        await Groups.AddToGroupAsync(Context.ConnectionId, next);
        Context.Items[ServiceItemKey] = next;
        _subscriptions.Join(next);
    }

    public Task LeaveService() => LeaveCurrentAsync();

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await LeaveCurrentAsync();
        await base.OnDisconnectedAsync(exception);
    }

    private async Task LeaveCurrentAsync()
    {
        if (Context.Items.TryGetValue(ServiceItemKey, out var raw) && raw is string current)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, current);
            _subscriptions.Leave(current);
            Context.Items.Remove(ServiceItemKey);
        }
    }

    private static bool IsKnown(string? servico) =>
        !string.IsNullOrWhiteSpace(servico)
        && DependencyInjection.KnownMonitorServiceIds.Any(id =>
            id.Equals(servico.Trim(), StringComparison.OrdinalIgnoreCase));

    private static string Normalize(string servico) => servico.Trim().ToLowerInvariant();
}
