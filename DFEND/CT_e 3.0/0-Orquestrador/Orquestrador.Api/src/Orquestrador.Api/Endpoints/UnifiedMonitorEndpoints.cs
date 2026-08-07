using System.Text.Json;
using CTe.BuildingBlocks.Auth;
using CTe.BuildingBlocks.Correlation;
using CTe.Modules.Monitors.Abstractions;
using CTe.Modules.Monitors.Infrastructure;

namespace Orquestrador.Api.Endpoints;

/// <summary>
/// Rotas unificadas <c>/api/monitores/{servico}/*</c> (SDD Monitor Unificado, W1) — um único
/// Swagger, tags por serviço, AuthZ Monitor.Read/Monitor.Control. Mapeadas com o servico literal
/// (não wildcard) por serviço conhecido: dá tag de Swagger própria por serviço e 404 automático
/// (via roteamento) para qualquer servico fora da lista.
/// </summary>
public static class UnifiedMonitorEndpoints
{
    public static IEndpointRouteBuilder MapUnifiedMonitorEndpoints(this IEndpointRouteBuilder app)
    {
        foreach (var servico in DependencyInjection.KnownMonitorServiceIds)
        {
            var tag = ToTag(servico);
            var group = app.MapGroup($"/api/monitores/{servico}").WithTags(tag);

            group.MapGet("/info", (IMonitorModuleRegistry registry, CancellationToken ct) =>
                    InvokeReadAsync(registry, servico, (m, c) => m.GetInfoAsync(c), ct))
                .RequireAuthorization(MonitorAuthPolicies.MonitorRead)
                .WithName($"Monitores_{tag}_Info");

            group.MapGet("/snapshot", (IMonitorModuleRegistry registry, CancellationToken ct) =>
                    InvokeReadAsync(registry, servico, (m, c) => m.GetSnapshotAsync(c), ct))
                .RequireAuthorization(MonitorAuthPolicies.MonitorRead)
                .WithName($"Monitores_{tag}_Snapshot");

            group.MapGet("/logs", (IMonitorModuleRegistry registry, long? afterSeq, int? take, CancellationToken ct) =>
                    InvokeReadAsync(registry, servico, (m, c) => m.GetLogsAsync(afterSeq ?? 0, take ?? 300, c), ct))
                .RequireAuthorization(MonitorAuthPolicies.MonitorRead)
                .WithName($"Monitores_{tag}_Logs");

            group.MapGet("/tables/{key}", (string key, IMonitorModuleRegistry registry, int? take, CancellationToken ct) =>
                    InvokeReadAsync(registry, servico, (m, c) => m.GetTableAsync(key, ClampTableTake(take), c), ct))
                .RequireAuthorization(MonitorAuthPolicies.MonitorRead)
                .WithName($"Monitores_{tag}_Table");

            group.MapGet("/service/status", (IMonitorModuleRegistry registry, CancellationToken ct) =>
                    InvokeReadAsync(registry, servico, (m, c) => m.GetServiceStatusAsync(c), ct))
                .RequireAuthorization(MonitorAuthPolicies.MonitorRead)
                .WithName($"Monitores_{tag}_ServiceStatus");

            group.MapGet("/health", (IMonitorModuleRegistry registry, CancellationToken ct) =>
                    InvokeReadAsync(registry, servico, (m, c) => m.GetHealthAsync(c), ct))
                .RequireAuthorization(MonitorAuthPolicies.MonitorRead)
                .WithName($"Monitores_{tag}_Health");

            group.MapGet("/queues/proof", (IMonitorModuleRegistry registry, CancellationToken ct) =>
                    InvokeReadAsync(registry, servico, (m, c) => m.GetQueueProofAsync(c), ct))
                .RequireAuthorization(MonitorAuthPolicies.MonitorRead)
                .WithName($"Monitores_{tag}_QueuesProof");

            group.MapPost("/service/start", (HttpContext http, IMonitorModuleRegistry registry, CancellationToken ct) =>
                    InvokeActionAsync(registry, servico, "start", http, (m, c) => m.StartAsync(c), ct))
                .RequireAuthorization(MonitorAuthPolicies.MonitorControl)
                .WithName($"Monitores_{tag}_Start");

            group.MapPost("/service/stop", (HttpContext http, IMonitorModuleRegistry registry, CancellationToken ct) =>
                    InvokeActionAsync(registry, servico, "stop", http, (m, c) => m.StopAsync(c), ct))
                .RequireAuthorization(MonitorAuthPolicies.MonitorControl)
                .WithName($"Monitores_{tag}_Stop");
        }

        return app;
    }

    private static async Task<IResult> InvokeReadAsync(
        IMonitorModuleRegistry registry,
        string servico,
        Func<IMonitorModule, CancellationToken, Task<object?>> action,
        CancellationToken ct)
    {
        var module = registry.Get(servico);
        if (module is null)
        {
            return Results.NotFound(new { message = $"Serviço '{servico}' não encontrado." });
        }

        try
        {
            var result = await action(module, ct);
            return result is null
                ? Results.NotFound(new { message = $"Sem dados disponíveis para '{servico}' no momento." })
                : Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.Json(
                new { message = $"Falha ao consultar '{servico}'.", detail = ex.Message },
                statusCode: StatusCodes.Status502BadGateway);
        }
    }

    private static async Task<IResult> InvokeActionAsync(
        IMonitorModuleRegistry registry,
        string servico,
        string operacao,
        HttpContext http,
        Func<IMonitorModule, CancellationToken, Task<object?>> action,
        CancellationToken ct)
    {
        var module = registry.Get(servico);
        if (module is null)
        {
            return Results.NotFound(new { message = $"Serviço '{servico}' não encontrado." });
        }

        var correlationId = CorrelationHttp.GetOrCreate(http);

        try
        {
            var raw = await action(module, ct);
            if (raw is null)
            {
                var offline = new UnifiedServiceActionResult(
                    servico,
                    EstadoAtual: "offline",
                    OperacaoNecessaria: operacao,
                    Resultado: false,
                    CorrelationId: correlationId,
                    Mensagem: $"Monitor '{servico}' indisponível.");
                return Results.Json(offline, statusCode: StatusCodes.Status502BadGateway);
            }

            var normalized = NormalizeActionResult(servico, operacao, correlationId, raw);
            return normalized.Resultado ? Results.Ok(normalized) : Results.BadRequest(normalized);
        }
        catch (Exception ex)
        {
            var fallback = new UnifiedServiceActionResult(
                servico,
                EstadoAtual: null,
                OperacaoNecessaria: operacao,
                Resultado: false,
                CorrelationId: correlationId,
                Mensagem: ex.Message);
            return Results.Json(fallback, statusCode: StatusCodes.Status502BadGateway);
        }
    }

    /// <summary>Normaliza o payload cru do monitor (ServiceControlResult-like) para o shape unificado.</summary>
    private static UnifiedServiceActionResult NormalizeActionResult(
        string servico,
        string operacao,
        string correlationId,
        object raw)
    {
        bool resultado = true;
        string? estadoAtual = null;
        string? mensagem = null;
        string? commandId = null;

        if (raw is JsonElement el && el.ValueKind == JsonValueKind.Object)
        {
            resultado = TryGetBool(el, "success") ?? true;
            estadoAtual = TryGetString(el, "status") ?? TryGetString(el, "scmStatus");
            mensagem = TryGetString(el, "message");
            commandId = TryGetString(el, "commandId");
        }
        else
        {
            // Records vindos de IMonitorClient (MonitorActionResult) — refletidos via reflexão leve.
            var type = raw.GetType();
            resultado = (bool?)type.GetProperty("Success")?.GetValue(raw) ?? true;
            estadoAtual = (string?)type.GetProperty("Status")?.GetValue(raw);
            mensagem = (string?)type.GetProperty("Message")?.GetValue(raw);
            commandId = (string?)type.GetProperty("CommandId")?.GetValue(raw);
        }

        return new UnifiedServiceActionResult(
            servico,
            estadoAtual,
            operacao,
            resultado,
            commandId ?? correlationId,
            mensagem);
    }

    private static string? TryGetString(JsonElement root, string name)
    {
        foreach (var prop in root.EnumerateObject())
        {
            if (!prop.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (prop.Value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
            {
                return null;
            }

            return prop.Value.ValueKind == JsonValueKind.String ? prop.Value.GetString() : prop.Value.ToString();
        }

        return null;
    }

    private static bool? TryGetBool(JsonElement root, string name)
    {
        foreach (var prop in root.EnumerateObject())
        {
            if (!prop.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            return prop.Value.ValueKind switch
            {
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.String when bool.TryParse(prop.Value.GetString(), out var b) => b,
                _ => null
            };
        }

        return null;
    }

    /// <summary>Contrato único de detalhe de tabela: take 1–1000 (default 1000).</summary>
    private static int ClampTableTake(int? take) =>
        Math.Clamp(take is null or <= 0 ? 1000 : take.Value, 1, 1000);

    private static string ToTag(string servico) =>
        servico.Length == 0 ? servico : char.ToUpperInvariant(servico[0]) + servico[1..];
}

/// <summary>Resposta normalizada de start/stop unificado (aproxima ServiceControlResult do Receptor).</summary>
public sealed record UnifiedServiceActionResult(
    string Servico,
    string? EstadoAtual,
    string? OperacaoNecessaria,
    bool Resultado,
    string? CorrelationId,
    string? Mensagem = null);
