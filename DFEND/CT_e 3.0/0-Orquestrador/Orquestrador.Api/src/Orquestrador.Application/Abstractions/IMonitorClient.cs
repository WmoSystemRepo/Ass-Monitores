using System.Text.Json;
using Orquestrador.Application.Options;

namespace Orquestrador.Application.Abstractions;

public interface IMonitorClient
{
    Task<bool> PingReadyAsync(OrchestratorSystemOptions system, CancellationToken ct);

    Task<MonitorFetchResult<JsonDocument>> GetSnapshotAsync(OrchestratorSystemOptions system, CancellationToken ct);

    Task<MonitorFetchResult<MonitorServiceStatusDto>> GetStatusAsync(OrchestratorSystemOptions system, CancellationToken ct);

    Task<MonitorActionResult> StartAsync(OrchestratorSystemOptions system, CancellationToken ct);

    Task<MonitorActionResult> StopAsync(OrchestratorSystemOptions system, CancellationToken ct);
}

/// <summary>Kind: offline | unauthorized | error (null quando ok).</summary>
public sealed record MonitorFetchResult<T>(
    T? Value,
    string? ErrorKind,
    string? Message,
    int? StatusCode)
{
    public static MonitorFetchResult<T> Ok(T value) => new(value, null, null, 200);

    public static MonitorFetchResult<T> Fail(string kind, string message, int? statusCode = null) =>
        new(default, kind, message, statusCode);
}

public sealed record MonitorServiceStatusDto(
    bool Success,
    string? Status,
    string? Message,
    bool? IsRunning,
    string? ScmStatus,
    int? Executar,
    string? CommandId = null);

public sealed record MonitorActionResult(
    bool Success,
    string? Status,
    string? Message,
    string? CommandId = null);
