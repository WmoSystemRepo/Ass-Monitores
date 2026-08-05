namespace CTe.Modules.Monitors.WindowsControl;

/// <summary>
/// Opções de controle Windows/DevHost de um monitor unificado, ligadas em
/// <c>Monitors:{serviceId}</c> no appsettings do Orquestrador (W3 — SDD Monitor Unificado).
/// Espelha os campos hoje espalhados em cada Monitor.Api (Monitor:WindowsServiceName,
/// Monitor:{X}RootPath, Monitor:{X}ExeRelativePath, Monitor:{X}ProcessName, Monitor:CodServico{X}).
/// </summary>
public sealed class MonitorControlOptions
{
    public string ServiceId { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    /// <summary>Domínio/pasta curta (receptor, arquivador, sintetizador, analisador, integrador, carga).</summary>
    public string Domain { get; set; } = string.Empty;

    /// <summary>Nome do Windows Service monitorado (ex.: DFEND_CTe_Receptor).</summary>
    public string MonitoredService { get; set; } = string.Empty;

    /// <summary>Nome do serviço no SCM (Service Control Manager) — geralmente igual a MonitoredService.</summary>
    public string WindowsServiceName { get; set; } = string.Empty;

    /// <summary>
    /// POC LocalDev: não consulta o SCM, controla o processo DevHost (.exe) diretamente.
    /// </summary>
    public bool PreferLocalProcess { get; set; } = true;

    /// <summary>Connection string do banco monitorado (ping de saúde). Vazio = sem checagem SQL.</summary>
    public string? ConnectionString { get; set; }

    public int SqlTimeoutSeconds { get; set; } = 3;

    /// <summary>CodServico{X} do serviço no banco (ex.: CodServicoReceptor=2).</summary>
    public int CodServico { get; set; }

    /// <summary>
    /// Raiz explícita do pacote. Evite path absoluto de outra máquina — deixe vazio e use PackageFolder.
    /// </summary>
    public string? RootPath { get; set; }

    /// <summary>
    /// Pasta do engine relativa à raiz do 0-Orquestrador (ex.: "engines\receptor").
    /// Também aceita legado "0-Orquestrador\engines\receptor" ou "1-Receptor".
    /// </summary>
    public string PackageFolder { get; set; } = string.Empty;

    /// <summary>Caminho do .exe DevHost relativo à raiz do pacote (Monitor:{X}ExeRelativePath).</summary>
    public string? ExeRelativePath { get; set; }

    /// <summary>Nome do processo DevHost (ex.: Receptor.DevHost) usado para Process.GetProcessesByName.</summary>
    public string? ProcessName { get; set; }

    /// <summary>
    /// W3 fallback opcional: quando true, snapshot/logs/tables usam o Monitor.Api HTTP do serviço
    /// (comportamento W1) em vez do payload estruturado in-process. Default false — a rota unificada
    /// funciona sem o Monitor.Api de pé.
    /// </summary>
    public bool UseHttpFallback { get; set; }
}
