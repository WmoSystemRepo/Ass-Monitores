using System.Text;
using CTe.Resgate.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace CTe.Resgate.Infrastructure.Tests;

public sealed class SqlDocumentoRepositoryIntegrationTests
{
    private static IConfiguration? TryLoadConfig()
    {
        var baseDir = AppContext.BaseDirectory;
        var devSettings = Path.GetFullPath(Path.Combine(
            baseDir, "..", "..", "..", "..", "src", "CTe.Resgate.Api", "appsettings.Development.json"));

        if (!File.Exists(devSettings))
            return null;

        return new ConfigurationBuilder()
            .AddJsonFile(devSettings, optional: false)
            .Build();
    }

    private static SqlDocumentoRepository? TryCreateRepo()
    {
        var config = TryLoadConfig();
        var cs = config?.GetConnectionString("BDCTeSintetico");
        if (string.IsNullOrWhiteSpace(cs))
            return null;

        return new SqlDocumentoRepository(config!, NullLogger<SqlDocumentoRepository>.Instance);
    }

    [Fact]
    public async Task SqlDocumentoRepository_exists_e_insert_if_absent()
    {
        var repo = TryCreateRepo();
        if (repo is null)
            return;

        var chave = GenerateUniqueKey();
        var xml = BuildXml(chave, 123456789012345, DateTime.UtcNow);

        try
        {
            var existedBefore = await repo.ExistsAsync(chave, CancellationToken.None);
            Assert.False(existedBefore);

            await repo.InsertIfAbsentAsync(chave, xml, "123456789012345", "0", CancellationToken.None);
            var existsAfterInsert = await repo.ExistsAsync(chave, CancellationToken.None);
            Assert.True(existsAfterInsert);

            // Segunda chamada não deve falhar nem duplicar
            await repo.InsertIfAbsentAsync(chave, xml, "123456789012345", "0", CancellationToken.None);
            var existsAfterSecond = await repo.ExistsAsync(chave, CancellationToken.None);
            Assert.True(existsAfterSecond);
        }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number is 53 or -1 or 4060 or 18456)
        {
            // SQL DEV indisponível neste ambiente — rode tools/validar-t03.ps1 na máquina com VPN.
            return;
        }
        finally
        {
            await CleanupIfExistsAsync(chave);
        }
    }

    private static string GenerateUniqueKey()
    {
        // 44 dígitos: prefixo válido CT-e + ticks para unicidade
        var seed = DateTime.UtcNow.Ticks.ToString();
        var digits = new string(seed.Where(char.IsDigit).ToArray());
        var payload = ("3526" + digits).PadRight(44, '7');
        return payload[..44];
    }

    private static string BuildXml(string chave, long protocolo, DateTime dt)
    {
        var dh = dt.ToString("yyyy-MM-ddTHH:mm:ssK");
        var sb = new StringBuilder();
        sb.Append("<procCTe>");
        sb.Append("<protCTe><infProt>");
        sb.Append($"<chCTe>{chave}</chCTe>");
        sb.Append($"<nProt>{protocolo}</nProt>");
        sb.Append($"<dhRecbto>{dh}</dhRecbto>");
        sb.Append("</infProt></protCTe>");
        sb.Append("</procCTe>");
        return sb.ToString();
    }

    private static async Task CleanupIfExistsAsync(string chave)
    {
        var config = TryLoadConfig();
        var cs = config?.GetConnectionString("BDCTeSintetico");
        if (string.IsNullOrWhiteSpace(cs))
            return;

        await using var conn = new Microsoft.Data.SqlClient.SqlConnection(cs);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM cte.documento_conhecimento_transporte_eletronico_autorizacao WHERE cod_chave_acesso = @chave";
        cmd.Parameters.AddWithValue("@chave", chave);
        await cmd.ExecuteNonQueryAsync();
    }
}
