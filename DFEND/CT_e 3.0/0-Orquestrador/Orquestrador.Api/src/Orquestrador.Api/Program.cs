using System.Reflection;
using CTe.BuildingBlocks.Auth;
using CTe.Modules.Monitors.Infrastructure;
using CTe.Resgate.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orquestrador.Api.Auth;
using Orquestrador.Api.Endpoints;
using Orquestrador.Application.Abstractions;
using Orquestrador.Application.Options;
using Orquestrador.Application.Services;
using Orquestrador.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<OrchestratorOptions>()
    .Bind(builder.Configuration.GetSection(OrchestratorOptions.SectionName))
    .ValidateOnStart();
builder.Services.AddSingleton<IValidateOptions<OrchestratorOptions>, OrchestratorOptionsValidator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalAngular", policy =>
        policy.WithOrigins(
                "http://localhost:4220",
                "http://localhost:8080",
                "http://127.0.0.1:4220",
                "http://127.0.0.1:8080")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddOrquestradorInfrastructure();
builder.Services.AddUnifiedMonitorModules();

// Wave 4 (SDD Monitor Unificado): Resgate CT-e AN absorvido no Orquestrador.Api — deixa de exigir
// processo/porta próprios (antes 7-Resgate/CTe.Resgate.Api, :5070). Fonte permanece em 7-Resgate.
builder.Services.AddResgateInfrastructure(builder.Configuration);

// AuthN/AuthZ skeleton (Wave 0/1 — SDD Monitor Unificado).
// Development: esquema concede Monitor.Read + Monitor.Control sem credenciais (ver
// DevelopmentMonitorAuthHandler). Homologação/Produção: esquema fecha por padrão (401) até uma
// wave futura plugar API key/JWT reais — só as rotas novas /api/monitores/* exigem autorização;
// as rotas do orquestrador existentes continuam abertas (sem RequireAuthorization).
var monitorAuthScheme = builder.Environment.IsDevelopment()
    ? DevelopmentMonitorAuthHandler.SchemeName
    : LockedMonitorAuthHandler.SchemeName;

builder.Services.AddAuthentication(monitorAuthScheme)
    .AddScheme<AuthenticationSchemeOptions, DevelopmentMonitorAuthHandler>(DevelopmentMonitorAuthHandler.SchemeName, _ => { })
    .AddScheme<AuthenticationSchemeOptions, LockedMonitorAuthHandler>(LockedMonitorAuthHandler.SchemeName, _ => { })
    // Wave 4: esquema JWT próprio do Resgate (issuer/audience "cte-resgate"), independente do
    // esquema padrão de Monitor.Read/Monitor.Control acima — só as rotas /api/resgate/* e
    // /api/auth/token exigem esse esquema (ver ResgateEndpoints.MapResgateEndpoints).
    .AddJwtBearer(ResgateEndpoints.JwtSchemeName, o =>
    {
        o.TokenValidationParameters = ResgateEndpoints.BuildTokenValidationParameters(builder.Configuration);
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        MonitorAuthPolicies.MonitorRead,
        policy => policy.RequireClaim(MonitorAuthPolicies.ScopeClaimType, MonitorAuthPolicies.ScopeRead, MonitorAuthPolicies.ScopeControl));
    options.AddPolicy(
        MonitorAuthPolicies.MonitorControl,
        policy => policy.RequireClaim(MonitorAuthPolicies.ScopeClaimType, MonitorAuthPolicies.ScopeControl));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // 1 Swagger com tags por domínio (SDD): orquestrador + os 6 monitores unificados + Resgate
    // (Wave 4 — absorvido neste processo, autenticação JWT própria "ResgateJwt").
    options.SwaggerDoc("v1", new() { Title = "Orquestrador.Api", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Bearer (Resgate) — obtenha via POST /api/auth/token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Orquestrador.Api v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseCors("LocalAngular");

// AuthN/AuthZ (skeleton, W0/W1): só endpoints com .RequireAuthorization() exigem token/claims —
// sem FallbackPolicy, as rotas do orquestrador abaixo continuam públicas como antes.
app.UseAuthentication();
app.UseAuthorization();

// Grupo só para agrupar as rotas legadas do orquestrador sob a tag "Orquestrador" no Swagger
// (MapGroup com prefixo vazio não altera as rotas, só adiciona metadados).
var orchestrator = app.MapGroup(string.Empty).WithTags("Orquestrador");

app.Use(async (ctx, next) =>
{
    var correlationId = ctx.Request.Headers[OrchestratorOptions.CorrelationIdHeaderName].FirstOrDefault();
    if (string.IsNullOrWhiteSpace(correlationId))
    {
        correlationId = Guid.NewGuid().ToString("N");
    }

    ctx.Response.Headers[OrchestratorOptions.CorrelationIdHeaderName] = correlationId;
    ctx.Items[OrchestratorOptions.CorrelationIdHeaderName] = correlationId;
    using (ctx.RequestServices.GetRequiredService<ILoggerFactory>()
               .CreateLogger("Correlation")
               .BeginScope(new Dictionary<string, object>
               {
                   ["CorrelationId"] = correlationId,
                   ["RequestId"] = ctx.TraceIdentifier,
                   ["Host"] = Environment.MachineName,
                   ["Ambiente"] = app.Environment.EnvironmentName,
                   ["Versao"] = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0"
               }))
    {
        await next();
    }
});

app.Use(async (ctx, next) =>
{
    var opts = ctx.RequestServices.GetRequiredService<IOptions<OrchestratorOptions>>().Value;
    ctx.Response.OnStarting(() =>
    {
        ctx.Response.Headers["X-Monitor-Service"] = opts.ServiceId;
        ctx.Response.Headers["X-Monitor-Version"] = opts.ApiVersion;
        return Task.CompletedTask;
    });
    await next();
});

orchestrator.MapGet("/api/orchestrator/info", (IOptions<OrchestratorOptions> options) =>
{
    var o = options.Value;
    var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";
    return Results.Ok(new
    {
        serviceId = o.ServiceId,
        displayName = o.DisplayName,
        domain = o.Domain,
        version,
        apiVersion = o.ApiVersion,
        systems = o.Systems.Select(s => new
        {
            s.Id,
            s.DisplayName,
            s.Symbol,
            s.Version,
            s.Order,
            s.DependsOn,
            baseUrl = s.ResolveApiBase(),
            frontendUrl = s.ResolveFrontendUrl(),
            s.Enabled,
            endpoints = s.Endpoints,
            ui = s.Ui,
            instances = s.Instances
        }),
        registrySchemaVersion = o.RegistrySchemaVersion,
        endpoints = new[]
        {
            "/api/orchestrator/snapshot",
            "/api/orchestrator/status",
            "/api/orchestrator/start",
            "/api/orchestrator/stop",
            "/api/orchestrator/ensure-stacks",
            "/api/orchestrator/systems/{id}/ensure-open",
            "/api/orchestrator/info",
            "/api/v1/orchestrator/snapshot",
            "/api/v1/orchestrator/status",
            "/api/v1/orchestrator/start",
            "/api/v1/orchestrator/stop",
            "/api/v1/orchestrator/ensure-stacks",
            "/api/chain/health",
            "/health",
            "/health/live",
            "/health/ready",
            "/api/health/live",
            "/api/health/ready",
            "/api/monitores/{servico}/info",
            "/api/monitores/{servico}/snapshot",
            "/api/monitores/{servico}/logs",
            "/api/monitores/{servico}/tables/{key}",
            "/api/monitores/{servico}/service/status",
            "/api/monitores/{servico}/service/start",
            "/api/monitores/{servico}/service/stop",
            "/api/monitores/{servico}/health"
        }
    });
});

orchestrator.MapGet("/api/orchestrator/snapshot", async (ChainSnapshotAggregator aggregator, CancellationToken ct) =>
{
    var snapshot = await aggregator.BuildAsync(ct);
    return Results.Ok(snapshot);
});

orchestrator.MapGet("/api/orchestrator/status", (CascadeControlService cascade) =>
{
    var (phase, message) = cascade.GetStatus();
    return Results.Ok(new
    {
        cascadePhase = ChainSnapshotAggregator.ToPhaseString(phase),
        cascadeMessage = message
    });
});

orchestrator.MapPost("/api/orchestrator/start", async (CascadeControlService cascade, CancellationToken ct) =>
{
    var (success, message) = await cascade.StartAsync(ct);
    var (phase, _) = cascade.GetStatus();
    var payload = new
    {
        success,
        cascadePhase = ChainSnapshotAggregator.ToPhaseString(phase),
        cascadeMessage = message
    };
    return success ? Results.Ok(payload) : Results.BadRequest(payload);
});

orchestrator.MapPost("/api/orchestrator/stop", async (CascadeControlService cascade, CancellationToken ct) =>
{
    var (success, message) = await cascade.StopAsync(ct);
    var (phase, _) = cascade.GetStatus();
    var payload = new
    {
        success,
        cascadePhase = ChainSnapshotAggregator.ToPhaseString(phase),
        cascadeMessage = message
    };
    return success ? Results.Ok(payload) : Results.BadRequest(payload);
});

/// <summary>
/// Sobe/valida API + Angular de todos os Enabled (sem ligar workers).
/// Chamado no boot do front do Orquestrador.
/// </summary>
orchestrator.MapPost("/api/orchestrator/ensure-stacks", async (CascadeControlService cascade, CancellationToken ct) =>
{
    var (success, message, ready, total) = await cascade.EnsureAllStacksAsync(ct);
    var payload = new
    {
        success,
        message,
        readyCount = ready,
        totalCount = total
    };
    return success ? Results.Ok(payload) : Results.BadRequest(payload);
});

orchestrator.MapPost("/api/orchestrator/systems/{id}/ensure-open", async (
    string id,
    SystemOpenService openService,
    CancellationToken ct) =>
{
    var result = await openService.EnsureAndOpenAsync(id, ct);
    var payload = new
    {
        success = result.Success,
        systemId = result.SystemId,
        frontendUrl = result.FrontendUrl,
        apiReady = result.ApiReady,
        apiStarted = result.ApiStarted,
        frontendReachable = result.FrontendReachable,
        frontendStarted = result.FrontendStarted,
        message = result.Message
    };

    if (!result.Success && result.FrontendUrl is null)
    {
        return Results.NotFound(payload);
    }

    // Só 200 quando API + front estão prontos (não abrir URL morta).
    return result.Success ? Results.Ok(payload) : Results.BadRequest(payload);
});

orchestrator.MapGet("/api/chain/health", async (
    IOptions<OrchestratorOptions> options,
    IMonitorClient client,
    CancellationToken ct) =>
{
    var systems = new List<object>();
    foreach (var cfg in options.Value.Systems)
    {
        if (!cfg.Enabled)
        {
            systems.Add(new
            {
                cfg.Id,
                cfg.DisplayName,
                status = "disabled",
                baseUrl = cfg.ResolveApiBase()
            });
            continue;
        }

        var ready = await client.PingReadyAsync(cfg, ct);
        if (!ready)
        {
            systems.Add(new
            {
                cfg.Id,
                cfg.DisplayName,
                status = "offline",
                baseUrl = cfg.ResolveApiBase()
            });
            continue;
        }

        var snap = await client.GetSnapshotAsync(cfg, ct);
        snap.Value?.Dispose();
        var status = snap.ErrorKind switch
        {
            "unauthorized" => "unauthorized",
            "offline" => "offline",
            { } => "error",
            null => "online"
        };

        systems.Add(new
        {
            cfg.Id,
            cfg.DisplayName,
            status,
            baseUrl = cfg.ResolveApiBase(),
            message = snap.Message
        });
    }

    return Results.Ok(new
    {
        status = "ok",
        systems,
        checkedAtUtc = DateTimeOffset.UtcNow
    });
});

orchestrator.MapGet("/health", (IOptions<OrchestratorOptions> options, IHostEnvironment env) =>
    Results.Ok(new
    {
        status = "ok",
        env = env.EnvironmentName,
        serviceId = options.Value.ServiceId
    }));

orchestrator.MapGet("/health/live", (IOptions<OrchestratorOptions> options, IHostEnvironment env) =>
    Results.Ok(new
    {
        status = "ok",
        probe = "liveness",
        env = env.EnvironmentName,
        serviceId = options.Value.ServiceId
    }));

orchestrator.MapGet("/health/ready", (IOptions<OrchestratorOptions> options, IHostEnvironment env) =>
{
    // Ready = processo + registry válido (ValidateOnStart). Monitores offline não derrubam o BFF.
    var o = options.Value;
    return Results.Ok(new
    {
        status = "ready",
        probe = "readiness",
        env = env.EnvironmentName,
        serviceId = o.ServiceId,
        enabledSystems = o.Systems.Count(s => s.Enabled),
        hasInternalApiKey = !string.IsNullOrWhiteSpace(o.InternalApiKey),
        registrySchemaVersion = o.RegistrySchemaVersion
    });
});

// Aliases /api/v1 (contrato versionado sem breaking)
orchestrator.MapGet("/api/v1/orchestrator/info", () => Results.Redirect("/api/orchestrator/info"));
orchestrator.MapGet("/api/v1/orchestrator/snapshot", async (ChainSnapshotAggregator aggregator, CancellationToken ct) =>
{
    var snapshot = await aggregator.BuildAsync(ct);
    return Results.Ok(snapshot);
});
orchestrator.MapGet("/api/v1/orchestrator/status", (CascadeControlService cascade) =>
{
    var (phase, message) = cascade.GetStatus();
    return Results.Ok(new
    {
        cascadePhase = ChainSnapshotAggregator.ToPhaseString(phase),
        cascadeMessage = message
    });
});
orchestrator.MapPost("/api/v1/orchestrator/start", async (CascadeControlService cascade, CancellationToken ct) =>
{
    var (success, message) = await cascade.StartAsync(ct);
    var (phase, _) = cascade.GetStatus();
    var payload = new
    {
        success,
        cascadePhase = ChainSnapshotAggregator.ToPhaseString(phase),
        cascadeMessage = message
    };
    return success ? Results.Ok(payload) : Results.BadRequest(payload);
});
orchestrator.MapPost("/api/v1/orchestrator/stop", async (CascadeControlService cascade, CancellationToken ct) =>
{
    var (success, message) = await cascade.StopAsync(ct);
    var (phase, _) = cascade.GetStatus();
    var payload = new
    {
        success,
        cascadePhase = ChainSnapshotAggregator.ToPhaseString(phase),
        cascadeMessage = message
    };
    return success ? Results.Ok(payload) : Results.BadRequest(payload);
});
orchestrator.MapPost("/api/v1/orchestrator/ensure-stacks", async (CascadeControlService cascade, CancellationToken ct) =>
{
    var (success, message, ready, total) = await cascade.EnsureAllStacksAsync(ct);
    var payload = new { success, message, readyCount = ready, totalCount = total };
    return success ? Results.Ok(payload) : Results.BadRequest(payload);
});

// Aliases /api/health/* (mesmo comportamento de /health/*) — ready NÃO depende dos monitores.
orchestrator.MapGet("/api/health/live", (IOptions<OrchestratorOptions> options, IHostEnvironment env) =>
    Results.Ok(new
    {
        status = "ok",
        probe = "liveness",
        env = env.EnvironmentName,
        serviceId = options.Value.ServiceId
    }));

orchestrator.MapGet("/api/health/ready", (IOptions<OrchestratorOptions> options, IHostEnvironment env) =>
{
    var o = options.Value;
    return Results.Ok(new
    {
        status = "ready",
        probe = "readiness",
        env = env.EnvironmentName,
        serviceId = o.ServiceId,
        enabledSystems = o.Systems.Count(s => s.Enabled),
        hasInternalApiKey = !string.IsNullOrWhiteSpace(o.InternalApiKey),
        registrySchemaVersion = o.RegistrySchemaVersion
    });
});

// Rotas unificadas dos monitores (SDD): /api/monitores/{servico}/* — Monitor.Read/Monitor.Control.
app.MapUnifiedMonitorEndpoints();

// Wave 4: rotas do Resgate CT-e AN (/api/auth/token, /api/resgate/*) — JWT próprio ("ResgateJwt").
app.MapResgateEndpoints();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();

public partial class Program;
