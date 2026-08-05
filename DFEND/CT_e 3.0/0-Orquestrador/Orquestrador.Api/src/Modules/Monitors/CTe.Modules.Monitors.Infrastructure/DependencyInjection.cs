using CTe.Modules.Monitors.Abstractions;
using CTe.Modules.Monitors.WindowsControl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orquestrador.Application.Abstractions;
using Orquestrador.Application.Options;

namespace CTe.Modules.Monitors.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Ids oficiais dos monitores unificados (SDD).
    /// W3: registro padrão = <see cref="InProcessMonitorModule"/> (zero dependência funcional de Monitor.Api).
    /// </summary>
    public static readonly IReadOnlyList<string> KnownMonitorServiceIds =
    [
        "receptor",
        "arquivador",
        "sintetizador",
        "analisador",
        "integrador",
        "carga"
    ];

    public static IServiceCollection AddUnifiedMonitorModules(this IServiceCollection services)
    {
        foreach (var serviceId in KnownMonitorServiceIds)
        {
            var id = serviceId;
            services.AddSingleton<IMonitorModule>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var hostEnv = sp.GetRequiredService<IHostEnvironment>();
                var options = BindMonitorOptions(configuration, id);

                IMonitorModule? httpFallback = null;
                if (options.UseHttpFallback)
                {
                    // Usa MonitorHttpClient concreto (não IMonitorClient) para evitar ciclo com ModuleBackedMonitorClient.
                    httpFallback = new HttpMonitorModule(
                        id,
                        sp.GetRequiredService<Orquestrador.Infrastructure.Http.MonitorHttpClient>(),
                        sp.GetRequiredService<IHttpClientFactory>(),
                        sp.GetRequiredService<IOptions<OrchestratorOptions>>(),
                        sp.GetRequiredService<ILogger<HttpMonitorModule>>());
                }

                return new InProcessMonitorModule(
                    options,
                    httpFallback,
                    sp.GetRequiredService<ILogger<InProcessMonitorModule>>(),
                    hostEnv.ContentRootPath,
                    AppContext.BaseDirectory);
            });
        }

        services.AddSingleton<IMonitorModuleRegistry, MonitorModuleRegistry>();
        // Substitui MonitorHttpClient na cascata: start/stop/status/snapshot via módulos in-process.
        services.AddSingleton<IMonitorClient, ModuleBackedMonitorClient>();
        return services;
    }

    private static MonitorControlOptions BindMonitorOptions(IConfiguration configuration, string serviceId)
    {
        var options = new MonitorControlOptions { ServiceId = serviceId };
        configuration.GetSection($"Monitors:{serviceId}").Bind(options);
        if (string.IsNullOrWhiteSpace(options.ServiceId))
        {
            options.ServiceId = serviceId;
        }

        if (string.IsNullOrWhiteSpace(options.Domain))
        {
            options.Domain = serviceId;
        }

        return options;
    }
}
