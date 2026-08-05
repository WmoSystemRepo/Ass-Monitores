using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Monitor.Application.Abstractions;
using Monitor.Application.Services;
using Monitor.Domain.Alerts;
using Monitor.Infrastructure.Live;
using Monitor.Infrastructure.Realtime;
using Monitor.Infrastructure.Sql;
using Monitor.Infrastructure.Windows;

namespace Monitor.Infrastructure;

public static class DependencyInjection
{
    [SupportedOSPlatform("windows")]
    public static IServiceCollection AddMonitorInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IWindowsServiceController, WindowsServiceControllerAdapter>();
        services.AddSingleton<ILiveTraceReader, LiveTraceReader>();
        services.AddScoped<SqlMonitorRepository>();
        services.AddScoped<IMonitorReadRepository>(sp => sp.GetRequiredService<SqlMonitorRepository>());
        services.AddScoped<IMonitorWriteRepository>(sp => sp.GetRequiredService<SqlMonitorRepository>());
        services.AddScoped<ISnapshotAggregator, SnapshotAggregator>();
        services.AddScoped<ITableDetailService, TableDetailService>();
        services.AddScoped<ServiceControlService>();
        services.AddHostedService<MonitorPushHostedService>();
        return services;
    }
}
