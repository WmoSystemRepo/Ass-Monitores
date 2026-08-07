using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace CTe.Modules.Monitors.Infrastructure;

/// <summary>
/// Telemetria in-process alinhada ao SnapshotAggregator CT_e 2.0
/// (filas, docs, logs, serviço, Intervalo/NSU) para o pipeline animar.
/// </summary>
internal static class MonitorTelemetrySql
{
    internal sealed record ServiceTelemetry(
        string? DesServico,
        string? NomServidor,
        DateTimeOffset? DtcExecucao,
        string? MainNsu);

    internal sealed record SnapshotTelemetry(
        long TempBacklog,
        long BrokerDepth,
        DateTimeOffset? OldestTempAt,
        IReadOnlyList<object> RecentDocs,
        IReadOnlyList<object> Logs,
        IReadOnlyDictionary<string, string> Configs,
        ServiceTelemetry? Service,
        int? Executar);

    public static async Task<SnapshotTelemetry> ReadSnapshotAsync(
        string? connectionString,
        string domain,
        int codServico,
        int timeoutSeconds,
        ILogger logger,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(connectionString) || codServico <= 0)
        {
            return Empty();
        }

        try
        {
            await using var conn = new SqlConnection(connectionString);
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct);
            linked.CancelAfter(TimeSpan.FromSeconds(Math.Max(3, timeoutSeconds * 3)));
            await conn.OpenAsync(linked.Token);

            var schema = ResolveSchema(domain);
            var configs = await ReadConfigsAsync(conn, schema, codServico, timeoutSeconds, linked.Token);
            var service = await ReadServiceAsync(conn, schema, codServico, timeoutSeconds, linked.Token);
            var (temp, broker, oldest) = await ReadQueuesAsync(conn, domain, timeoutSeconds, linked.Token);
            var docs = await ReadRecentDocsAsync(conn, domain, timeoutSeconds, linked.Token);
            var logs = await ReadLogsAsync(conn, schema, afterSeq: 0, take: 80, timeoutSeconds, linked.Token);

            int? executar = null;
            if (configs.TryGetValue("Executar", out var exRaw) && int.TryParse(exRaw, out var ex))
            {
                executar = ex;
            }

            return new SnapshotTelemetry(temp, broker, oldest, docs, logs, configs, service, executar);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Telemetria completa falhou para domain={Domain}", domain);
            return Empty();
        }
    }

    public static async Task<IReadOnlyList<object>> ReadLogsOnlyAsync(
        string? connectionString,
        string domain,
        long afterSeq,
        int take,
        int timeoutSeconds,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return Array.Empty<object>();
        }

        try
        {
            await using var conn = new SqlConnection(connectionString);
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct);
            linked.CancelAfter(TimeSpan.FromSeconds(Math.Max(1, timeoutSeconds)));
            await conn.OpenAsync(linked.Token);
            var schema = ResolveSchema(domain);
            return await ReadLogsAsync(conn, schema, afterSeq, Math.Clamp(take, 1, 500), timeoutSeconds, linked.Token);
        }
        catch
        {
            return Array.Empty<object>();
        }
    }

    private static SnapshotTelemetry Empty() => new(
        0, 0, null,
        Array.Empty<object>(),
        Array.Empty<object>(),
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
        null,
        null);

    private sealed record Schema(
        string ConfigTable,
        string ConfigCodColumn,
        string ServiceTable,
        string ServiceCodColumn,
        string LogTable,
        string LogSeqColumn,
        string LogDatePrimary,
        string LogMsgPrimary,
        string LogDateFallback,
        string LogMsgFallback);

    private static Schema ResolveSchema(string domain) =>
        domain.Trim().ToLowerInvariant() switch
        {
            "receptor" or "arquivador" or "carga" => new Schema(
                "configuracao_recepcao_conhecimento_transporte_eletronico",
                "cod_servico_recepcao_conhecimento_transporte_eletronico",
                "servico_recepcao_conhecimento_transporte_eletronico",
                "cod_servico_recepcao_conhecimento_transporte_eletronico",
                "log_recepcao_conhecimento_transporte_eletronico",
                "seq_log_recepcao_conhecimento_transporte_eletronico",
                "dtc_insercao",
                "des_log",
                "dtc_atualizacao",
                "des_mensagem"),
            _ => new Schema(
                "configuracao_sintetico_conhecimento_transporte_eletronico",
                "cod_servico_sintetico_conhecimento_transporte_eletronico",
                "servico_sintetico_conhecimento_transporte_eletronico",
                "cod_servico_sintetico_conhecimento_transporte_eletronico",
                "log_sintetico_conhecimento_transporte_eletronico",
                "seq_log_sintetico_conhecimento_transporte_eletronico",
                "dtc_insercao",
                "des_log",
                "dtc_atualizacao",
                "des_mensagem")
        };

    /// <summary>
    /// Temp + fila de entrada por domínio — alinhado ao SqlMonitorRepository CT_e 2.0
    /// (âncora do cronômetro de “último lote” / queuesConsuming).
    /// </summary>
    private static (string TempTable, string? BrokerQueue) ResolveTempAndBroker(string domain) =>
        domain.Trim().ToLowerInvariant() switch
        {
            "receptor" or "arquivador" => (
                "tmp_documento_conhecimento_transporte_eletronico",
                "fila_alvo_cte_arquivador"),
            "sintetizador" => (
                "tmp_sintetizador_conhecimento_transporte_eletronico",
                "fila_alvo_cte_sintetizador"),
            "analisador" => (
                "tmp_analise_conhecimento_transporte_eletronico",
                "fila_alvo_cte_analisador"),
            "integrador" or "carga" => (
                "tmp_integracao_conhecimento_transporte_eletronico",
                "fila_alvo_cte_integrador"),
            _ => (
                "tmp_documento_conhecimento_transporte_eletronico",
                null)
        };

    private static async Task<(long Temp, long Broker, DateTimeOffset? Oldest)> ReadQueuesAsync(
        SqlConnection conn,
        string domain,
        int timeoutSeconds,
        CancellationToken ct)
    {
        var (tempTable, brokerQueue) = ResolveTempAndBroker(domain);
        var tempSql = $"""
            SELECT COUNT(1) AS total, MIN(dtc_atualizacao) AS oldest
            FROM cte.{tempTable} WITH (READPAST)
            """;
        var brokerSql = brokerQueue is null
            ? null
            : $"""
                SELECT COUNT(1)
                FROM {brokerQueue} WITH (READPAST)
                """;

        long temp = 0;
        DateTimeOffset? oldest = null;
        try
        {
            await using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = Math.Max(1, timeoutSeconds);
            cmd.CommandText = tempSql;
            await using var reader = await cmd.ExecuteReaderAsync(ct);
            if (await reader.ReadAsync(ct))
            {
                temp = reader.IsDBNull(0) ? 0 : Convert.ToInt64(reader.GetValue(0));
                if (!reader.IsDBNull(1))
                {
                    var dt = reader.GetDateTime(1);
                    oldest = new DateTimeOffset(DateTime.SpecifyKind(dt, DateTimeKind.Local));
                }
            }
        }
        catch
        {
            temp = 0;
        }

        long broker = 0;
        if (!string.IsNullOrWhiteSpace(brokerSql))
        {
            broker = await ScalarLongAsync(conn, brokerSql, timeoutSeconds, ct);
        }

        return (temp, broker, oldest);
    }

    private static async Task<IReadOnlyDictionary<string, string>> ReadConfigsAsync(
        SqlConnection conn,
        Schema schema,
        int codServico,
        int timeoutSeconds,
        CancellationToken ct)
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        try
        {
            await using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = Math.Max(1, timeoutSeconds);
            cmd.CommandText = $"""
                SELECT c.des_configuracao, c.nom_configuracao
                FROM cte.{schema.ConfigTable} c WITH (READPAST)
                WHERE c.sts_ativo = 1
                  AND c.{schema.ConfigCodColumn} = @cod
                """;
            cmd.Parameters.AddWithValue("@cod", codServico);
            await using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                var key = reader["des_configuracao"]?.ToString() ?? string.Empty;
                var val = reader["nom_configuracao"]?.ToString() ?? string.Empty;
                if (!string.IsNullOrEmpty(key))
                {
                    map[key] = val;
                }
            }
        }
        catch
        {
            // ignore
        }

        return map;
    }

    private static async Task<ServiceTelemetry?> ReadServiceAsync(
        SqlConnection conn,
        Schema schema,
        int codServico,
        int timeoutSeconds,
        CancellationToken ct)
    {
        try
        {
            await using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = Math.Max(1, timeoutSeconds);
            cmd.CommandText = $"""
                SELECT TOP (1)
                  des_servico,
                  nom_servidor,
                  dtc_execucao,
                  num_sequencial_unico
                FROM cte.{schema.ServiceTable} WITH (NOLOCK)
                WHERE {schema.ServiceCodColumn} = @cod
                """;
            cmd.Parameters.AddWithValue("@cod", codServico);
            await using var reader = await cmd.ExecuteReaderAsync(ct);
            if (!await reader.ReadAsync(ct))
            {
                return null;
            }

            return new ServiceTelemetry(
                reader["des_servico"] as string,
                reader["nom_servidor"] as string,
                reader["dtc_execucao"] is DateTime dtc
                    ? new DateTimeOffset(DateTime.SpecifyKind(dtc, DateTimeKind.Local))
                    : null,
                reader["num_sequencial_unico"]?.ToString());
        }
        catch
        {
            return null;
        }
    }

    private static async Task<IReadOnlyList<object>> ReadRecentDocsAsync(
        SqlConnection conn,
        string domain,
        int timeoutSeconds,
        CancellationToken ct)
    {
        var (table, _) = ResolveTempAndBroker(domain);

        var list = new List<object>();
        try
        {
            await using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = Math.Max(1, timeoutSeconds);
            cmd.CommandText = $"""
                SELECT TOP (12)
                  num_sequencial_unico AS nsu,
                  num_sequencial_unico_final AS nsuFinal,
                  qtd_documento AS qtdDocumento,
                  des_mensagem_erro AS mensagemErro,
                  dtc_documento AS dtcDocumento,
                  dtc_atualizacao AS dtcAtualizacao
                FROM cte.{table} WITH (READPAST)
                ORDER BY dtc_atualizacao DESC
                """;
            await using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                var erro = reader["mensagemErro"] as string;
                list.Add(new
                {
                    nsu = reader["nsu"] is DBNull ? 0L : Convert.ToInt64(reader["nsu"]),
                    nsuFinal = reader["nsuFinal"] is DBNull or null
                        ? (long?)null
                        : Convert.ToInt64(reader["nsuFinal"]),
                    qtdDocumento = reader["qtdDocumento"] is DBNull ? 0 : Convert.ToInt32(reader["qtdDocumento"]),
                    mensagemErro = erro,
                    dtcDocumento = reader["dtcDocumento"] is DateTime d1
                        ? new DateTimeOffset(DateTime.SpecifyKind(d1, DateTimeKind.Local))
                        : (DateTimeOffset?)null,
                    dtcAtualizacao = reader["dtcAtualizacao"] is DateTime d2
                        ? new DateTimeOffset(DateTime.SpecifyKind(d2, DateTimeKind.Local))
                        : (DateTimeOffset?)null,
                    hasError = !string.IsNullOrWhiteSpace(erro)
                });
            }
        }
        catch
        {
            return Array.Empty<object>();
        }

        return list;
    }

    private static async Task<IReadOnlyList<object>> ReadLogsAsync(
        SqlConnection conn,
        Schema schema,
        long afterSeq,
        int take,
        int timeoutSeconds,
        CancellationToken ct)
    {
        var primary = $"""
            SELECT TOP (@take)
              l.{schema.LogSeqColumn} AS seq_log,
              l.{schema.LogDatePrimary} AS dtc_log,
              l.{schema.LogMsgPrimary} AS des_log
            FROM cte.{schema.LogTable} l WITH (READPAST)
            WHERE l.{schema.LogSeqColumn} > @after
            ORDER BY l.{schema.LogSeqColumn} DESC
            """;
        var fallback = $"""
            SELECT TOP (@take)
              l.{schema.LogSeqColumn} AS seq_log,
              l.{schema.LogDateFallback} AS dtc_log,
              l.{schema.LogMsgFallback} AS des_log
            FROM cte.{schema.LogTable} l WITH (READPAST)
            WHERE l.{schema.LogSeqColumn} > @after
            ORDER BY l.{schema.LogSeqColumn} DESC
            """;

        var rows = await TryReadLogRowsAsync(conn, primary, afterSeq, take, timeoutSeconds, ct);
        if (rows.Count == 0)
        {
            rows = await TryReadLogRowsAsync(conn, fallback, afterSeq, take, timeoutSeconds, ct);
        }

        return rows
            .OrderBy(r => r.SeqLog)
            .Select(r => (object)new
            {
                seqLog = r.SeqLog,
                dtcLog = r.DtcLog,
                mensagem = r.Mensagem,
                threadId = r.ThreadId,
                cStat = r.CStat,
                severityHint = r.SeverityHint
            })
            .ToList();
    }

    private sealed record LogRow(
        long SeqLog,
        DateTimeOffset? DtcLog,
        string? Mensagem,
        int? ThreadId,
        string? CStat,
        string SeverityHint);

    private static async Task<List<LogRow>> TryReadLogRowsAsync(
        SqlConnection conn,
        string sql,
        long afterSeq,
        int take,
        int timeoutSeconds,
        CancellationToken ct)
    {
        var list = new List<LogRow>();
        try
        {
            await using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = Math.Max(1, timeoutSeconds);
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@take", take);
            cmd.Parameters.AddWithValue("@after", afterSeq);
            await using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                var msg = reader["des_log"] as string;
                var (threadId, cStat) = ParseLogMessage(msg);
                list.Add(new LogRow(
                    Convert.ToInt64(reader["seq_log"]),
                    reader["dtc_log"] is DateTime d
                        ? new DateTimeOffset(DateTime.SpecifyKind(d, DateTimeKind.Local))
                        : null,
                    msg,
                    threadId,
                    cStat,
                    ClassifySeverity(msg, cStat)));
            }
        }
        catch
        {
            return [];
        }

        return list;
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

    private static (int? ThreadId, string? CStat) ParseLogMessage(string? msg)
    {
        if (string.IsNullOrWhiteSpace(msg))
        {
            return (null, null);
        }

        int? threadId = null;
        var tm = Regex.Match(msg, @"\b[Tt]hread\s*[:=]?\s*(\d+)\b");
        if (tm.Success && int.TryParse(tm.Groups[1].Value, out var t))
        {
            threadId = t;
        }

        string? cStat = null;
        var cm = Regex.Match(msg, @"\bcStat\s*[:=]?\s*(\d+)\b", RegexOptions.IgnoreCase);
        if (cm.Success)
        {
            cStat = cm.Groups[1].Value;
        }

        return (threadId, cStat);
    }

    private static string ClassifySeverity(string? msg, string? cStat)
    {
        if (cStat is "118" or "117" or "146" or "730" or "992")
        {
            return "success";
        }

        if (string.IsNullOrWhiteSpace(msg))
        {
            return "info";
        }

        if (Regex.IsMatch(msg, @"\b(erro|error|exception|falha)\b", RegexOptions.IgnoreCase))
        {
            return "error";
        }

        if (Regex.IsMatch(msg, @"\b(warn|aviso)\b", RegexOptions.IgnoreCase))
        {
            return "warn";
        }

        return "info";
    }

    public static int ResolveIntervaloSeconds(IReadOnlyDictionary<string, string> configs)
    {
        if (!configs.TryGetValue("Intervalo", out var raw) || !int.TryParse(raw, out var n) || n <= 0)
        {
            return 60;
        }

        // Alguns ambientes gravam milissegundos.
        return n >= 1000 ? Math.Max(1, (int)Math.Round(n / 1000.0)) : n;
    }
}
