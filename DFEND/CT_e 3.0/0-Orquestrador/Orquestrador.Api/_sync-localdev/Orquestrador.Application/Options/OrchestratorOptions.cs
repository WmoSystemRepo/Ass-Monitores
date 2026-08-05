namespace Orquestrador.Application.Options;

public sealed class OrchestratorOptions
{
    public const string SectionName = "Orchestrator";

    /// <summary>Header HTTP enviado aos monitores (serviço-a-serviço).</summary>
    public const string InternalApiKeyHeaderName = "X-Cte-Internal-Api-Key";

    public List<OrchestratorSystemOptions> Systems { get; set; } = [];

    /// <summary>API key compartilhada da cadeia. Obrigatória em Development, Homologacao e Production.</summary>
    public string InternalApiKey { get; set; } = string.Empty;

    public int CascadeDelayMs { get; set; } = 1500;

    public int PollIntervalMs { get; set; } = 1000;

    /// <summary>Timeout por tentativa HTTP aos monitores (segundos).</summary>
    public int HttpTimeoutSeconds { get; set; } = 5;

    /// <summary>Falhas amostradas antes de abrir o circuit breaker.</summary>
    public int CircuitBreakerSamplingDurationSeconds { get; set; } = 30;

    /// <summary>Proporção de falhas (0–1) para abrir o circuit.</summary>
    public double CircuitBreakerFailureRatio { get; set; } = 0.5;

    /// <summary>Mínimo de chamadas no sampling window.</summary>
    public int CircuitBreakerMinimumThroughput { get; set; } = 4;

    /// <summary>Tempo que o circuit permanece aberto (segundos).</summary>
    public int CircuitBreakerBreakDurationSeconds { get; set; } = 15;

    public string ServiceId { get; set; } = "dfend-cte-orchestrator";

    public string DisplayName { get; set; } = "Orquestrador CT-e";

    public string Domain { get; set; } = "orquestrador";

    public string ApiVersion { get; set; } = "1.0";

    /// <summary>Somente Development: sobe Monitor.Api localmente se estiver offline.</summary>
    public LocalDevOptions LocalDev { get; set; } = new();
}

public sealed class LocalDevOptions
{
    /// <summary>No boot do Orquestrador, sobe os Monitor.Api habilitados que estiverem offline.</summary>
    public bool AutoStartMonitors { get; set; }

    /// <summary>No Ligar cadeia, tenta subir o Monitor.Api antes de pular por /health/ready.</summary>
    public bool EnsureBeforeCascade { get; set; } = true;

    /// <summary>Tempo máximo aguardando cada Monitor.Api ficar ready após o launch.</summary>
    public int StartupTimeoutSeconds { get; set; } = 90;

    /// <summary>
    /// Opcional. Raiz do monorepo CT_e (pasta que contém 0-Orquestrador, …).
    /// Preferir deixar vazio: o sistema descobre sozinho a partir de Orquestrador.Api.
    /// Só use se a descoberta automática falhar; não versionar caminho absoluto de máquina.
    /// </summary>
    public string? RepoRoot { get; set; }
}

public sealed class OrchestratorSystemOptions
{
    public string Id { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;

    /// <summary>URL absoluta do monitor (http/https). Injetar por ambiente em produção.</summary>
    public string BaseUrl { get; set; } = string.Empty;

    public bool Enabled { get; set; }

    /// <summary>
    /// Caminho do .csproj do Monitor.Api, relativo à raiz CT_e
    /// (ex.: 1-Receptor/Receptor.Api/src/Monitor.Api/Monitor.Api.csproj).
    /// Absolutos de outra máquina são remapeados automaticamente pelo sufixo estável.
    /// Usado só com Orchestrator:LocalDev em Development.
    /// </summary>
    public string? ProjectPath { get; set; }

    /// <summary>
    /// URL do frontend Angular do sistema (ex.: http://localhost:4200).
    /// Usada pelo dashboard para abrir o monitor ao clicar no estágio.
    /// </summary>
    public string? FrontendUrl { get; set; }

    /// <summary>
    /// Pasta do frontend Nx/Angular (relativa à raiz CT_e). Só DEV — auto-start no clique.
    /// Ex.: 1-Receptor/Frontend
    /// </summary>
    public string? FrontendProjectPath { get; set; }
}
