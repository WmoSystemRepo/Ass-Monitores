using System.Security.Cryptography.X509Certificates;
using CTe.Resgate.Application.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CTe.Resgate.Infrastructure.An;

public sealed class AnOptions
{
    public const string SectionName = "AmbienteNacional";
    public string Url { get; set; } = "";
    /// <summary>Fragmento do Subject no store Windows (ex.: BAHIA SECRETARIA DA FAZENDA).</summary>
    public string CertificadoSubject { get; set; } = "";
    public string CertificadoPath { get; set; } = "";
    public string CertificadoSenha { get; set; } = "";
    public short TpAmb { get; set; } = 1;
    /// <summary>Alias de config legada (appsettings TipoAmbiente).</summary>
    public short TipoAmbiente { get => TpAmb; set => TpAmb = value; }
    public int TimeoutSeconds { get; set; } = 60;
    /// <summary>Development legado — simulação AN removida; mantido só para compatibilidade de config.</summary>
    public bool AllowSimulatedWhenUnconfigured { get; set; } = false;
}

/// <summary>
/// Cliente de consulta por chave via SOAP cteConsDFe (referência Carga ProcessarDownload).
/// </summary>
public sealed class AnConsultaClient(
    IOptions<AnOptions> options,
    ILogger<AnConsultaClient> logger) : IAnConsultaClient
{
    public async Task<AnConsultaResult> ConsultarPorChaveAsync(string chave, CancellationToken ct)
    {
        var opt = options.Value;
        if (string.IsNullOrWhiteSpace(opt.Url) || string.IsNullOrWhiteSpace(opt.CertificadoSubject))
            return new AnConsultaResult(false, false, null, "CFG", "AN não configurado (Url + CertificadoSubject)", false);

        try
        {
            using var cert = LoadCertFromWindowsStore(opt.CertificadoSubject);
            if (cert is null)
                return new AnConsultaResult(false, false, null, "CERT", $"Certificado não encontrado: {opt.CertificadoSubject}", false);

            if (DateTime.Now > cert.NotAfter)
                return new AnConsultaResult(false, false, null, "CERT", "Certificado expirado", false);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(opt.TimeoutSeconds));

            logger.LogInformation("Consulta AN SOAP url={Url} chave={Chave}", opt.Url, Mask(chave));
            var parsed = await CteConsultaSoap.PostAsync(opt.Url, cert, chave, opt.TpAmb, opt.TimeoutSeconds, cts.Token);
            return new AnConsultaResult(parsed.Sucesso, parsed.Encontrado, parsed.Xml, parsed.Codigo, parsed.Mensagem, parsed.Retryable);
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
        {
            return new AnConsultaResult(false, false, null, "TIMEOUT", "Timeout SOAP", true);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Falha consulta AN chave={Chave}", Mask(chave));
            return new AnConsultaResult(false, false, null, "ERR", "Falha comunicação AN", true);
        }
    }

    private static X509Certificate2? LoadCertFromWindowsStore(string subjectFragment)
        => CertificadoWindowsLoader.FindBySubjectContains(subjectFragment);

    private static string Mask(string chave)
        => chave.Length == 44 ? $"{chave[..6]}****{chave[^6..]}" : "****";
}
