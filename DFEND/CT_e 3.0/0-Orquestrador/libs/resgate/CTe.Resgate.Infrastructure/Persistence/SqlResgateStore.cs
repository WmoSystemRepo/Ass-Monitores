using System.Data;
using CTe.Resgate.Application.Abstractions;
using CTe.Resgate.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CTe.Resgate.Infrastructure.Persistence;

/// <summary>
/// LEGADO — persistência em cte.lote_resgate_cte / item / evento.
/// Não registrado no DI do modo Carga-download (SqlCargaDownloadEnqueue).
/// </summary>
public sealed class SqlResgateStore(IConfiguration configuration, ILogger<SqlResgateStore> logger) : IResgateStore
{
    private string ConnectionString =>
        configuration.GetConnectionString("BDCTeSintetico")
        ?? throw new InvalidOperationException("ConnectionStrings:BDCTeSintetico não configurada.");

    public async Task<LoteResgate> CreateLoteAsync(string usuario, IReadOnlyList<string> chaves, CancellationToken ct)
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync(ct);
        await using var tx = await conn.BeginTransactionAsync(ct);

        var correlationId = Guid.NewGuid();
        long loteId;
        await using (var cmd = new SqlCommand(
            """
            INSERT INTO cte.lote_resgate_cte (usuario, status, total, correlation_id)
            OUTPUT INSERTED.id, INSERTED.usuario, INSERTED.criado_em, INSERTED.status, INSERTED.total,
                   INSERTED.recuperados, INSERTED.existentes, INSERTED.nao_localizados, INSERTED.erros,
                   INSERTED.chave_atual, INSERTED.passo_atual_lote, INSERTED.correlation_id
            VALUES (@usuario, @status, @total, @correlation_id)
            """, conn, (SqlTransaction)tx))
        {
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@status", LoteStatus.Aberto);
            cmd.Parameters.AddWithValue("@total", chaves.Count);
            cmd.Parameters.AddWithValue("@correlation_id", correlationId);
            await using var reader = await cmd.ExecuteReaderAsync(ct);
            if (!await reader.ReadAsync(ct))
                throw new InvalidOperationException("Falha ao criar lote.");
            loteId = reader.GetInt64(0);
            var lote = ReadLote(reader);
            await reader.CloseAsync();

            foreach (var chave in chaves)
            {
                await using var itemCmd = new SqlCommand(
                    """
                    INSERT INTO cte.item_resgate_cte (lote_id, chave, status, passo_atual)
                    VALUES (@lote_id, @chave, @status, @passo)
                    """, conn, (SqlTransaction)tx);
                itemCmd.Parameters.AddWithValue("@lote_id", loteId);
                itemCmd.Parameters.AddWithValue("@chave", chave);
                itemCmd.Parameters.AddWithValue("@status", ItemStatus.Pendente);
                itemCmd.Parameters.AddWithValue("@passo", PassoResgate.P0);
                await itemCmd.ExecuteNonQueryAsync(ct);
            }

            await AppendEventoInternalAsync(conn, (SqlTransaction)tx, loteId, null,
                $"Lote {loteId} criado com {chaves.Count} chaves", PassoResgate.P0, ct);

            await tx.CommitAsync(ct);
            lote.Id = loteId;
            return lote;
        }
    }

    public async Task<LoteResgate?> GetLoteAsync(long id, CancellationToken ct)
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(
            """
            SELECT id, usuario, criado_em, status, total, recuperados, existentes, nao_localizados, erros,
                   chave_atual, passo_atual_lote, correlation_id
            FROM cte.lote_resgate_cte WHERE id = @id
            """, conn);
        cmd.Parameters.AddWithValue("@id", id);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        return await reader.ReadAsync(ct) ? ReadLote(reader) : null;
    }

    public async Task<IReadOnlyList<ItemResgate>> GetItensAsync(long loteId, int skip, int take, CancellationToken ct)
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(
            """
            SELECT id, lote_id, chave, status, passo_atual, motivo, tentativas, atualizado_em, tempo_ms
            FROM cte.item_resgate_cte
            WHERE lote_id = @lote_id
            ORDER BY id
            OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY
            """, conn);
        cmd.Parameters.AddWithValue("@lote_id", loteId);
        cmd.Parameters.AddWithValue("@skip", skip);
        cmd.Parameters.AddWithValue("@take", take);
        var list = new List<ItemResgate>();
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
            list.Add(ReadItem(reader));
        return list;
    }

    public async Task<IReadOnlyList<EventoResgate>> GetEventosAsync(long loteId, int take, CancellationToken ct)
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(
            """
            SELECT id, lote_id, item_id, horario, mensagem, passo FROM (
                SELECT TOP (@take) id, lote_id, item_id, horario, mensagem, passo
                FROM cte.evento_resgate_cte
                WHERE lote_id = @lote_id
                ORDER BY id DESC
            ) x ORDER BY id ASC
            """, conn);
        cmd.Parameters.AddWithValue("@lote_id", loteId);
        cmd.Parameters.AddWithValue("@take", take);
        var list = new List<EventoResgate>();
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
            list.Add(ReadEvento(reader));
        return list;
    }

    public async Task<ItemResgate?> ClaimNextPendenteAsync(CancellationToken ct)
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync(ct);
        await using var tx = await conn.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);

        await using var cmd = new SqlCommand(
            """
            ;WITH c AS (
                SELECT TOP (1) i.id
                FROM cte.item_resgate_cte i WITH (UPDLOCK, READPAST, ROWLOCK)
                WHERE i.status = @pendente
                ORDER BY i.id
            )
            UPDATE i SET
                status = @proc,
                passo_atual = @p1,
                tentativas = i.tentativas + 1,
                atualizado_em = SYSUTCDATETIME()
            OUTPUT INSERTED.id, INSERTED.lote_id, INSERTED.chave, INSERTED.status, INSERTED.passo_atual,
                   INSERTED.motivo, INSERTED.tentativas, INSERTED.atualizado_em, INSERTED.tempo_ms
            FROM cte.item_resgate_cte i
            INNER JOIN c c ON c.id = i.id
            """, conn, (SqlTransaction)tx);
        cmd.Parameters.AddWithValue("@pendente", ItemStatus.Pendente);
        cmd.Parameters.AddWithValue("@proc", ItemStatus.EmProcessamento);
        cmd.Parameters.AddWithValue("@p1", PassoResgate.P1);

        ItemResgate? claimed = null;
        await using (var reader = await cmd.ExecuteReaderAsync(ct))
        {
            if (await reader.ReadAsync(ct))
                claimed = ReadItem(reader);
        }

        if (claimed is not null)
        {
            await using var loteCmd = new SqlCommand(
                """
                UPDATE cte.lote_resgate_cte SET
                    status = @proc,
                    chave_atual = @chave,
                    passo_atual_lote = @p1
                WHERE id = @lote_id
                """, conn, (SqlTransaction)tx);
            loteCmd.Parameters.AddWithValue("@proc", LoteStatus.Processando);
            loteCmd.Parameters.AddWithValue("@chave", claimed.Chave);
            loteCmd.Parameters.AddWithValue("@p1", PassoResgate.P1);
            loteCmd.Parameters.AddWithValue("@lote_id", claimed.LoteId);
            await loteCmd.ExecuteNonQueryAsync(ct);
        }

        await tx.CommitAsync(ct);
        return claimed;
    }

    public async Task UpdateItemAsync(ItemResgate item, CancellationToken ct)
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(
            """
            UPDATE cte.item_resgate_cte SET
                status = @status, passo_atual = @passo, motivo = @motivo,
                tentativas = @tentativas, atualizado_em = SYSUTCDATETIME(), tempo_ms = @tempo
            WHERE id = @id
            """, conn);
        cmd.Parameters.AddWithValue("@status", item.Status);
        cmd.Parameters.AddWithValue("@passo", item.PassoAtual);
        cmd.Parameters.AddWithValue("@motivo", (object?)item.Motivo ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@tentativas", item.Tentativas);
        cmd.Parameters.AddWithValue("@tempo", (object?)item.TempoMs ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@id", item.Id);
        await cmd.ExecuteNonQueryAsync(ct);
    }

    public async Task UpdateLoteAsync(LoteResgate lote, CancellationToken ct)
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(
            """
            UPDATE cte.lote_resgate_cte SET
                status = @status, recuperados = @rec, existentes = @exi,
                nao_localizados = @nao, erros = @err, chave_atual = @chave, passo_atual_lote = @passo
            WHERE id = @id
            """, conn);
        cmd.Parameters.AddWithValue("@status", lote.Status);
        cmd.Parameters.AddWithValue("@rec", lote.Recuperados);
        cmd.Parameters.AddWithValue("@exi", lote.Existentes);
        cmd.Parameters.AddWithValue("@nao", lote.NaoLocalizados);
        cmd.Parameters.AddWithValue("@err", lote.Erros);
        cmd.Parameters.AddWithValue("@chave", (object?)lote.ChaveAtual ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@passo", (object?)lote.PassoAtualLote ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@id", lote.Id);
        await cmd.ExecuteNonQueryAsync(ct);
    }

    public async Task AppendEventoAsync(EventoResgate evt, CancellationToken ct)
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync(ct);
        await AppendEventoInternalAsync(conn, null, evt.LoteId, evt.ItemId, evt.Mensagem, evt.Passo, ct);
    }

    public async Task RequeueStaleProcessingAsync(TimeSpan staleAfter, CancellationToken ct)
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(
            """
            UPDATE cte.item_resgate_cte SET
                status = @pendente, passo_atual = @p0,
                motivo = @motivo, atualizado_em = SYSUTCDATETIME()
            WHERE status = @proc AND atualizado_em < @cutoff
            """, conn);
        cmd.Parameters.AddWithValue("@pendente", ItemStatus.Pendente);
        cmd.Parameters.AddWithValue("@p0", PassoResgate.P0);
        cmd.Parameters.AddWithValue("@proc", ItemStatus.EmProcessamento);
        cmd.Parameters.AddWithValue("@motivo", "Retomada após stale EmProcessamento");
        cmd.Parameters.AddWithValue("@cutoff", DateTime.UtcNow - staleAfter);
        var n = await cmd.ExecuteNonQueryAsync(ct);
        if (n > 0)
            logger.LogWarning("Requeue stale: {Count} itens", n);
    }

    private static async Task AppendEventoInternalAsync(
        SqlConnection conn, SqlTransaction? tx, long loteId, long? itemId,
        string mensagem, string? passo, CancellationToken ct)
    {
        await using var cmd = tx is null
            ? new SqlCommand(
                """
                INSERT INTO cte.evento_resgate_cte (lote_id, item_id, mensagem, passo)
                VALUES (@lote_id, @item_id, @mensagem, @passo)
                """, conn)
            : new SqlCommand(
                """
                INSERT INTO cte.evento_resgate_cte (lote_id, item_id, mensagem, passo)
                VALUES (@lote_id, @item_id, @mensagem, @passo)
                """, conn, tx);
        cmd.Parameters.AddWithValue("@lote_id", loteId);
        cmd.Parameters.AddWithValue("@item_id", (object?)itemId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@mensagem", mensagem);
        cmd.Parameters.AddWithValue("@passo", (object?)passo ?? DBNull.Value);
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static LoteResgate ReadLote(SqlDataReader r) => new()
    {
        Id = r.GetInt64(0),
        Usuario = r.GetString(1),
        CriadoEm = r.GetDateTime(2),
        Status = r.GetString(3),
        Total = r.GetInt32(4),
        Recuperados = r.GetInt32(5),
        Existentes = r.GetInt32(6),
        NaoLocalizados = r.GetInt32(7),
        Erros = r.GetInt32(8),
        ChaveAtual = r.IsDBNull(9) ? null : r.GetString(9),
        PassoAtualLote = r.IsDBNull(10) ? null : r.GetString(10),
        CorrelationId = r.GetGuid(11)
    };

    private static ItemResgate ReadItem(SqlDataReader r) => new()
    {
        Id = r.GetInt64(0),
        LoteId = r.GetInt64(1),
        Chave = r.GetString(2).Trim(),
        Status = r.GetString(3),
        PassoAtual = r.GetString(4),
        Motivo = r.IsDBNull(5) ? null : r.GetString(5),
        Tentativas = r.GetInt32(6),
        AtualizadoEm = r.GetDateTime(7),
        TempoMs = r.IsDBNull(8) ? null : r.GetInt32(8)
    };

    private static EventoResgate ReadEvento(SqlDataReader r) => new()
    {
        Id = r.GetInt64(0),
        LoteId = r.GetInt64(1),
        ItemId = r.IsDBNull(2) ? null : r.GetInt64(2),
        Horario = r.GetDateTime(3),
        Mensagem = r.GetString(4),
        Passo = r.IsDBNull(5) ? null : r.GetString(5)
    };
}
