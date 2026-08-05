using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Monitor.Application.Services;

namespace Monitor.Api.Security;

/// <summary>
/// Exige header X-Cte-Internal-Api-Key em /api/monitor/* quando Monitor:InternalApiKey está configurada.
/// Mesmo contrato em Development, Homologacao e Production.
/// Probes (/health, /health/ready), Swagger e SignalR hub permanecem abertos.
/// </summary>
public sealed class InternalApiKeyMiddleware
{
    public const string HeaderName = "X-Cte-Internal-Api-Key";

    private readonly RequestDelegate _next;

    public InternalApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IOptions<MonitorOptions> options)
    {
        var path = context.Request.Path.Value ?? string.Empty;
        if (!path.StartsWith("/api/monitor", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var expected = options.Value.InternalApiKey;
        if (string.IsNullOrWhiteSpace(expected))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(HeaderName, out var provided) ||
            string.IsNullOrWhiteSpace(provided))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = $"Header {HeaderName} ausente."
            });
            return;
        }

        if (!FixedTimeEquals(expected, provided.ToString()))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = "API key interna inválida."
            });
            return;
        }

        await _next(context);
    }

    private static bool FixedTimeEquals(string expected, string provided)
    {
        var a = Encoding.UTF8.GetBytes(expected);
        var b = Encoding.UTF8.GetBytes(provided);
        if (a.Length != b.Length)
        {
            CryptographicOperations.FixedTimeEquals(a, a);
            return false;
        }

        return CryptographicOperations.FixedTimeEquals(a, b);
    }
}

public static class InternalApiKeyMiddlewareExtensions
{
    public static IApplicationBuilder UseInternalApiKey(this IApplicationBuilder app) =>
        app.UseMiddleware<InternalApiKeyMiddleware>();
}
