using System.Reflection;
using CTe.BuildingBlocks.Auth;
using CTe.Modules.Monitors.Abstractions;
using NetArchTest.Rules;

namespace CTe.Architecture.Tests;

/// <summary>
/// Regras de arquitetura (Wave 0): BuildingBlocks é a base compartilhada da cadeia CT-e e não
/// pode acoplar em Orquestrador.* nem Monitor.* (evita ciclos e vazamento de detalhes internos).
/// </summary>
public class BuildingBlocksArchitectureTests
{
    private static readonly Assembly BuildingBlocksAssembly = typeof(MonitorAuthPolicies).Assembly;

    private static readonly Assembly MonitorsAbstractionsAssembly = typeof(IMonitorModule).Assembly;

    // Busca por "Orquestrador." / "Monitor." (com ponto) — namespace real, não substring do nome
    // da própria classe (ex.: MonitorAuthPolicies contém "Monitor" mas não depende do domínio).
    [Fact]
    public void BuildingBlocks_Should_Not_Depend_On_Orquestrador()
    {
        var result = Types.InAssembly(BuildingBlocksAssembly)
            .ShouldNot()
            .HaveDependencyOn("Orquestrador.")
            .GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    [Fact]
    public void BuildingBlocks_Should_Not_Depend_On_Monitor()
    {
        var result = Types.InAssembly(BuildingBlocksAssembly)
            .ShouldNot()
            .HaveDependencyOn("Monitor.")
            .GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    [Fact]
    public void MonitorsAbstractions_Should_Not_Depend_On_Orquestrador_Or_Monitor()
    {
        var result = Types.InAssembly(MonitorsAbstractionsAssembly)
            .ShouldNot()
            .HaveDependencyOnAny("Orquestrador.", "Monitor.Api", "Monitor.Application", "Monitor.Domain", "Monitor.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    /// <summary>Smoke test: os assemblies compartilhados carregam e expõem o contrato esperado.</summary>
    [Fact]
    public void Smoke_BuildingBlocks_Exposes_Auth_Policies()
    {
        Assert.Equal("Monitor.Read", MonitorAuthPolicies.MonitorRead);
        Assert.Equal("Monitor.Control", MonitorAuthPolicies.MonitorControl);
    }

    private static string Describe(TestResult result) =>
        result.FailingTypeNames is null
            ? "sem detalhes"
            : string.Join(", ", result.FailingTypeNames);
}
