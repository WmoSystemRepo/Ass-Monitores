using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CTe.Resgate.Api.Logging;
using CTe.Resgate.Application.Abstractions;
using CTe.Resgate.Application.Services;
using CTe.Resgate.Domain;
using CTe.Resgate.Infrastructure;
using CTe.Resgate.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
ResgateFileLogging.Configure(builder);

builder.Services.AddResgateInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CTe.Resgate.Api", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Bearer",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var jwtKey = builder.Configuration["Auth:JwtKey"] ?? "DEV_ONLY_CHANGE_ME_RESGATE_CTE_32CHARS!!";
var signing = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = "cte-resgate",
            ValidAudience = "cte-resgate",
            IssuerSigningKey = signing
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddCors(o => o.AddPolicy("Orquestrador", p =>
    p.WithOrigins("http://localhost:4220", "http://127.0.0.1:4220")
        .AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseSerilogRequestLogging(opts =>
{
    opts.GetLevel = (ctx, _, ex) =>
        ex is not null || ctx.Response.StatusCode >= 500
            ? Serilog.Events.LogEventLevel.Error
            : ctx.Response.StatusCode >= 400
                ? Serilog.Events.LogEventLevel.Warning
                : Serilog.Events.LogEventLevel.Information;
});

app.Use(async (ctx, next) =>
{
    const string header = "X-Correlation-Id";
    if (!ctx.Request.Headers.TryGetValue(header, out var cid) || string.IsNullOrWhiteSpace(cid))
        cid = Guid.NewGuid().ToString("N");
    ctx.Response.Headers[header] = cid!;
    ctx.Items["CorrelationId"] = cid.ToString();
    using (Serilog.Context.LogContext.PushProperty("CorrelationId", cid.ToString()))
    {
        await next();
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Orquestrador");
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health/live", () => Results.Ok(new { status = "Live" }));
app.MapGet("/health/ready", (IConfiguration cfg) =>
{
    var csOk = !string.IsNullOrWhiteSpace(cfg.GetConnectionString("BDCTeSintetico"));
    return Results.Ok(new
    {
        status = csOk ? "Ready" : "NotReady",
        banco = csOk ? "SqlServer" : "MissingConnectionString",
        modo = "carga-download-enqueue",
        worker = "Disabled — Carga ProcessarDownload consome a fila",
        logs = ResgateFileLogging.ResolveLogDirectory(cfg)
    });
});

app.MapPost("/api/auth/token", (LoginRequest req, IConfiguration cfg) =>
{
    var ok = DevFileAuth.TryValidate(
        req.Usuario,
        req.Senha,
        cfg["Auth:UsersFile"],
        cfg["Auth:Usuario"],
        cfg["Auth:Senha"],
        out var matchedUser);

    if (!ok || string.IsNullOrWhiteSpace(matchedUser))
    {
        Log.Warning("Login DEV rejeitado para usuário {Usuario}", req.Usuario);
        return Results.Json(
            new
            {
                error = "Usuário ou senha inválidos.",
                hint = "DEV: edite 7-Resgate/data/usuarios-dev.txt (padrão dev:dev)."
            },
            statusCode: StatusCodes.Status401Unauthorized);
    }

    var creds = new SigningCredentials(signing, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: "cte-resgate",
        audience: "cte-resgate",
        claims: [new Claim(ClaimTypes.Name, matchedUser), new Claim("role", "OperadorDev")],
        expires: DateTime.UtcNow.AddHours(8),
        signingCredentials: creds);
    return Results.Ok(new
    {
        token = new JwtSecurityTokenHandler().WriteToken(token),
        expiresIn = 28800,
        usuario = matchedUser
    });
});

var api = app.MapGroup("/api/resgate").RequireAuthorization();

api.MapPost("/lotes", async (CreateLoteRequest body, ClaimsPrincipal user, IResgateService svc, CancellationToken ct) =>
{
    var usuario = user.Identity?.Name ?? "anon";
    try
    {
        var (result, errors) = await svc.EnfileirarDownloadAsync(usuario, body.Chaves ?? [], ct);
        if (errors.Count > 0 || result is null)
        {
            Log.Warning("Enfileirar rejeitado ({Usuario}): {Errors}", usuario, string.Join("; ", errors));
            return Results.BadRequest(new { errors, hint = "Valide as chaves (44 dígitos, 1–1000)." });
        }

        Log.Information(
            "Download enfileirado por {Usuario}: {Qtd} chaves (pendentes temp={Pend}, fila={Fila})",
            usuario, result.Enfileirados, result.PendentesTemp, result.ProfundidadeFilaBroker);

        return Results.Ok(ToEnqueueResponse(result));
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Falha ao enfileirar download (usuário {Usuario})", usuario);
        return Results.Json(
            new
            {
                errors = new[] { FriendlySqlOrConfigError(ex) },
                detail = ex.Message,
                hint = "Veja o log em Logging:File:Directory (padrão CT_e 2.0\\Logs\\resgate-*.log)."
            },
            statusCode: StatusCodes.Status500InternalServerError);
    }
});

api.MapPost("/lotes/upload", async (HttpRequest request, ClaimsPrincipal user, IResgateService svc, CancellationToken ct) =>
{
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
        Log.Warning(ex, "Falha ao ler planilha {File}", file.FileName);
        return Results.BadRequest(new { errors = new[] { $"Falha ao ler planilha: {ex.Message}" } });
    }

    var usuario = user.Identity?.Name ?? "anon";
    try
    {
        var (result, errors) = await svc.EnfileirarDownloadAsync(usuario, lines, ct);
        if (errors.Count > 0 || result is null)
        {
            Log.Warning("Upload enfileirar rejeitado ({Usuario}): {Errors}", usuario, string.Join("; ", errors));
            return Results.BadRequest(new { errors });
        }

        Log.Information("Download (upload) enfileirado por {Usuario}: {Qtd} chaves", usuario, result.Enfileirados);
        return Results.Ok(ToEnqueueResponse(result));
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Falha ao enfileirar via upload (usuário {Usuario})", usuario);
        return Results.Json(
            new
            {
                errors = new[] { FriendlySqlOrConfigError(ex) },
                detail = ex.Message,
                hint = "Veja o log em Logging:File:Directory (padrão CT_e 2.0\\Logs\\resgate-*.log)."
            },
            statusCode: StatusCodes.Status500InternalServerError);
    }
});

api.MapGet("/fila-download", async (IResgateService svc, CancellationToken ct) =>
{
    var dto = await svc.GetFilaStatusAsync(ct);
    return Results.Ok(dto);
});

api.MapPost("/status-chaves", async (StatusChavesRequest body, IResgateService svc, CancellationToken ct) =>
{
    var chaves = body.Chaves ?? [];
    if (chaves.Count == 0)
        return Results.BadRequest(new { errors = new[] { "Informe ao menos uma chave." } });
    var dto = await svc.GetStatusChavesAsync(chaves, ct);
    return Results.Ok(dto);
});

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}

static object ToEnqueueResponse(CargaEnqueueResult result) => new
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
        monitor = "http://localhost:4260"
    },
    riscoFila = "Fila compartilhada com Integrador — preferir janela com Carga ativa."
};

static string FriendlySqlOrConfigError(Exception ex)
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

public partial class Program;

public sealed record LoginRequest(string Usuario, string Senha);
public sealed record CreateLoteRequest(List<string>? Chaves);
public sealed record StatusChavesRequest(List<string>? Chaves);
