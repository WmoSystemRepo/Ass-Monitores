using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace CTe.Modules.Monitors.Infrastructure;

/// <summary>
/// Telemetria mínima de filas/docs para animações do pipeline (paridade CT_e 2.0),
/// sem copiar o SqlMonitorRepository completo.
/// </summary>
internal static class MonitorTelemetrySql
{
    public static async Task<(long TempBacklog, long BrokerDepth, IReadOnlyList<object> RecentDocs)> ReadAsync(
        string? connectionString,
        string domain,
        int timeoutSeconds,
        ILogger logger,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return (0, 0, Array.Empty<object>());
        }

        try
        {
            await using var conn = new SqlConnection(connectionString);
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct);
            linked.CancelAfter(TimeSpan.FromSeconds(Math.Max(1, timeoutSeconds)));
            await conn.OpenAsync(linked.Token);

            var (tempSql, brokerSql, docsSql) = ResolveQueries(domain);
            var temp = await ScalarLongAsync(conn, tempSql, timeoutSeconds, linked.Token);
            var broker = string.IsNullOrWhiteSpace(brokerSql)
                ? 0
                : await ScalarLongAsync(conn, brokerSql!, timeoutSeconds, linked.Token);
            var docs = string.IsNullOrWhiteSpace(docsSql)
                ? Array.Empty<object>()
                : await ReadRecentDocsAsync(conn, docsSql!, timeoutSeconds, linked.Token);

            return (temp, broker, docs);
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Telemetria de filas falhou para domain={Domain}", domain);
            return (0, 0, Array.Empty<object>());
        }
    }

    private static (string TempSql, string? BrokerSql, string? DocsSql) ResolveQueries(string domain)
    {
        switch (domain.Trim().ToLowerInvariant())
        {
            case "receptor":
                return (
                    """
                    SELECT COUNT(1)
                    FROM cte.tmp_documento_conhecimento_transporte_eletronico WITH (READPAST)
                    """,
                    """
                    SELECT COUNT(1)
                    FROM fila_alvo_cte_arquivador WITH (READPAST)
                    """,
                    """
                    SELECT TOP (8)
                      num_sequencial_unico AS nsu,
                      num_sequencial_unico_final AS nsuFinal,
                      qtd_documento AS qtdDocumento,
                      dtc_atualizacao AS dtcAtualizacao
                    FROM cte.tmp_documento_conhecimento_transporte_eletronico WITH (READPAST)
                    ORDER BY dtc_atualizacao DESC
                    """);
            case "arquivador":
            case "carga":
                return (
                    """
                    SELECT COUNT(1)
                    FROM cte.tmp_documento_conhecimento_transporte_eletronico WITH (READPAST)
                    """,
                    """
                    SELECT COUNT(1)
                    FROM fila_alvo_cte_arquivador WITH (READPAST)
                    """,
                    null);
            case "sintetizador":
            case "analisador":
            case "integrador":
                return (
                    """
                    SELECT COUNT(1)
                    FROM cte.tmp_sintetico_conhecimento_transporte_eletronico WITH (READPAST)
                    """,
                    null,
                    null);
            default:
                return ("SELECT 0", null, null);
        }
    }

    private static async Task<long> ScalarLongAsync(
        SqlConnection conn,
        string sql,
        int timeoutSeconds,
        CancellationToken ct)
    {
        try
        {
            await using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = Math.Max(1, timeoutSeconds);
            cmd.CommandText = sql;
            var raw = await cmd.ExecuteScalarAsync(ct);
            return raw is null or DBNull ? 0 : Convert.ToInt64(raw);
        }
        catch
        {
            return 0;
        }
    }

    private static async Task<IReadOnlyList<object>> ReadRecentDocsAsync(
        SqlConnection conn,
        string sql,
        int timeoutSeconds,
        CancellationToken ct)
    {
        var list = new List<object>();
        try
        {
            await using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = Math.Max(1, timeoutSeconds);
            cmd.CommandText = sql;
            await using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                list.Add(new
                {
                    nsu = reader["nsu"] is DBNull ? 0 : Convert.ToInt64(reader["nsu"]),
                    nsuFinal = reader["nsuFinal"] is DBNull or null
                        ? (long?)null
                        : Convert.ToInt64(reader["nsuFinal"]),
                    qtdDocumento = reader["qtdDocumento"] is DBNull ? 0 : Convert.ToInt32(reader["qtdDocumento"]),
                    dtcAtualizacao = reader["dtcAtualizacao"] is DateTime dt
                        ? new DateTimeOffset(DateTime.SpecifyKind(dt, DateTimeKind.Local))
                        : (DateTimeOffset?)null,
                    hasError = false
                });
            }
        }
        catch
        {
            return Array.Empty<object>();
        }

        return list;
    }
}
