using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Orquestrador.Application.Options;

public sealed class OrchestratorOptionsValidator : IValidateOptions<OrchestratorOptions>
{
    private readonly IConfiguration _configuration;

    public OrchestratorOptionsValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ValidateOptionsResult Validate(string? name, OrchestratorOptions options)
    {
        var errors = new List<string>();

        if (options.Systems.Count == 0)
        {
            errors.Add("Orchestrator:Systems deve conter ao menos um sistema no registry.");
        }

        var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < options.Systems.Count; i++)
        {
            var system = options.Systems[i];
            var prefix = $"Orchestrator:Systems[{i}]";

            if (string.IsNullOrWhiteSpace(system.Id))
            {
                errors.Add($"{prefix}:Id é obrigatório.");
            }
            else if (!ids.Add(system.Id.Trim()))
            {
                errors.Add($"{prefix}:Id '{system.Id}' está duplicado.");
            }

            if (!system.Enabled)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(system.BaseUrl))
            {
                errors.Add($"{prefix}:BaseUrl é obrigatória quando Enabled=true (Id={system.Id}).");
                continue;
            }

            if (!Uri.TryCreate(system.BaseUrl.Trim(), UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                errors.Add($"{prefix}:BaseUrl deve ser URL absoluta http/https (Id={system.Id}, valor='{system.BaseUrl}').");
            }
        }

        if (options.HttpTimeoutSeconds is < 1 or > 120)
        {
            errors.Add("Orchestrator:HttpTimeoutSeconds deve estar entre 1 e 120.");
        }

        if (options.CircuitBreakerFailureRatio is < 0 or > 1)
        {
            errors.Add("Orchestrator:CircuitBreakerFailureRatio deve estar entre 0 e 1.");
        }

        // Mesmo contrato em Development, Homologacao e Production: key obrigatória.
        if (string.IsNullOrWhiteSpace(options.InternalApiKey))
        {
            var env = ResolveEnvironmentName();
            errors.Add(
                $"Orchestrator:InternalApiKey é obrigatória em {env} " +
                "(Development, Homologacao e Production). " +
                "Defina via appsettings do ambiente ou env Orchestrator__InternalApiKey.");
        }

        return errors.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(errors);
    }

    private string ResolveEnvironmentName() =>
        _configuration["ASPNETCORE_ENVIRONMENT"]
        ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
        ?? "Production";
}
