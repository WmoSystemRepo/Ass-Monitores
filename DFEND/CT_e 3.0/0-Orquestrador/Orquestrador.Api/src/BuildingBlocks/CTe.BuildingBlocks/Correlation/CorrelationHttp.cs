using Microsoft.AspNetCore.Http;

namespace CTe.BuildingBlocks.Correlation;

/// <summary>
/// Helper thin para o cabeçalho de correlação (X-Correlation-Id) entre Orquestrador e monitores.
/// Não é middleware — cada API decide onde/como propagar (ver Orquestrador.Api Program.cs).
/// </summary>
public static class CorrelationHttp
{
    public const string HeaderName = "X-Correlation-Id";

    /// <summary>Lê o header da requisição ou gera um novo id (sem gravar na resposta).</summary>
    public static string GetOrCreate(HttpContext context)
    {
        var value = context.Request.Headers[HeaderName].ToString();
        return string.IsNullOrWhiteSpace(value) ? NewId() : value;
    }

    public static string NewId() => Guid.NewGuid().ToString("N");
}
