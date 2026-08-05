using CTe.Resgate.Application.Abstractions;
using CTe.Resgate.Application.Services;
using CTe.Resgate.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CTe.Resgate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddResgateInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("BDCTeSintetico");
        if (string.IsNullOrWhiteSpace(cs))
        {
            throw new InvalidOperationException(
                "ConnectionStrings:BDCTeSintetico é obrigatório. Configure SQL Server — fallback InMemory foi removido.");
        }

        // Resgate só enfileira na Carga (temp + Service Broker). Worker próprio desligado.
        services.AddSingleton<ICargaDownloadEnqueue, SqlCargaDownloadEnqueue>();
        services.AddScoped<IResgateService, ResgateService>();
        return services;
    }
}
