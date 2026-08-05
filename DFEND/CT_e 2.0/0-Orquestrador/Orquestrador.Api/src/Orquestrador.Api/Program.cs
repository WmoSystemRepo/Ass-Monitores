using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Orquestrador.Api", Version = "v1" });
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

app.MapGet("/api/orchestrator/info", (IOptions<OrchestratorOptions> options) =>
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
            "/health/ready"
        }
    });
});

app.MapGet("/api/orchestrator/snapshot", async (ChainSnapshotAggregator aggregator, CancellationToken ct) =>
{
    var snapshot = await aggregator.BuildAsync(ct);
    return Results.Ok(snapshot);
});

app.MapGet("/api/orchestrator/status", (CascadeControlService cascade) =>
{
    var (phase, message) = cascade.GetStatus();
    return Results.Ok(new
    {
        cascadePhase = ChainSnapshotAggregator.ToPhaseString(phase),
        cascadeMessage = message
    });
});

app.MapPost("/api/orchestrator/start", async (CascadeControlService cascade, CancellationToken ct) =>
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

app.MapPost("/api/orchestrator/stop", async (CascadeControlService cascade, CancellationToken ct) =>
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
app.MapPost("/api/orchestrator/ensure-stacks", async (CascadeControlService cascade, CancellationToken ct) =>
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

app.MapPost("/api/orchestrator/systems/{id}/ensure-open", async (
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

app.MapGet("/api/chain/health", async (
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

app.MapGet("/health", (IOptions<OrchestratorOptions> options, IHostEnvironment env) =>
    Results.Ok(new
    {
        status = "ok",
        env = env.EnvironmentName,
        serviceId = options.Value.ServiceId
    }));

app.MapGet("/health/live", (IOptions<OrchestratorOptions> options, IHostEnvironment env) =>
    Results.Ok(new
    {
        status = "ok",
        probe = "liveness",
        env = env.EnvironmentName,
        serviceId = options.Value.ServiceId
    }));

app.MapGet("/health/ready", (IOptions<OrchestratorOptions> options, IHostEnvironment env) =>
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
app.MapGet("/api/v1/orchestrator/info", () => Results.Redirect("/api/orchestrator/info"));
app.MapGet("/api/v1/orchestrator/snapshot", async (ChainSnapshotAggregator aggregator, CancellationToken ct) =>
{
    var snapshot = await aggregator.BuildAsync(ct);
    return Results.Ok(snapshot);
});
app.MapGet("/api/v1/orchestrator/status", (CascadeControlService cascade) =>
{
    var (phase, message) = cascade.GetStatus();
    return Results.Ok(new
    {
        cascadePhase = ChainSnapshotAggregator.ToPhaseString(phase),
        cascadeMessage = message
    });
});
app.MapPost("/api/v1/orchestrator/start", async (CascadeControlService cascade, CancellationToken ct) =>
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
app.MapPost("/api/v1/orchestrator/stop", async (CascadeControlService cascade, CancellationToken ct) =>
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
app.MapPost("/api/v1/orchestrator/ensure-stacks", async (CascadeControlService cascade, CancellationToken ct) =>
{
    var (success, message, ready, total) = await cascade.EnsureAllStacksAsync(ct);
    var payload = new { success, message, readyCount = ready, totalCount = total };
    return success ? Results.Ok(payload) : Results.BadRequest(payload);
});

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();

public partial class Program;
