using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;

namespace CTe.Resgate.Infrastructure.An;

internal static class CteConsultaSoap
{
    private const string NsCte = "http://www.portalfiscal.inf.br/cte";
    private const string NsWsdl = "http://www.portalfiscal.inf.br/cte/wsdl/cteConsultaDFe";
    private const string SoapAction = "http://www.portalfiscal.inf.br/cte/wsdl/cteConsultaDFe/cteConsDFe";

    /// <summary>Status AN que indicam documento retornado (referência Carga).</summary>
    private static readonly HashSet<string> StatusComDocumento = new(StringComparer.Ordinal) { "129", "130", "131" };

    public static string BuildConsultaEnvelope(string chave, short tpAmb)
    {
        var consulta =
            $"""<cteConsultaDFe versao="1.00" xmlns="{NsCte}"><tpAmb>{tpAmb}</tpAmb><xServ>CONSULTAR</xServ><chCTe>{chave}</chCTe></cteConsultaDFe>""";
        return $"""
            <?xml version="1.0" encoding="utf-8"?>
            <soap12:Envelope xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
              <soap12:Header>
                <cteCabecMsg xmlns="{NsWsdl}"><versaoDados>1.00</versaoDados></cteCabecMsg>
              </soap12:Header>
              <soap12:Body>
                <cteDadosMsg xmlns="{NsWsdl}">{consulta}</cteDadosMsg>
              </soap12:Body>
            </soap12:Envelope>
            """;
    }

    public static async Task<SoapConsultaParseResult> PostAsync(
        string url, X509Certificate2 cert, string chave, short tpAmb, int timeoutSeconds, CancellationToken ct)
    {
        var handler = new HttpClientHandler
        {
            ClientCertificates = { cert },
            SslProtocols = SslProtocols.Tls12
        };
        using var client = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(timeoutSeconds) };
        var body = BuildConsultaEnvelope(chave, tpAmb);
        using var content = new StringContent(body, Encoding.UTF8, "application/soap+xml");
        content.Headers.ContentType = new MediaTypeHeaderValue("application/soap+xml") { CharSet = "utf-8" };
        content.Headers.Add("SOAPAction", SoapAction);

        using var response = await client.PostAsync(url, content, ct);
        var responseXml = await response.Content.ReadAsStringAsync(ct);
        if (!response.IsSuccessStatusCode)
            return new SoapConsultaParseResult(false, false, null, "HTTP", $"HTTP {(int)response.StatusCode}", true);

        return ParseResponse(responseXml);
    }

    internal static SoapConsultaParseResult ParseResponse(string responseXml)
    {
        try
        {
            var doc = XDocument.Parse(responseXml);
            var ret = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "retConsDFe")
                        ?? doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "retConsDFe".Replace("Cons", "Cons"));
            ret ??= doc.Descendants().FirstOrDefault(e => e.Name.LocalName.Contains("retCons", StringComparison.OrdinalIgnoreCase));
            if (ret is null)
            {
                var cStatAny = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "cStat")?.Value;
                if (cStatAny is null)
                    return new SoapConsultaParseResult(false, false, null, "PARSE", "Resposta SOAP sem retConsDFe", true);
                ret = doc.Descendants().First(e => e.Name.LocalName == "cStat").Parent!;
            }

            var cStat = ret.Elements().FirstOrDefault(e => e.Name.LocalName == "cStat")?.Value
                        ?? doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "cStat")?.Value ?? "";
            var xMotivo = ret.Elements().FirstOrDefault(e => e.Name.LocalName == "xMotivo")?.Value ?? "";

            if (!StatusComDocumento.Contains(cStat))
            {
                if (cStat is "137" or "138")
                    return new SoapConsultaParseResult(true, false, null, cStat, xMotivo, false);
                return new SoapConsultaParseResult(true, false, null, cStat, xMotivo, false);
            }

            var cteNode = ret.Descendants().FirstOrDefault(e => e.Name.LocalName == "CTe")
                          ?? ret.Descendants().FirstOrDefault(e => e.Name.LocalName.EndsWith("CTe"));
            if (cteNode is null)
                return new SoapConsultaParseResult(true, false, null, cStat, "Sem nó CTe na resposta", false);

            return new SoapConsultaParseResult(true, true, cteNode.ToString(SaveOptions.DisableFormatting), cStat, xMotivo, false);
        }
        catch (Exception ex)
        {
            return new SoapConsultaParseResult(false, false, null, "PARSE", ex.Message, true);
        }
    }
}

internal sealed record SoapConsultaParseResult(
    bool Sucesso, bool Encontrado, string? Xml, string? Codigo, string? Mensagem, bool Retryable);
