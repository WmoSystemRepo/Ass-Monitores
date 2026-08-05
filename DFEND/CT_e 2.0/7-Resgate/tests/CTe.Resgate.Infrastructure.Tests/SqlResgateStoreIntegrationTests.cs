using CTe.Resgate.Domain;
using CTe.Resgate.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace CTe.Resgate.Infrastructure.Tests;

public sealed class SqlResgateStoreIntegrationTests
{
    private const string SmokeUser = "T02_smoke_test";
    private static readonly string[] SmokeKeys =
    [
        "35260712345678901234567890123456789012345678",
        "35260712345678901234567890123456789012345679"
    ];

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

    private static SqlResgateStore? TryCreateStore()
    {
        var config = TryLoadConfig();
        var cs = config?.GetConnectionString("BDCTeSintetico");
        if (string.IsNullOrWhiteSpace(cs))
            return null;

        return new SqlResgateStore(config!, NullLogger<SqlResgateStore>.Instance);
    }

    [Fact]
    public async Task SqlResgateStore_crud_e_claim_pendente()
    {
        var store = TryCreateStore();
        if (store is null)
            return;

        long loteId = 0;
        try
        {
            var lote = await store.CreateLoteAsync(SmokeUser, SmokeKeys, CancellationToken.None);
            loteId = lote.Id;

            Assert.True(loteId > 0);
            Assert.Equal(SmokeUser, lote.Usuario);
            Assert.Equal(LoteStatus.Aberto, lote.Status);
            Assert.Equal(SmokeKeys.Length, lote.Total);

            var loaded = await store.GetLoteAsync(loteId, CancellationToken.None);
            Assert.NotNull(loaded);
            Assert.Equal(loteId, loaded!.Id);

            var itens = await store.GetItensAsync(loteId, 0, 10, CancellationToken.None);
            Assert.Equal(2, itens.Count);
            Assert.All(itens, i =>
            {
                Assert.Equal(ItemStatus.Pendente, i.Status);
                Assert.Equal(PassoResgate.P0, i.PassoAtual);
            });

            var eventos = await store.GetEventosAsync(loteId, 10, CancellationToken.None);
            Assert.NotEmpty(eventos);

            var claimed = await store.ClaimNextPendenteAsync(CancellationToken.None);
            Assert.NotNull(claimed);
            Assert.Equal(ItemStatus.EmProcessamento, claimed!.Status);
            Assert.Equal(PassoResgate.P1, claimed.PassoAtual);

            var loteProc = await store.GetLoteAsync(loteId, CancellationToken.None);
            Assert.NotNull(loteProc);
            Assert.Equal(LoteStatus.Processando, loteProc!.Status);
            Assert.Equal(claimed.Chave, loteProc.ChaveAtual);

            claimed.PassoAtual = PassoResgate.P5b;
            claimed.Status = ItemStatus.Recuperado;
            claimed.Motivo = "T02 smoke";
            await store.UpdateItemAsync(claimed, CancellationToken.None);

            loteProc.Recuperados = 1;
            loteProc.PassoAtualLote = PassoResgate.P7;
            await store.UpdateLoteAsync(loteProc, CancellationToken.None);

            await store.AppendEventoAsync(new EventoResgate
            {
                LoteId = loteId,
                ItemId = claimed.Id,
                Mensagem = "T02 entrega P5b",
                Passo = PassoResgate.P5b
            }, CancellationToken.None);

            var eventosAfter = await store.GetEventosAsync(loteId, 20, CancellationToken.None);
            Assert.Contains(eventosAfter, e => e.Passo == PassoResgate.P5b);
        }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number is 53 or -1 or 4060 or 18456)
        {
            // SQL DEV indisponível neste ambiente — rode tools/validar-t02.ps1 na máquina com VPN.
            return;
        }
        finally
        {
            if (loteId > 0)
                await CleanupSmokeLoteAsync(store, loteId);
        }
    }

    private static async Task CleanupSmokeLoteAsync(SqlResgateStore store, long loteId)
    {
        var config = TryLoadConfig()!;
        var cs = config.GetConnectionString("BDCTeSintetico")!;
        await using var conn = new Microsoft.Data.SqlClient.SqlConnection(cs);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            DELETE FROM cte.evento_resgate_cte WHERE lote_id = @id;
            DELETE FROM cte.item_resgate_cte WHERE lote_id = @id;
            DELETE FROM cte.lote_resgate_cte WHERE id = @id AND usuario = @user;
            """;
        cmd.Parameters.AddWithValue("@id", loteId);
        cmd.Parameters.AddWithValue("@user", SmokeUser);
        await cmd.ExecuteNonQueryAsync();
    }
}
