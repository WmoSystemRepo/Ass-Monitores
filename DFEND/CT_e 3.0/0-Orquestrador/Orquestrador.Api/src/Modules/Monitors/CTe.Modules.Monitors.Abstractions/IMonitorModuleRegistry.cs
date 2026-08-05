namespace CTe.Modules.Monitors.Abstractions;

/// <summary>Resolve o <see cref="IMonitorModule"/> de um serviço pelo id (ver rotas unificadas).</summary>
public interface IMonitorModuleRegistry
{
    IMonitorModule? Get(string serviceId);

    IReadOnlyList<string> ServiceIds { get; }
}
