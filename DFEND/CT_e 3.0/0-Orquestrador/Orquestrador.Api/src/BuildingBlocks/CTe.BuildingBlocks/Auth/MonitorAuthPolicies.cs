namespace CTe.BuildingBlocks.Auth;

/// <summary>
/// Nomes de política de autorização compartilhados entre Orquestrador e os monitores unificados
/// (SDD Monitor Unificado). Mantido em BuildingBlocks para evitar duplicação/typos entre projetos.
/// </summary>
public static class MonitorAuthPolicies
{
    /// <summary>Leitura de telemetria/estado de um monitor (info, snapshot, logs, tabelas, status, health).</summary>
    public const string MonitorRead = "Monitor.Read";

    /// <summary>Controle operacional de um monitor (start/stop).</summary>
    public const string MonitorControl = "Monitor.Control";

    /// <summary>Tipo de claim usado pelos esquemas de autenticação para carregar os escopos concedidos.</summary>
    public const string ScopeClaimType = "cte:monitor-scope";

    public const string ScopeRead = "read";

    public const string ScopeControl = "control";
}
