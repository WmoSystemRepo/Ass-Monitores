using System.Security.Claims;
using System.Text.Encodings.Web;
using CTe.BuildingBlocks.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Orquestrador.Api.Auth;

/// <summary>
/// Esqueleto de autenticação para as rotas unificadas de monitor (Wave 0/1 — SDD).
/// Development: concede Monitor.Read + Monitor.Control automaticamente, sem credenciais, para
/// destravar o desenvolvimento local (dashboard chama /api/monitores/* sem login).
/// NÃO USAR fora de Development — outra wave troca por API key/JWT reais neste mesmo esquema.
/// </summary>
public sealed class DevelopmentMonitorAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "DevMonitorAuth";

    public DevelopmentMonitorAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(MonitorAuthPolicies.ScopeClaimType, MonitorAuthPolicies.ScopeRead),
            new Claim(MonitorAuthPolicies.ScopeClaimType, MonitorAuthPolicies.ScopeControl),
        };
        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

/// <summary>
/// Esquema padrão fora de Development: nega por padrão (401) até outra wave plugar autenticação
/// real (API key/JWT) — mantém as rotas unificadas fechadas em Homologação/Produção neste meio-tempo.
/// </summary>
public sealed class LockedMonitorAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "LockedMonitorAuth";

    public LockedMonitorAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync() =>
        Task.FromResult(AuthenticateResult.NoResult());
}
