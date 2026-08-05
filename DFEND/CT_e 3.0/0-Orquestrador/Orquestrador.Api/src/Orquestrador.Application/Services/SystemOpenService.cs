using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orquestrador.Application.Abstractions;
using Orquestrador.Application.Options;

namespace Orquestrador.Application.Services;

/// <summary>
/// Antes de abrir o frontend de um sistema: testa a API do monitor;
/// se estiver offline (DEV), sobe o Monitor.Api; depois libera a URL do front.
/// </summary>
public sealed class SystemOpenService
{
    private readonly IMonitorProcessLauncher _launcher;
    private readonly OrchestratorOptions _options;
    private readonly ILogger<SystemOpenService> _logger;

    public SystemOpenService(
        IMonitorProcessLauncher launcher,
        IOptions<OrchestratorOptions> options,
        ILogger<SystemOpenService> logger)
    {
        _launcher = launcher;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<SystemOpenResult> EnsureAndOpenAsync(string systemId, CancellationToken ct)
    {
        var cfg = _options.Systems.FirstOrDefault(s =>
            s.Id.Equals(systemId, StringComparison.OrdinalIgnoreCase));

        if (cfg is null)
        {
            return Fail(systemId, "Sistema não encontrado no registry.");
        }

        var frontendUrl = cfg.ResolveFrontendUrl();
        var apiBase = cfg.ResolveApiBase();

        if (string.IsNullOrWhiteSpace(frontendUrl))
        {
            return Fail(systemId, $"{cfg.DisplayName} não tem FrontendUrl configurada.");
        }

        if (!cfg.Enabled)
        {
            return Fail(systemId, $"{cfg.DisplayName} está desabilitado no registry.", frontendUrl);
        }

        if (string.IsNullOrWhiteSpace(apiBase))
        {
            return Fail(systemId, $"{cfg.DisplayName} sem BaseUrl da API.", frontendUrl);
        }

        cfg.FrontendUrl ??= frontendUrl;
        cfg.BaseUrl = string.IsNullOrWhiteSpace(cfg.BaseUrl) ? apiBase : cfg.BaseUrl;

        var wasReady = await _launcher.IsApiReadyAsync(apiBase, ct);
        var apiStarted = false;
        var apiReady = wasReady;

        if (!wasReady)
        {
            _logger.LogInformation(
                "Abrir {Id}: API offline em {BaseUrl} — tentando ensure/start…",
                cfg.Id,
                apiBase);

            apiReady = await _launcher.EnsureApiReadyAsync(cfg, ct);
            apiStarted = apiReady && !wasReady;

            if (!apiReady)
            {
                return new SystemOpenResult(
                    Success: false,
                    cfg.Id,
                    frontendUrl.Trim(),
                    ApiReady: false,
                    ApiStarted: false,
                    FrontendReachable: false,
                    FrontendStarted: false,
                    Message:
                        $"API do {cfg.DisplayName} indisponível em {apiBase}. " +
                        "Suba o Monitor.Api (ou confira SQL / ProjectPath) antes de abrir o front.");
            }
        }

        var frontWasUp = await _launcher.IsFrontendReachableAsync(frontendUrl, ct);
        var frontStarted = false;
        var frontReachable = frontWasUp;

        if (!frontWasUp)
        {
            _logger.LogInformation(
                "Abrir {Id}: frontend offline em {Url} — tentando ensure/start…",
                cfg.Id,
                frontendUrl);

            frontReachable = await _launcher.EnsureFrontendAsync(cfg, ct);
            frontStarted = frontReachable && !frontWasUp;
        }

        if (!frontReachable)
        {
            return new SystemOpenResult(
                Success: false,
                cfg.Id,
                frontendUrl.Trim(),
                ApiReady: true,
                ApiStarted: apiStarted,
                FrontendReachable: false,
                FrontendStarted: frontStarted,
                Message:
                    $"API do {cfg.DisplayName} pronta" +
                    (apiStarted ? " (iniciada agora)" : "") +
                    $", mas o front em {frontendUrl} ainda não está no ar. " +
                    "Aguarde o Angular (npm start) ou use docker compose — não abrimos URL morta.");
        }

        return new SystemOpenResult(
            Success: true,
            cfg.Id,
            frontendUrl.Trim(),
            ApiReady: true,
            ApiStarted: apiStarted,
            FrontendReachable: true,
            FrontendStarted: frontStarted,
            Message: BuildOkMessage(cfg.DisplayName, apiStarted, frontStarted));
    }

    private static string BuildOkMessage(string displayName, bool apiStarted, bool frontStarted)
    {
        if (apiStarted && frontStarted)
        {
            return $"{displayName}: API e front iniciados — abrindo.";
        }

        if (apiStarted)
        {
            return $"{displayName}: API iniciada — abrindo front.";
        }

        if (frontStarted)
        {
            return $"{displayName}: front iniciado — abrindo.";
        }

        return $"{displayName}: API e front ok — abrindo.";
    }

    private static SystemOpenResult Fail(string systemId, string message, string? frontendUrl = null) =>
        new(false, systemId, frontendUrl, false, false, false, false, message);
}
