using System.Reflection;
using Microsoft.Extensions.Options;
using Monitor.Api.Security;
using Monitor.Application.Abstractions;
using Monitor.Application.Services;
using Monitor.Domain.Alerts;
using Monitor.Infrastructure;
using Monitor.Infrastructure.Realtime;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MonitorOptions>(builder.Configuration.GetSection(MonitorOptions.SectionName));
builder.Services.Configure<AlertThresholdOptions>(builder.Configuration.GetSection(AlertThresholdOptions.SectionName));

builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalAngular", policy =>
        policy.WithOrigins(
                "http://localhost:4230",
                "http://localhost:8080",
                "http://127.0.0.1:4230",
                "http://127.0.0.1:8080")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddSignalR();
builder.Services.AddMonitorInfrastructure();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Monitor.Api (Sintetizador)", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Monitor.Api v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseCors("LocalAngular");
app.UseInternalApiKey();

app.Use(async (ctx, next) =>
{
    var opts = ctx.RequestServices.GetRequiredService<IOptions<MonitorOptions>>().Value;
    ctx.Response.OnStarting(() =>
    {
        ctx.Response.Headers["X-Monitor-Service"] = opts.ServiceId;
        ctx.Response.Headers["X-Monitor-Version"] = opts.ApiVersion;
        return Task.CompletedTask;
    });
    await next();
});

app.MapGet("/api/monitor/info", (IOptions<MonitorOptions> options) =>
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
        monitoredService = o.MonitoredService,
        endpoints = new[]
        {
            "/api/monitor/snapshot",
            "/api/monitor/logs",
            "/api/monitor/tables/{key}",
            "/api/monitor/service/status",
            "/api/monitor/service/start",
            "/api/monitor/service/stop",
            "/api/monitor/info",
            "/hubs/monitor",
            "/health",
            "/health/ready"
        }
    });
});

app.MapGet("/api/monitor/snapshot", async (ISnapshotAggregator aggregator, CancellationToken ct) =>
{
    var snapshot = await aggregator.BuildSnapshotAsync(ct);
    return Results.Ok(snapshot);
});

app.MapGet("/api/monitor/logs", async (IMonitorReadRepository repo, long? afterSeq, int? take, CancellationToken ct) =>
{
    var logs = await repo.GetLogsAfterAsync(afterSeq ?? 0, take is > 0 and <= 1000 ? take.Value : 300, ct);
    return Results.Ok(logs.Items);
});

app.MapGet("/api/monitor/tables/{key}", async (
    string key,
    ITableDetailService tables,
    int? take,
    CancellationToken ct) =>
{
    var detail = await tables.GetAsync(key, take ?? 1000, ct);
    return detail is null ? Results.NotFound(new { message = $"Tabela '{key}' não encontrada." }) : Results.Ok(detail);
});

app.MapGet("/api/monitor/service/status", (ServiceControlService control) =>
{
    var status = control.GetStatus();
    return Results.Ok(status);
});

app.MapPost("/api/monitor/service/start", async (ServiceControlService control, CancellationToken ct) =>
{
    var result = await control.StartAsync(ct);
    return result.Success ? Results.Ok(result) : Results.BadRequest(result);
});

app.MapPost("/api/monitor/service/stop", async (ServiceControlService control, CancellationToken ct) =>
{
    var result = await control.StopAsync(ct);
    return result.Success ? Results.Ok(result) : Results.BadRequest(result);
});

app.MapHub<MonitorHub>("/hubs/monitor");

app.MapGet("/health", (IOptions<MonitorOptions> options, IHostEnvironment env) =>
    Results.Ok(new
    {
        status = "ok",
        probe = "liveness",
        env = env.EnvironmentName,
        serviceId = options.Value.ServiceId
    }));

app.MapGet("/health/live", (IOptions<MonitorOptions> options, IHostEnvironment env) =>
    Results.Ok(new
    {
        status = "ok",
        probe = "liveness",
        env = env.EnvironmentName,
        serviceId = options.Value.ServiceId
    }));

app.MapGet("/health/ready", async (IMonitorReadRepository repo, CancellationToken ct) =>
{
    var primaryOk = await repo.PingAsync(ct);
    var sinteticoOk = await repo.PingSinteticoAsync(ct);
    if (!primaryOk || !sinteticoOk)
    {
        return Results.Json(
            new
            {
                status = "unhealthy",
                probe = "readiness",
                primary = primaryOk,
                sintetico = sinteticoOk
            },
            statusCode: StatusCodes.Status503ServiceUnavailable);
    }

    return Results.Ok(new { status = "ready", probe = "readiness", primary = true, sintetico = true });
});

app.MapGet("/api/v1/monitor/service/status", (ServiceControlService control) => Results.Ok(control.GetStatus()));
app.MapPost("/api/v1/monitor/service/start", async (ServiceControlService control, CancellationToken ct) =>
{
    var result = await control.StartAsync(ct);
    return result.Success ? Results.Ok(result) : Results.BadRequest(result);
});
app.MapPost("/api/v1/monitor/service/stop", async (ServiceControlService control, CancellationToken ct) =>
{
    var result = await control.StopAsync(ct);
    return result.Success ? Results.Ok(result) : Results.BadRequest(result);
});
app.MapGet("/api/v1/monitor/snapshot", async (ISnapshotAggregator aggregator, CancellationToken ct) =>
    Results.Ok(await aggregator.BuildSnapshotAsync(ct)));

// VS / browser na raiz → Swagger (fluxo acostumado)
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();

public partial class Program;
