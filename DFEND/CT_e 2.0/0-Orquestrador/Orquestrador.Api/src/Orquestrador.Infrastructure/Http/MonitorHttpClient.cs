using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orquestrador.Application.Abstractions;
using Orquestrador.Application.Options;

namespace Orquestrador.Infrastructure.Http;

public sealed class MonitorHttpClient : IMonitorClient
{
    public const string HttpClientName = "MonitorClient";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<OrchestratorOptions> _options;
    private readonly ILogger<MonitorHttpClient> _logger;
    private readonly ConcurrentDictionary<string, byte> _offlineLogged = new(StringComparer.OrdinalIgnoreCase);

    public MonitorHttpClient(
        IHttpClientFactory httpClientFactory,
        IOptions<OrchestratorOptions> options,
        ILogger<MonitorHttpClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _options = options;
        _logger = logger;
    }

    public async Task<bool> PingReadyAsync(OrchestratorSystemOptions system, CancellationToken ct)
    {
        var baseUrl = system.ResolveApiBase();
        try
        {
            var client = CreateClient();
            using var response = await client.GetAsync(Combine(baseUrl, system.HealthReadyPath()), ct);
            if (response.IsSuccessStatusCode)
            {
                MarkOnline(baseUrl);
                return true;
            }

            LogOfflineOnce(baseUrl, $"HTTP {(int)response.StatusCode}");
            return false;
        }
        catch (Exception ex) when (IsConnectivityFailure(ex))
        {
            LogOfflineOnce(baseUrl, ex.GetType().Name);
            return false;
        }
        catch (Exception ex)
        {
            LogOfflineOnce(baseUrl, ex.Message);
            return false;
        }
    }

    public async Task<MonitorFetchResult<JsonDocument>> GetSnapshotAsync(
        OrchestratorSystemOptions system,
        CancellationToken ct)
    {
        var baseUrl = system.ResolveApiBase();
        try
        {
            var client = CreateClient();
            using var response = await client.GetAsync(Combine(baseUrl, system.MetricsPath()), ct);
            if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            {
                return MonitorFetchResult<JsonDocument>.Fail(
                    "unauthorized",
                    "Autenticação interna rejeitada (API key).",
                    (int)response.StatusCode);
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Snapshot {BaseUrl} → {Status}", baseUrl, (int)response.StatusCode);
                return MonitorFetchResult<JsonDocument>.Fail(
                    "error",
                    $"Snapshot HTTP {(int)response.StatusCode}",
                    (int)response.StatusCode);
            }

            await using var stream = await response.Content.ReadAsStreamAsync(ct);
            var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
            MarkOnline(baseUrl);
            return MonitorFetchResult<JsonDocument>.Ok(doc);
        }
        catch (Exception ex) when (IsConnectivityFailure(ex))
        {
            LogOfflineOnce(baseUrl, ex.GetType().Name);
            return MonitorFetchResult<JsonDocument>.Fail("offline", "Monitor indisponível.", null);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "GetSnapshot falhou para {BaseUrl}", baseUrl);
            return MonitorFetchResult<JsonDocument>.Fail("error", ex.Message, null);
        }
    }

    public async Task<MonitorFetchResult<MonitorServiceStatusDto>> GetStatusAsync(
        OrchestratorSystemOptions system,
        CancellationToken ct)
    {
        var baseUrl = system.ResolveApiBase();
        try
        {
            var client = CreateClient();
            using var response = await client.GetAsync(Combine(baseUrl, system.StatusPath()), ct);
            if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            {
                return MonitorFetchResult<MonitorServiceStatusDto>.Fail(
                    "unauthorized",
                    "Autenticação interna rejeitada (API key).",
                    (int)response.StatusCode);
            }

            if (!response.IsSuccessStatusCode)
            {
                return MonitorFetchResult<MonitorServiceStatusDto>.Ok(
                    new MonitorServiceStatusDto(
                        false,
                        response.StatusCode.ToString(),
                        "Falha ao obter status.",
                        null,
                        null,
                        null));
            }

            await using var stream = await response.Content.ReadAsStreamAsync(ct);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
            var root = doc.RootElement;

            var dto = new MonitorServiceStatusDto(
                Success: TryGetBool(root, "success") ?? true,
                Status: TryGetString(root, "status") ?? TryGetString(root, "scmStatus"),
                Message: TryGetString(root, "message"),
                IsRunning: TryGetBool(root, "isRunning"),
                ScmStatus: TryGetString(root, "scmStatus") ?? TryGetString(root, "status"),
                Executar: TryGetInt(root, "executar"),
                CommandId: TryGetString(root, "commandId"));

            MarkOnline(baseUrl);
            return MonitorFetchResult<MonitorServiceStatusDto>.Ok(dto);
        }
        catch (Exception ex) when (IsConnectivityFailure(ex))
        {
            LogOfflineOnce(baseUrl, ex.GetType().Name);
            return MonitorFetchResult<MonitorServiceStatusDto>.Fail("offline", "Monitor indisponível.", null);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "GetStatus falhou para {BaseUrl}", baseUrl);
            return MonitorFetchResult<MonitorServiceStatusDto>.Fail("error", ex.Message, null);
        }
    }

    public Task<MonitorActionResult> StartAsync(OrchestratorSystemOptions system, CancellationToken ct) =>
        PostActionAsync(system, system.StartPath(), ct);

    public Task<MonitorActionResult> StopAsync(OrchestratorSystemOptions system, CancellationToken ct) =>
        PostActionAsync(system, system.StopPath(), ct);

    private async Task<MonitorActionResult> PostActionAsync(
        OrchestratorSystemOptions system,
        string path,
        CancellationToken ct)
    {
        var baseUrl = system.ResolveApiBase();
        try
        {
            var client = CreateClient();
            var idempotencyKey = Guid.NewGuid().ToString("N");
            using var request = new HttpRequestMessage(HttpMethod.Post, Combine(baseUrl, path));
            request.Headers.TryAddWithoutValidation(
                OrchestratorOptions.IdempotencyKeyHeaderName,
                idempotencyKey);
            request.Headers.TryAddWithoutValidation(
                OrchestratorOptions.CorrelationIdHeaderName,
                idempotencyKey);

            using var response = await client.SendAsync(request, ct);
            var body = await response.Content.ReadAsStringAsync(ct);

            if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            {
                return new MonitorActionResult(false, "unauthorized", "Autenticação interna rejeitada (API key).");
            }

            string? status = null;
            string? message = null;
            string? commandId = null;
            bool? success = null;

            if (!string.IsNullOrWhiteSpace(body))
            {
                try
                {
                    using var doc = JsonDocument.Parse(body);
                    var root = doc.RootElement;
                    success = TryGetBool(root, "success");
                    status = TryGetString(root, "status");
                    message = TryGetString(root, "message");
                    commandId = TryGetString(root, "commandId");
                }
                catch
                {
                    message = body;
                }
            }

            var ok = response.IsSuccessStatusCode && (success ?? true);
            return new MonitorActionResult(
                ok,
                status,
                message ?? (ok ? null : $"HTTP {(int)response.StatusCode}"),
                commandId ?? idempotencyKey);
        }
        catch (Exception ex) when (IsConnectivityFailure(ex))
        {
            LogOfflineOnce(baseUrl, ex.GetType().Name);
            return new MonitorActionResult(false, "offline", "Monitor indisponível.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Ação {Path} falhou para MonitorId={MonitorId} BaseUrl={BaseUrl}",
                path,
                system.Id,
                baseUrl);
            return new MonitorActionResult(false, null, ex.Message);
        }
    }

    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient(HttpClientName);
        var key = _options.Value.InternalApiKey;
        client.DefaultRequestHeaders.Remove(OrchestratorOptions.InternalApiKeyHeaderName);
        if (!string.IsNullOrWhiteSpace(key))
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation(
                OrchestratorOptions.InternalApiKeyHeaderName,
                key);
        }

        return client;
    }

    private void LogOfflineOnce(string baseUrl, string detail)
    {
        if (_offlineLogged.TryAdd(baseUrl, 0))
        {
            _logger.LogWarning(
                "Monitor offline em {BaseUrl} ({Detail}). Novas falhas de conectividade serão silenciadas até recuperar.",
                baseUrl,
                detail);
        }
        else
        {
            _logger.LogDebug("Monitor ainda offline em {BaseUrl} ({Detail})", baseUrl, detail);
        }
    }

    private void MarkOnline(string baseUrl)
    {
        if (_offlineLogged.TryRemove(baseUrl, out _))
        {
            _logger.LogInformation("Monitor voltou a responder em {BaseUrl}", baseUrl);
        }
    }

    private static bool IsConnectivityFailure(Exception ex) =>
        ex is HttpRequestException
            or TaskCanceledException
            or TimeoutException
            or OperationCanceledException { CancellationToken.IsCancellationRequested: false }
        || ex.InnerException is HttpRequestException or SocketException;

    private static string Combine(string baseUrl, string path)
    {
        var root = baseUrl.TrimEnd('/');
        return root + (path.StartsWith('/') ? path : "/" + path);
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

            return prop.Value.ValueKind == JsonValueKind.String
                ? prop.Value.GetString()
                : prop.Value.ToString();
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

    private static int? TryGetInt(JsonElement root, string name)
    {
        foreach (var prop in root.EnumerateObject())
        {
            if (!prop.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (prop.Value.ValueKind == JsonValueKind.Number && prop.Value.TryGetInt32(out var n))
            {
                return n;
            }

            if (prop.Value.ValueKind == JsonValueKind.String && int.TryParse(prop.Value.GetString(), out var parsed))
            {
                return parsed;
            }
        }

        return null;
    }
}
