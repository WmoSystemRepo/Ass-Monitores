using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CTe.Resgate.Application.Abstractions;
using CTe.Resgate.Application.Services;
using CTe.Resgate.Domain;
using CTe.Resgate.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Orquestrador.Api.Endpoints;

/// <summary>
/// Rotas do Resgate CT-e AN absorvidas no Orquestrador.Api (Wave 4 — SDD Monitor Unificado).
/// Antes hospedadas em processo próprio (7-Resgate/CTe.Resgate.Api, :5070); agora vivem aqui em
/// :5000 para eliminar a porta separada exigida pela UI do Orquestrador. Lógica copiada de
/// 7-Resgate/src/CTe.Resgate.Api/Program.cs — autenticação própria (JWT "ResgateJwt"), independente
/// do esquema de Monitor.Read/Monitor.Control usado pelas demais rotas.
/// </summary>
public static class ResgateEndpoints
{
    public const string JwtSchemeName = "ResgateJwt";
    public const string JwtIssuer = "cte-resgate";
    public const string JwtAudience = "cte-resgate";

    public static TokenValidationParameters BuildTokenValidationParameters(IConfiguration configuration)
    {
        var jwtKey = configuration["Auth:JwtKey"] ?? "DEV_ONLY_CHANGE_ME_RESGATE_CTE_32CHARS!!";
        var signing = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey));
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = JwtIssuer,
            ValidAudience = JwtAudience,
            IssuerSigningKey = signing
        };
    }

    public static IEndpointRouteBuilder MapResgateEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/token", (LoginRequest req, IConfiguration cfg, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("Resgate.Auth");
                var ok = DevFileAuth.TryValidate(
                    req.Usuario,
                    req.Senha,
                    cfg["Auth:UsersFile"],
                    cfg["Auth:Usuario"],
                    cfg["Auth:Senha"],
                    out var matchedUser);

                if (!ok || string.IsNullOrWhiteSpace(matchedUser))
                {
                    logger.LogWarning("Login DEV rejeitado para usuário {Usuario}", req.Usuario);
                    return Results.Json(
                        new
                        {
                            error = "Usuário ou senha inválidos.",
                            hint = "DEV: edite data/usuarios-dev.txt (padrão dev:dev)."
                        },
                        statusCode: StatusCodes.Status401Unauthorized);
                }

                var tvp = BuildTokenValidationParameters(cfg);
                var creds = new SigningCredentials(tvp.IssuerSigningKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: JwtIssuer,
                    audience: JwtAudience,
                    claims: [new Claim(ClaimTypes.Name, matchedUser), new Claim("role", "OperadorDev")],
                    expires: DateTime.UtcNow.AddHours(8),
                    signingCredentials: creds);
                return Results.Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiresIn = 28800,
                    usuario = matchedUser
                });
            })
            .WithTags("Resgate")
            .WithName("Resgate_Auth_Token");

        var api = app.MapGroup("/api/resgate")
            .WithTags("Resgate")
            .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtSchemeName });

        api.MapPost("/lotes", async (
                CreateLoteRequest body,
                ClaimsPrincipal user,
                IResgateService svc,
                ILoggerFactory loggerFactory,
                HttpContext http,
                CancellationToken ct) =>
            {
                var logger = loggerFactory.CreateLogger("Resgate.Lotes");
                var usuario = user.Identity?.Name ?? "anon";
                try
                {
                    var (result, errors) = await svc.EnfileirarDownloadAsync(usuario, body.Chaves ?? [], ct);
                    if (errors.Count > 0 || result is null)
                    {
                        logger.LogWarning("Enfileirar rejeitado ({Usuario}): {Errors}", usuario, string.Join("; ", errors));
                        return Results.BadRequest(new
                        {
                            errors,
                            hint = "Valide as chaves (44 dígitos, 1–1000).",
                            correlationId = GetCorrelationId(http)
                        });
                    }

                    logger.LogInformation(
                        "Download enfileirado por {Usuario}: {Qtd} chaves (pendentes temp={Pend}, fila={Fila})",
                        usuario, result.Enfileirados, result.PendentesTemp, result.ProfundidadeFilaBroker);

                    return Results.Ok(ToEnqueueResponse(result));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Falha ao enfileirar download (usuário {Usuario})", usuario);
                    return Results.Json(
                        new
                        {
                            errors = new[] { FriendlySqlOrConfigError(ex) },
                            detail = ex.Message,
                            hint = "Veja o log do Orquestrador.Api.",
                            correlationId = GetCorrelationId(http)
                        },
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithName("Resgate_Lotes_Create");

        api.MapPost("/lotes/upload", async (
                HttpRequest request,
                ClaimsPrincipal user,
                IResgateService svc,
                ILoggerFactory loggerFactory,
                CancellationToken ct) =>
            {
                var logger = loggerFactory.CreateLogger("Resgate.Lotes");
                if (!request.HasFormContentType) return Results.BadRequest(new { errors = new[] { "multipart esperado" } });
                var file = request.Form.Files.GetFile("file");
                if (file is null) return Results.BadRequest(new { errors = new[] { "arquivo obrigatório" } });
                if (file.Length > ChaveAccessRules.MaxUploadBytes)
                    return Results.BadRequest(new { errors = new[] { "arquivo > 5MB" } });

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (ext is not (".csv" or ".txt" or ".xlsx"))
                    return Results.BadRequest(new { errors = new[] { "Use .csv, .txt ou .xlsx" } });

                IEnumerable<string> lines;
                try
                {
                    await using var stream = file.OpenReadStream();
                    lines = ChavePlanilhaParser.Parse(stream, ext);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Falha ao ler planilha {File}", file.FileName);
                    return Results.BadRequest(new { errors = new[] { $"Falha ao ler planilha: {ex.Message}" } });
                }

                var usuario = user.Identity?.Name ?? "anon";
                try
                {
                    var (result, errors) = await svc.EnfileirarDownloadAsync(usuario, lines, ct);
                    if (errors.Count > 0 || result is null)
                    {
                        logger.LogWarning("Upload enfileirar rejeitado ({Usuario}): {Errors}", usuario, string.Join("; ", errors));
                        return Results.BadRequest(new { errors });
                    }

                    logger.LogInformation("Download (upload) enfileirado por {Usuario}: {Qtd} chaves", usuario, result.Enfileirados);
                    return Results.Ok(ToEnqueueResponse(result));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Falha ao enfileirar via upload (usuário {Usuario})", usuario);
                    return Results.Json(
                        new
                        {
                            errors = new[] { FriendlySqlOrConfigError(ex) },
                            detail = ex.Message,
                            hint = "Veja o log do Orquestrador.Api."
                        },
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithName("Resgate_Lotes_Upload");

        api.MapGet("/fila-download", async (IResgateService svc, CancellationToken ct) =>
            {
                var dto = await svc.GetFilaStatusAsync(ct);
                return Results.Ok(dto);
            })
            .WithName("Resgate_FilaDownload");

        api.MapPost("/status-chaves", async (StatusChavesRequest body, IResgateService svc, CancellationToken ct) =>
            {
                var chaves = body.Chaves ?? [];
                if (chaves.Count == 0)
                    return Results.BadRequest(new { errors = new[] { "Informe ao menos uma chave." } });
                var dto = await svc.GetStatusChavesAsync(chaves, ct);
                return Results.Ok(dto);
            })
            .WithName("Resgate_StatusChaves");

        return app;
    }

    private static string? GetCorrelationId(HttpContext http) =>
        http.Items.TryGetValue("X-Correlation-Id", out var v) ? v as string : null;

    private static object ToEnqueueResponse(CargaEnqueueResult result) => new
    {
        modo = "carga-download",
        enfileirados = result.Enfileirados,
        pendentesTemp = result.PendentesTemp,
        profundidadeFilaBroker = result.ProfundidadeFilaBroker,
        idadeMaxTempMinutos = result.IdadeMaxTempMinutos,
        ids = result.Ids,
        aviso = "Enfileirado não significa que o CT-e já foi resgatado. A Carga executa o download.",
        mensagem = "Chaves informadas ao início do Download (Carga). Requer Carga ligada (Executar/ExecutarAuto, CodServico 99).",
        checklistCarga = new
        {
            executar = "Executar=1",
            executarAuto = "ExecutarAuto=1",
            codServico = 99,
            monitor = "/monitores/carga"
        },
        riscoFila = "Fila compartilhada com Integrador — preferir janela com Carga ativa."
    };

    private static string FriendlySqlOrConfigError(Exception ex)
    {
        var msg = ex.Message ?? "";
        if (msg.Contains("BDCTeSintetico", StringComparison.OrdinalIgnoreCase) ||
            msg.Contains("ConnectionString", StringComparison.OrdinalIgnoreCase))
            return "Connection string BDCTeSintetico ausente ou inválida.";
        if (msg.Contains("tmp_integracao", StringComparison.OrdinalIgnoreCase) ||
            msg.Contains("fila_alvo_cte_integrador", StringComparison.OrdinalIgnoreCase) ||
            msg.Contains("servico_iniciador_cte_integrador", StringComparison.OrdinalIgnoreCase) ||
            msg.Contains("Invalid object name", StringComparison.OrdinalIgnoreCase))
            return "Fila/temp do Download (Carga) ausente no SQL — confira Service Broker e cte.tmp_integracao_conhecimento_transporte_eletronico.";
        if (msg.Contains("Login failed", StringComparison.OrdinalIgnoreCase) ||
            msg.Contains("network-related", StringComparison.OrdinalIgnoreCase) ||
            msg.Contains("não foi possível abrir", StringComparison.OrdinalIgnoreCase) ||
            msg.Contains("Could not open", StringComparison.OrdinalIgnoreCase))
            return $"Falha de conexão SQL: {msg}";
        return $"Erro ao enfileirar download: {msg}";
    }
}

public sealed record LoginRequest(string Usuario, string Senha);
public sealed record CreateLoteRequest(List<string>? Chaves);
public sealed record StatusChavesRequest(List<string>? Chaves);
