namespace CTe.Resgate.Infrastructure.Options;

public sealed class ResgateOptions
{
    public const string SectionName = "Resgate";

    public string ConnectionString { get; set; } = "";

    /// <summary>Banco sintético para documento_conhecimento_transporte_eletronico_autorizacao.</summary>
    public string ConnectionStringSintetico { get; set; } = "";

    public int SqlTimeoutSeconds { get; set; } = 30;
}
