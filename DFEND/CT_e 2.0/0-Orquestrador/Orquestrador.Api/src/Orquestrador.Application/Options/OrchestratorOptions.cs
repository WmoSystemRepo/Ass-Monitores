namespace Orquestrador.Application.Options;

public sealed class OrchestratorOptions
{
    public const string SectionName = "Orchestrator";

    /// <summary>Header HTTP enviado aos monitores (serviço-a-serviço).</summary>
    public const string InternalApiKeyHeaderName = "X-Cte-Internal-Api-Key";

    public const string CorrelationIdHeaderName = "X-Correlation-Id";

    public const string IdempotencyKeyHeaderName = "Idempotency-Key";

    /// <summary>Versão do schema do Registry (config). Evolui sem quebrar consumidores.</summary>
    public string RegistrySchemaVersion { get; set; } = "1.0";

    public List<OrchestratorSystemOptions> Systems { get; set; } = [];

    /// <summary>API key compartilhada da cadeia. Obrigatória em Development, Homologacao e Production.</summary>
    public string InternalApiKey { get; set; } = string.Empty;

    public int CascadeDelayMs { get; set; } = 1500;

    public int PollIntervalMs { get; set; } = 2000;

    /// <summary>Timeout por tentativa HTTP aos monitores (segundos) — status/snapshot.</summary>
    public int HttpTimeoutSeconds { get; set; } = 5;

    /// <summary>Timeout health live/ready (segundos).</summary>
    public int HealthTimeoutSeconds { get; set; } = 3;

    /// <summary>Timeout HTTP do POST start/stop (aceite), segundos.</summary>
    public int StartStopHttpTimeoutSeconds { get; set; } = 10;

    /// <summary>Tempo máximo de poll até Running/Stopped após start/stop (segundos).</summary>
    public int PollUntilSettledSeconds { get; set; } = 60;

    /// <summary>Falhas amostradas antes de abrir o circuit breaker.</summary>
    public int CircuitBreakerSamplingDurationSeconds { get; set; } = 30;

    /// <summary>Proporção de falhas (0–1) para abrir o circuit.</summary>
    public double CircuitBreakerFailureRatio { get; set; } = 0.5;

    /// <summary>Mínimo de chamadas no sampling window (padrão plano: 5).</summary>
    public int CircuitBreakerMinimumThroughput { get; set; } = 5;

    /// <summary>Tempo que o circuit permanece aberto / half-open (segundos).</summary>
    public int CircuitBreakerBreakDurationSeconds { get; set; } = 30;

    public string ServiceId { get; set; } = "dfend-cte-orchestrator";

    public string DisplayName { get; set; } = "Orquestrador CT-e";

    public string Domain { get; set; } = "orquestrador";

    public string ApiVersion { get; set; } = "1.0";

    /// <summary>Somente Development sem Docker: sobe Monitor.Api localmente se estiver offline.</summary>
    public LocalDevOptions LocalDev { get; set; } = new();
}

public sealed class LocalDevOptions
{
    /// <summary>No boot do Orquestrador, sobe os Monitor.Api habilitados que estiverem offline.</summary>
    public bool AutoStartMonitors { get; set; }

    /// <summary>No Ligar cadeia, tenta subir o Monitor.Api/Angular via spawn local se offline.</summary>
    public bool EnsureBeforeCascade { get; set; } = true;

    /// <summary>Tempo máximo aguardando cada Monitor.Api ficar ready após o launch.</summary>
    public int StartupTimeoutSeconds { get; set; } = 90;

    /// <summary>
    /// Timeout legado/alias; EnsureFrontend usa StartupTimeoutSeconds (mín. 60).
    /// Mantido para config existente em appsettings.
    /// </summary>
    public int FrontendEnsureTimeoutSeconds { get; set; } = 180;

    /// <summary>
    /// Opcional. Raiz do monorepo CT_e (pasta que contém 0-Orquestrador, …).
    /// Preferir deixar vazio: o sistema descobre sozinho a partir de Orquestrador.Api.
    /// </summary>
    public string? RepoRoot { get; set; }

    /// <summary>
    /// Após API ready no auto-start, abre Swagger UI no browser (confirmação visual LocalDev).
    /// </summary>
    public bool OpenSwaggerOnReady { get; set; } = true;
}

public sealed class OrchestratorSystemOptions
{
    public string Id { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;

    /// <summary>Versão do monitor no registry (contrato/imagem).</summary>
    public string Version { get; set; } = "1.0";

    /// <summary>Ordem na cascata (menor primeiro). Se 0, usa posição na lista.</summary>
    public int Order { get; set; }

    /// <summary>Ids de monitores que devem estar Running antes deste (Ligar).</summary>
    public List<string> DependsOn { get; set; } = [];

    /// <summary>URL absoluta do monitor (http/https). Compatível com Endpoints.Api.</summary>
    public string BaseUrl { get; set; } = string.Empty;

    public bool Enabled { get; set; }

    /// <summary>
    /// Se false: sobe API no LocalDev (AutoStart), mas não entra no fluxograma nem no Ligar/Desligar R→C.
    /// Usado pelo Resgate CT-e (API :5070, UI no menu Orquestrador).
    /// </summary>
    public bool InCascade { get; set; } = true;

    /// <summary>Caminho do .csproj do Monitor.Api (só DEV local sem Docker).</summary>
    public string? ProjectPath { get; set; }

    /// <summary>URL do frontend Angular (compatível com Endpoints.Frontend).</summary>
    public string? FrontendUrl { get; set; }

    /// <summary>Pasta do frontend Nx (só DEV local sem Docker).</summary>
    public string? FrontendProjectPath { get; set; }

    public MonitorEndpointOptions Endpoints { get; set; } = new();

    public MonitorUiOptions Ui { get; set; } = new();

    public MonitorInstancesOptions Instances { get; set; } = new();

    public string ResolveApiBase() =>
        !string.IsNullOrWhiteSpace(Endpoints?.Api) ? Endpoints.Api!.TrimEnd('/') : BaseUrl.TrimEnd('/');

    public string ResolveFrontendUrl() =>
        !string.IsNullOrWhiteSpace(Endpoints?.Frontend)
            ? Endpoints.Frontend!
            : FrontendUrl ?? string.Empty;

    /// <summary>URL da Swagger UI (Endpoints.Swagger ou {Api}/swagger).</summary>
    public string ResolveSwaggerUrl()
    {
        var configured = Endpoints?.Swagger?.Trim().TrimEnd('/');
        if (!string.IsNullOrWhiteSpace(configured))
        {
            return configured.Contains("/swagger", StringComparison.OrdinalIgnoreCase)
                ? configured
                : configured + "/swagger";
        }

        var api = ResolveApiBase();
        return string.IsNullOrWhiteSpace(api) ? string.Empty : api + "/swagger";
    }

    public string HealthLivePath() =>
        string.IsNullOrWhiteSpace(Endpoints?.HealthLive) ? "/health" : Endpoints!.HealthLive!;

    public string HealthReadyPath() =>
        string.IsNullOrWhiteSpace(Endpoints?.HealthReady) ? "/health/ready" : Endpoints!.HealthReady!;

    public string StatusPath() =>
        string.IsNullOrWhiteSpace(Endpoints?.Status)
            ? "/api/monitor/service/status"
            : Endpoints!.Status!;

    public string StartPath() =>
        string.IsNullOrWhiteSpace(Endpoints?.Start)
            ? "/api/monitor/service/start"
            : Endpoints!.Start!;

    public string StopPath() =>
        string.IsNullOrWhiteSpace(Endpoints?.Stop)
            ? "/api/monitor/service/stop"
            : Endpoints!.Stop!;

    public string MetricsPath() =>
        string.IsNullOrWhiteSpace(Endpoints?.Metrics)
            ? "/api/monitor/snapshot"
            : Endpoints!.Metrics!;
}

public sealed class MonitorEndpointOptions
{
    public string? Api { get; set; }
    public string? Frontend { get; set; }
    public string? Swagger { get; set; }
    public string? HealthLive { get; set; }
    public string? HealthReady { get; set; }
    public string? Status { get; set; }
    public string? Start { get; set; }
    public string? Stop { get; set; }
    public string? Metrics { get; set; }
}

public sealed class MonitorUiOptions
{
    public string? Icon { get; set; }
    public string? Color { get; set; }
}

public sealed class MonitorInstancesOptions
{
    /// <summary>active | all | roundRobin | byEnvironment — MVP: active.</summary>
    public string Mode { get; set; } = "active";

    public List<string> Targets { get; set; } = [];
}
