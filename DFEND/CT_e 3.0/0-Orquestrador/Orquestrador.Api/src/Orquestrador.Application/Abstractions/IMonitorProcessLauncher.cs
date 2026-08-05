using Orquestrador.Application.Options;

namespace Orquestrador.Application.Abstractions;

/// <summary>
/// Sobe o processo Monitor.Api (e opcionalmente o front) em Development quando ainda não respondem.
/// Não inicia o worker (DevHost/SCM) — isso continua sendo POST /api/monitor/service/start.
/// </summary>
public interface IMonitorProcessLauncher
{
    Task<bool> IsApiReadyAsync(string baseUrl, CancellationToken ct);

    Task<bool> IsFrontendReachableAsync(string frontendUrl, CancellationToken ct);

    /// <summary>
    /// Garante que o Monitor.Api do sistema responda /health/ready.
    /// Se já estiver online, retorna true sem spawn.
    /// </summary>
    Task<bool> EnsureApiReadyAsync(OrchestratorSystemOptions system, CancellationToken ct);

    /// <summary>
    /// Garante que o frontend Angular responda (DEV: sobe npm start se FrontendProjectPath existir).
    /// </summary>
    Task<bool> EnsureFrontendAsync(OrchestratorSystemOptions system, CancellationToken ct);

    /// <summary>
    /// Encerra processos API/Angular spawnados por este launcher (libera DLLs no disco).
    /// Chamar ao parar o Orquestrador ou antes de rebuild/F5 nos Monitor.Api.
    /// </summary>
    void KillLaunchedChildren();

    /// <summary>
    /// Compila tools/*.DevHost faltantes (Debug) na raiz CT_e — necessário para Ligar cadeia
    /// (Sintetizador/Analisador etc. sem .exe pré-compilado).
    /// </summary>
    Task<(bool Success, string Message)> EnsureDevHostsBuiltAsync(CancellationToken ct);
}
