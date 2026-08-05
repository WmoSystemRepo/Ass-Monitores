using System.Net.Sockets;
using System.Text.Json;
using CTe.Modules.Monitors.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orquestrador.Application.Abstractions;
using Orquestrador.Application.Options;
using Orquestrador.Infrastructure.Http;

namespace CTe.Modules.Monitors.Infrastructure;

/// <summary>
/// Implementação HTTP de <see cref="IMonitorModule"/>: fala com o Monitor.Api do serviço usando
/// o contrato fixo (CONTRATO_MICROSERVICO_MONITOR.md — idêntico nos 6 monitores) e reaproveita
/// <see cref="IMonitorClient"/> (snapshot/status/start/stop) e o HttpClient resiliente já
/// configurado em Orquestrador.Infrastructure para info/logs/tables/health.
/// <para>
/// W1: HTTP bridge; usado também para o Receptor. Registro in-process do Receptor
/// (Monitor.Application/Infrastructure/Domain dele) foi adiado para W3 — aquele projeto referencia
/// SignalR + Windows ServiceController + pacotes Microsoft.Extensions.* em versão diferente da
/// pinada em Directory.Build.props, risco de conflito de build fora do escopo desta wave.
/// </para>
/// </summary>
public sealed class HttpMonitorModule : IMonitorModule
{
    private readonly IMonitorClient _monitorClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<OrchestratorOptions> _options;
    private readonly ILogger<HttpMonitorModule> _logger;

    public HttpMonitorModule(
        string serviceId,
        IMonitorClient monitorClient,
        IHttpClientFactory httpClientFactory,
        IOptions<OrchestratorOptions> options,
        ILogger<HttpMonitorModule> logger)
    {
        ServiceId = serviceId;
        _monitorClient = monitorClient;
        _httpClientFactory = httpClientFactory;
        _options = options;
        _logger = logger;
    }

    public string ServiceId { get; }

    public Task<object?> GetInfoAsync(CancellationToken ct) =>
        GetJsonAsync("/api/monitor/info", ct);

    public async Task<object?> GetSnapshotAsync(CancellationToken ct)
    {
        var system = ResolveSystem();
        if (system is null)
        {
            return null;
        }

        var result = await _monitorClient.GetSnapshotAsync(system, ct);
        using var doc = result.Value;
        return doc?.RootElement.Clone();
    }

    public Task<object?> GetLogsAsync(long afterSeq, int take, CancellationToken ct) =>
        GetJsonAsync($"/api/monitor/logs?afterSeq={afterSeq}&take={take}", ct);

    public Task<object?> GetTableAsync(string key, int take, CancellationToken ct) =>
        GetJsonAsync($"/api/monitor/tables/{Uri.EscapeDataString(key)}?take={take}", ct);

    public async Task<object?> GetServiceStatusAsync(CancellationToken ct)
    {
        var system = ResolveSystem();
        if (system is null)
        {
            return null;
        }

        var result = await _monitorClient.GetStatusAsync(system, ct);
        return result.ErrorKind is null ? result.Value : null;
    }

    public async Task<object?> StartAsync(CancellationToken ct)
    {
        var system = ResolveSystem();
        if (system is null)
        {
            return null;
        }

        return await _monitorClient.StartAsync(system, ct);
    }

    public async Task<object?> StopAsync(CancellationToken ct)
    {
        var system = ResolveSystem();
        if (system is null)
        {
            return null;
        }

        return await _monitorClient.StopAsync(system, ct);
    }

    public Task<object?> GetHealthAsync(CancellationToken ct)
    {
        var system = ResolveSystem();
        return system is null
            ? Task.FromResult<object?>(null)
            : GetJsonAsync(system.HealthLivePath(), ct);
    }

    private OrchestratorSystemOptions? ResolveSystem() =>
        _options.Value.Systems.FirstOrDefault(s => s.Id.Equals(ServiceId, StringComparison.OrdinalIgnoreCase));

    private async Task<object?> GetJsonAsync(string relativePath, CancellationToken ct)
    {
        var system = ResolveSystem();
        if (system is null)
        {
            return null;
        }

        var baseUrl = system.ResolveApiBase();
        try
        {
            var client = CreateClient();
            using var response = await client.GetAsync(Combine(baseUrl, relativePath), ct);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            await using var stream = await response.Content.ReadAsStreamAsync(ct);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
            return doc.RootElement.Clone();
        }
        catch (Exception ex) when (IsConnectivityFailure(ex))
        {
            _logger.LogDebug(
                ex,
                "Monitor {ServiceId} indisponível em {BaseUrl}{Path}",
                ServiceId,
                baseUrl,
                relativePath);
            return null;
        }
    }

    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient(MonitorHttpClient.HttpClientName);
        var key = _options.Value.InternalApiKey;
        client.DefaultRequestHeaders.Remove(OrchestratorOptions.InternalApiKeyHeaderName);
        if (!string.IsNullOrWhiteSpace(key))
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation(OrchestratorOptions.InternalApiKeyHeaderName, key);
        }

        return client;
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
}
