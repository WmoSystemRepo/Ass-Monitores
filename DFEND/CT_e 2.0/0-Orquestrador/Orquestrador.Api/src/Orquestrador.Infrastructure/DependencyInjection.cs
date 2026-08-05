using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Orquestrador.Application.Abstractions;
using Orquestrador.Application.Options;
using Orquestrador.Application.Services;
using Orquestrador.Infrastructure.Http;
using Orquestrador.Infrastructure.LocalDev;

namespace Orquestrador.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrquestradorInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient(MonitorHttpClient.HttpClientName)
            .AddStandardResilienceHandler()
            .Configure((HttpStandardResilienceOptions options, IServiceProvider sp) =>
            {
                var orch = sp.GetRequiredService<IOptions<OrchestratorOptions>>().Value;
                var timeout = TimeSpan.FromSeconds(Math.Clamp(orch.HttpTimeoutSeconds, 1, 120));

                options.AttemptTimeout.Timeout = timeout;
                options.TotalRequestTimeout.Timeout = timeout + TimeSpan.FromSeconds(3);
                options.Retry.MaxRetryAttempts = 1;
                options.CircuitBreaker.SamplingDuration =
                    TimeSpan.FromSeconds(Math.Max(10, orch.CircuitBreakerSamplingDurationSeconds));
                options.CircuitBreaker.FailureRatio = Math.Clamp(orch.CircuitBreakerFailureRatio, 0, 1);
                options.CircuitBreaker.MinimumThroughput = Math.Max(2, orch.CircuitBreakerMinimumThroughput);
                options.CircuitBreaker.BreakDuration =
                    TimeSpan.FromSeconds(Math.Max(1, orch.CircuitBreakerBreakDurationSeconds));

                // Em LocalDev o boot/ligar gera várias falhas de conexão esperadas; evita circuit aberto
                // impedir o POST /service/start logo após o Monitor.Api subir.
                if (orch.LocalDev.AutoStartMonitors || orch.LocalDev.EnsureBeforeCascade)
                {
                    options.CircuitBreaker.MinimumThroughput = Math.Max(options.CircuitBreaker.MinimumThroughput, 40);
                    options.CircuitBreaker.FailureRatio = Math.Max(options.CircuitBreaker.FailureRatio, 0.9);
                }
            });

        services.AddSingleton<IMonitorClient, MonitorHttpClient>();
        services.AddSingleton<IMonitorProcessLauncher, MonitorProcessLauncher>();
        services.AddHostedService<MonitorBootstrapHostedService>();
        services.AddSingleton<CascadeControlService>();
        services.AddSingleton<SystemOpenService>();
        services.AddSingleton<ChainSnapshotAggregator>();

        return services;
    }
}
