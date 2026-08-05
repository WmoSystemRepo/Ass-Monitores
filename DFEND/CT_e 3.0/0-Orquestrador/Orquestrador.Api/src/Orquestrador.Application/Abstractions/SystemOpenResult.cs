namespace Orquestrador.Application.Abstractions;

public sealed record SystemOpenResult(
    bool Success,
    string SystemId,
    string? FrontendUrl,
    bool ApiReady,
    bool ApiStarted,
    bool FrontendReachable,
    bool FrontendStarted,
    string? Message);
