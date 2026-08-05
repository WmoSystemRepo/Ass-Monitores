using CTe.Resgate.Application.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CTe.Resgate.Infrastructure.Persistence;

/// <summary>
/// Informa chaves ao Download da Carga (não executa download).
/// Implementação atual da Carga: des_esquema = chave de acesso (identificado na análise técnica).
/// </summary>
public sealed class SqlCargaDownloadEnqueue(IConfiguration configuration) : ICargaDownloadEnqueue
{
    private readonly string _cs = configuration.GetConnectionString("BDCTeSintetico")
        ?? throw new InvalidOperationException("ConnectionStrings:BDCTeSintetico não configurada.");

    private static long _seq;

    public async Task<CargaEnqueueResult> EnfileirarAsync(
        string usuario, IReadOnlyList<string> chaves, CancellationToken ct)
    {
        var ids = new List<long>(chaves.Count);
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);

        foreach (var chave in chaves)
        {
            await using var tx = (SqlTransaction)await conn.BeginTransactionAsync(ct);
            try
            {
                var id = await AllocUniqueIdAsync(conn, tx, ct);
                await InsertTempAsync(conn, tx, id, chave, ct);
                await SendFilaAsync(conn, tx, id, ct);
                await tx.CommitAsync(ct);
                ids.Add(id);
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }

        var pendentes = await ContarPendentesDownloadAsync(conn, ct);
        var profundidade = await ContarFilaBrokerAsync(conn, ct);
        var idade = await IdadeMaxTempMinutosAsync(conn, ct);
        return new CargaEnqueueResult(ids.Count, ids, pendentes, profundidade, idade, usuario);
    }

    public async Task<object> GetFilaStatusAsync(CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);

        const string sql = """
            SELECT TOP (200)
              num_sequencial_unico AS id,
              des_esquema AS chave,
              des_mensagem_erro AS erro,
              dtc_atualizacao AS atualizadoEm
            FROM cte.tmp_integracao_conhecimento_transporte_eletronico WITH (READPAST)
            WHERE LEN(LTRIM(RTRIM(des_esquema))) = 44
              AND des_esquema NOT LIKE '%[^0-9]%'
            ORDER BY dtc_atualizacao DESC;
            """;

        var itens = new List<object>();
        await using (var cmd = new SqlCommand(sql, conn))
        await using (var rd = await cmd.ExecuteReaderAsync(ct))
        {
            while (await rd.ReadAsync(ct))
            {
                var chave = rd.GetString(1);
                var erro = rd.IsDBNull(2) ? null : rd.GetString(2);
                var temErro = !string.IsNullOrWhiteSpace(erro);
                itens.Add(new
                {
                    id = rd.GetInt64(0),
                    chaveMascarada = Mask(chave),
                    status = temErro ? "Erro" : "Pendente",
                    erro = temErro ? erro : null,
                    atualizadoEm = rd.IsDBNull(3) ? (DateTime?)null : rd.GetDateTime(3)
                });
            }
        }

        var profundidadeFila = await ContarFilaBrokerAsync(conn, ct);
        var idade = await IdadeMaxTempMinutosAsync(conn, ct);
        return new
        {
            modo = "carga-download",
            aviso = "Enfileirado não significa resgatado. A Carga (ProcessarDownload) executa o download.",
            mensagem = "Chaves pendentes na temp (Carga consome a fila). Continuidade do fluxo normal: validar em homologação (V2).",
            pendentesTemp = itens.Count,
            profundidadeFilaBroker = profundidadeFila,
            idadeMaxTempMinutos = idade,
            consumidoresFila = new[] { "Carga.ProcessarDownload", "Integrador.Processar" },
            riscoConcorrencia = profundidadeFila > 0
                ? "Fila compartilhada: Carga e Integrador fazem RECEIVE — preferir Carga ativa e cuidado com Integrador na mesma janela."
                : null,
            checklistCarga = new
            {
                executar = "Executar=1",
                executarAuto = "ExecutarAuto=1",
                codServico = 99,
                monitor = "http://localhost:4260"
            },
            itens,
            servidorEm = DateTime.UtcNow
        };
    }

    public async Task<object> GetStatusChavesAsync(IReadOnlyList<string> chaves, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);

        var itens = new List<object>();
        foreach (var chave in chaves.Take(1000))
        {
            var (status, detalhe) = await ResolverStatusChaveAsync(conn, chave, ct);
            itens.Add(new
            {
                chaveMascarada = Mask(chave),
                status,
                detalhe
            });
        }

        return new
        {
            modo = "carga-download",
            aviso = "Enfileirado ≠ resgatado. PersistidoSintetico = documento no sintético (continuidade do fluxo normal: ver V2).",
            total = itens.Count,
            itens,
            servidorEm = DateTime.UtcNow
        };
    }

    private static async Task<(string Status, string? Detalhe)> ResolverStatusChaveAsync(
        SqlConnection conn, string chave, CancellationToken ct)
    {
        await using (var cmd = new SqlCommand(
            """
            SELECT TOP (1) des_mensagem_erro
            FROM cte.tmp_integracao_conhecimento_transporte_eletronico WITH (READPAST)
            WHERE des_esquema = @chave
            ORDER BY dtc_atualizacao DESC;
            """, conn))
        {
            cmd.Parameters.AddWithValue("@chave", chave);
            await using var rd = await cmd.ExecuteReaderAsync(ct);
            if (await rd.ReadAsync(ct))
            {
                var erro = rd.IsDBNull(0) ? null : rd.GetString(0);
                if (!string.IsNullOrWhiteSpace(erro))
                    return ("Erro", erro);
                return ("Pendente", "Aguardando Carga (temp).");
            }
        }

        await using (var cmd = new SqlCommand(
            """
            SELECT TOP (1) 1
            FROM cte.documento_conhecimento_transporte_eletronico_autorizacao WITH (READPAST)
            WHERE cod_chave_acesso = @chave;
            """, conn))
        {
            cmd.Parameters.AddWithValue("@chave", chave);
            var exists = await cmd.ExecuteScalarAsync(ct);
            if (exists is not null)
                return ("Baixado", "Persistido no sintético (autorização). Continuidade do fluxo normal: validar V2.");
        }

        return ("Indeterminado", "Não está na temp nem na autorização — em trânsito na fila, outro tipo de DF-e, ou ainda não processado.");
    }

    private static async Task<long> AllocUniqueIdAsync(SqlConnection conn, SqlTransaction tx, CancellationToken ct)
    {
        for (var attempt = 0; attempt < 8; attempt++)
        {
            var id = NextId();
            await using var cmd = new SqlCommand(
                "SELECT 1 FROM cte.tmp_integracao_conhecimento_transporte_eletronico WHERE num_sequencial_unico = @id",
                conn, tx);
            cmd.Parameters.AddWithValue("@id", id);
            var exists = await cmd.ExecuteScalarAsync(ct);
            if (exists is null) return id;
        }

        throw new InvalidOperationException("Não foi possível alocar num_sequencial_unico único para a fila da Carga.");
    }

    private static long NextId()
    {
        var n = Interlocked.Increment(ref _seq);
        var ticks = DateTime.UtcNow.Ticks % 100_000_000_000_000L;
        return 88_000_000_000_000_000L + ticks + (n % 10_000);
    }

    private static async Task InsertTempAsync(
        SqlConnection conn, SqlTransaction tx, long id, string chave, CancellationToken ct)
    {
        const string sql = """
            INSERT INTO cte.tmp_integracao_conhecimento_transporte_eletronico
            (
              num_sequencial_unico,
              num_sequencial_unico_final,
              qtd_documento,
              xml_documento,
              des_esquema,
              des_mensagem_erro,
              dtc_autorizacao,
              dtc_atualizacao
            )
            VALUES
            (
              @id,
              @id,
              1,
              CONVERT(varbinary(max), ''),
              @chave,
              '',
              GETDATE(),
              GETDATE()
            );
            """;

        await using var cmd = new SqlCommand(sql, conn, tx);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@chave", chave);
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static async Task SendFilaAsync(
        SqlConnection conn, SqlTransaction tx, long id, CancellationToken ct)
    {
        // Implementação atual: BdCTeSintetico.EnviarFilaIntegrador — corpo = num_sequencial_unico.
        const string sql = """
            DECLARE @IdDialogo UNIQUEIDENTIFIER;
            BEGIN DIALOG @IdDialogo
            FROM SERVICE servico_iniciador_cte_integrador
            TO SERVICE N'servico_alvo_cte_integrador'
            ON CONTRACT contrato_cte_integrador
            WITH ENCRYPTION = OFF;
            SEND ON CONVERSATION @IdDialogo
            MESSAGE TYPE tipo_mensagem_cte_integrador (@chave);
            """;

        await using var cmd = new SqlCommand(sql, conn, tx);
        cmd.Parameters.AddWithValue("@chave", id.ToString());
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static async Task<int> ContarPendentesDownloadAsync(SqlConnection conn, CancellationToken ct)
    {
        const string sql = """
            SELECT COUNT(1)
            FROM cte.tmp_integracao_conhecimento_transporte_eletronico WITH (READPAST)
            WHERE LEN(LTRIM(RTRIM(des_esquema))) = 44
              AND des_esquema NOT LIKE '%[^0-9]%';
            """;
        await using var cmd = new SqlCommand(sql, conn);
        var o = await cmd.ExecuteScalarAsync(ct);
        return Convert.ToInt32(o);
    }

    private static async Task<int> ContarFilaBrokerAsync(SqlConnection conn, CancellationToken ct)
    {
        try
        {
            const string sql = "SELECT COUNT(*) FROM fila_alvo_cte_integrador WITH (NOLOCK);";
            await using var cmd = new SqlCommand(sql, conn);
            var o = await cmd.ExecuteScalarAsync(ct);
            return Convert.ToInt32(o);
        }
        catch
        {
            return -1;
        }
    }

    private static async Task<double?> IdadeMaxTempMinutosAsync(SqlConnection conn, CancellationToken ct)
    {
        try
        {
            const string sql = """
                SELECT MIN(dtc_atualizacao)
                FROM cte.tmp_integracao_conhecimento_transporte_eletronico WITH (READPAST)
                WHERE LEN(LTRIM(RTRIM(des_esquema))) = 44
                  AND des_esquema NOT LIKE '%[^0-9]%';
                """;
            await using var cmd = new SqlCommand(sql, conn);
            var o = await cmd.ExecuteScalarAsync(ct);
            if (o is null or DBNull) return null;
            var min = Convert.ToDateTime(o);
            return Math.Round((DateTime.Now - min).TotalMinutes, 1);
        }
        catch
        {
            return null;
        }
    }

    private static string Mask(string chave)
        => chave.Length != 44 ? "****" : $"{chave[..6]}****{chave[^6..]}";
}
