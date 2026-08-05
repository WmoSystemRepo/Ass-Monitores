using System.Text;
using System.Xml.Linq;
using CTe.Resgate.Application.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CTe.Resgate.Infrastructure.Persistence;

public sealed class SqlDocumentoRepository(IConfiguration configuration, ILogger<SqlDocumentoRepository> logger) : IDocumentoRepository
{
    private const string Table = "cte.documento_conhecimento_transporte_eletronico_autorizacao";

    private string ConnectionString =>
        configuration.GetConnectionString("BDCTeSintetico")
        ?? throw new InvalidOperationException("ConnectionStrings:BDCTeSintetico não configurada.");

    public async Task<bool> ExistsAsync(string chave, CancellationToken ct)
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(
            $"SELECT 1 FROM {Table} WHERE cod_chave_acesso = @chave", conn);
        cmd.Parameters.AddWithValue("@chave", chave);
        var result = await cmd.ExecuteScalarAsync(ct);
        return result is not null;
    }

    public async Task InsertIfAbsentAsync(string chave, string xml, string? protocolo, string? nsu, CancellationToken ct)
    {
        if (await ExistsAsync(chave, ct))
            return;

        var dtr = DataReferenciaFromChave(chave);
        var (prot, dhRecbto) = ParseProtocolo(xml, protocolo);
        var nsuVal = long.TryParse(nsu, out var n) ? n : 0L;

        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(
            $"""
            INSERT INTO {Table} (
                dtr_referencia, cod_chave_acesso, num_protocolo, num_sequencial_unico,
                xml_documento, dtc_documento, dtc_insercao, dtc_atualizacao
            ) VALUES (
                @dtr, @chave, @prot, @nsu,
                CONVERT(VARBINARY(MAX), @xml), @dtc, GETDATE(), GETDATE()
            )
            """, conn);
        cmd.Parameters.AddWithValue("@dtr", dtr);
        cmd.Parameters.AddWithValue("@chave", chave);
        cmd.Parameters.AddWithValue("@prot", prot);
        cmd.Parameters.AddWithValue("@nsu", nsuVal);
        cmd.Parameters.AddWithValue("@xml", xml);
        cmd.Parameters.AddWithValue("@dtc", dhRecbto);

        try
        {
            await cmd.ExecuteNonQueryAsync(ct);
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            logger.LogInformation("Documento já existia (PK) chave={Chave}", Mask(chave));
        }
    }

    internal static int DataReferenciaFromChave(string chave)
        => int.Parse("20" + chave.Substring(2, 2) + chave.Substring(4, 2));

    private static (long Protocolo, DateTime DhRecbto) ParseProtocolo(string xml, string? fallbackProt)
    {
        try
        {
            var doc = XDocument.Parse(xml);
            var nProt = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "nProt")?.Value;
            var dh = doc.Descendants().FirstOrDefault(e => e.Name.LocalName is "dhRecbto" or "dhRegEvento")?.Value;
            var prot = long.TryParse(nProt ?? fallbackProt, out var p) ? p : 0L;
            var dtc = DateTime.TryParse(dh, out var dt) ? dt : DateTime.UtcNow;
            return (prot, dtc);
        }
        catch
        {
            return (long.TryParse(fallbackProt, out var p) ? p : 0L, DateTime.UtcNow);
        }
    }

    private static string Mask(string chave)
        => chave.Length == 44 ? $"{chave[..6]}****{chave[^6..]}" : "****";
}
