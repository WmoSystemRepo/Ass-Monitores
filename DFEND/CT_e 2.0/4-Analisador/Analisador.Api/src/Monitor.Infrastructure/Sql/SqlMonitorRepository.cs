using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Monitor.Application.Abstractions;
using Monitor.Application.Services;
using Monitor.Domain.Alerts;
using Monitor.Domain.Models;

namespace Monitor.Infrastructure.Sql;

public sealed class SqlMonitorRepository : IMonitorReadRepository, IMonitorWriteRepository
{
    private readonly MonitorOptions _options;

    public static readonly string[] ProcessConfigKeys =
    [
        "Executar",
        "Intervalo",
        "Threads",
        "ReEnviarFila",
        "QtdeMaxFila",
        "LogBanco",
        "LogEvento",
        "LogCompleto"
    ];

    public SqlMonitorRepository(IOptions<MonitorOptions> options)
    {
        _options = options.Value;
    }

    private SqlConnection CreateConnection(string? connectionString = null)
    {
        var cs = connectionString ?? _options.ConnectionString;
        if (string.IsNullOrWhiteSpace(cs))
        {
            throw new InvalidOperationException(
                "ConnectionString DEV não configurada. Use User Secrets ou appsettings.Development.json.");
        }

        return new SqlConnection(cs);
    }

    private async Task<SqlConnection> OpenAsync(CancellationToken ct, string? connectionString = null)
    {
        var conn = CreateConnection(connectionString);
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        cts.CancelAfter(TimeSpan.FromSeconds(_options.SqlTimeoutSeconds));
        await conn.OpenAsync(cts.Token);
        return conn;
    }

    public async Task<bool> PingAsync(CancellationToken ct)
    {
        try
        {
            await using var conn = await OpenAsync(ct);
            await using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = _options.SqlTimeoutSeconds;
            cmd.CommandText = "SELECT 1";
            _ = await cmd.ExecuteScalarAsync(ct);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> PingAnaliticoAsync(CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(_options.ConnectionStringAnalitico))
        {
            return true;
        }

        try
        {
            await using var conn = await OpenAsync(ct, _options.ConnectionStringAnalitico);
            await using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = _options.SqlTimeoutSeconds;
            cmd.CommandText = "SELECT 1";
            _ = await cmd.ExecuteScalarAsync(ct);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<ServiceReadResult> GetServiceAsync(int codServico, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        await using var conn = await OpenAsync(ct);
        await using var cmd = conn.CreateCommand();
        cmd.CommandTimeout = _options.SqlTimeoutSeconds;
        cmd.CommandText = """
            SELECT TOP(1)
              des_servico,
              nom_servidor,
              dtc_execucao,
              CAST(NULL AS varchar(50)) AS num_sequencial_unico,
              dtc_atualizacao
            FROM cte.servico_sintetico_conhecimento_transporte_eletronico WITH (NOLOCK)
            WHERE cod_servico_sintetico_conhecimento_transporte_eletronico = @cod
            """;
        cmd.Parameters.AddWithValue("@cod", codServico);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (!await reader.ReadAsync(ct))
        {
            sw.Stop();
            return new ServiceReadResult(null, sw.ElapsedMilliseconds);
        }

        var row = new ServiceRow(
            reader["des_servico"] as string,
            reader["nom_servidor"] as string,
            reader["dtc_execucao"] is DateTime dtcExec
                ? new DateTimeOffset(DateTime.SpecifyKind(dtcExec, DateTimeKind.Local))
                : null,
            reader["num_sequencial_unico"]?.ToString(),
            reader["dtc_atualizacao"] is DateTime dtcUpd
                ? new DateTimeOffset(DateTime.SpecifyKind(dtcUpd, DateTimeKind.Local))
                : null);
        sw.Stop();
        return new ServiceReadResult(row, sw.ElapsedMilliseconds);
    }

    public async Task<ConfigReadResult> GetConfigsAsync(int codServico, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        await using var conn = await OpenAsync(ct);
        await using var cmd = conn.CreateCommand();
        cmd.CommandTimeout = _options.SqlTimeoutSeconds;
        cmd.CommandText = """
            SELECT c.des_configuracao, c.nom_configuracao, c.dtc_atualizacao
            FROM cte.configuracao_sintetico_conhecimento_transporte_eletronico c WITH (READPAST)
            WHERE c.sts_ativo = 1
              AND c.cod_servico_sintetico_conhecimento_transporte_eletronico = @cod
            """;
        cmd.Parameters.AddWithValue("@cod", codServico);

        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        DateTimeOffset? executarUpdated = null;
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            var key = reader["des_configuracao"]?.ToString() ?? string.Empty;
            var val = reader["nom_configuracao"]?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(key))
            {
                map[key] = val;
            }

            if (key.Equals("Executar", StringComparison.OrdinalIgnoreCase)
                && reader["dtc_atualizacao"] is DateTime dtc)
            {
                executarUpdated = new DateTimeOffset(DateTime.SpecifyKind(dtc, DateTimeKind.Local));
            }
        }

        sw.Stop();
        return new ConfigReadResult(map, executarUpdated, sw.ElapsedMilliseconds);
    }

    public async Task<ConfigDetailReadResult> GetProcessConfigsAsync(int codServico, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        await using var conn = await OpenAsync(ct);
        await using var cmd = conn.CreateCommand();
        cmd.CommandTimeout = _options.SqlTimeoutSeconds;
        cmd.CommandText = """
            SELECT c.des_configuracao, c.nom_configuracao, c.dtc_atualizacao
            FROM cte.configuracao_sintetico_conhecimento_transporte_eletronico c WITH (READPAST)
            WHERE c.sts_ativo = 1
              AND c.cod_servico_sintetico_conhecimento_transporte_eletronico = @cod
            """;
        cmd.Parameters.AddWithValue("@cod", codServico);

        var list = new List<ConfigDetailRow>();
        DateTimeOffset? executarUpdated = null;
        var executarValue = 0;
        var processSet = new HashSet<string>(ProcessConfigKeys, StringComparer.OrdinalIgnoreCase);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            var key = reader["des_configuracao"]?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(key) || !processSet.Contains(key))
            {
                continue;
            }

            var val = reader["nom_configuracao"]?.ToString() ?? string.Empty;
            DateTimeOffset? updated = reader["dtc_atualizacao"] is DateTime dtc
                ? new DateTimeOffset(DateTime.SpecifyKind(dtc, DateTimeKind.Local))
                : null;

            list.Add(new ConfigDetailRow(key, val, updated, true));

            if (key.Equals("Executar", StringComparison.OrdinalIgnoreCase))
            {
                executarUpdated = updated;
                _ = int.TryParse(val, out executarValue);
            }
        }

        sw.Stop();
        var ordered = ProcessConfigKeys
            .Select(k => list.FirstOrDefault(x => x.Key.Equals(k, StringComparison.OrdinalIgnoreCase)))
            .Where(x => x is not null)
            .Cast<ConfigDetailRow>()
            .ToList();

        return new ConfigDetailReadResult(ordered, executarUpdated, executarValue, sw.ElapsedMilliseconds);
    }

    public async Task<QueueReadResult> GetQueueCountsAsync(CancellationToken ct)
    {
        await using var conn = await OpenAsync(ct);

        long temp = 0;
        DateTimeOffset? oldest = null;
        var swTemp = Stopwatch.StartNew();
        await using (var cmd = conn.CreateCommand())
        {
            cmd.CommandTimeout = _options.SqlTimeoutSeconds;
            cmd.CommandText = """
                SELECT COUNT(1) AS total, MIN(dtc_atualizacao) AS oldest
                FROM cte.tmp_analise_conhecimento_transporte_eletronico WITH (READPAST)
                """;
            await using var reader = await cmd.ExecuteReaderAsync(ct);
            if (await reader.ReadAsync(ct))
            {
                temp = reader.GetInt32(0);
                if (!reader.IsDBNull(1))
                {
                    var dt = reader.GetDateTime(1);
                    oldest = new DateTimeOffset(DateTime.SpecifyKind(dt, DateTimeKind.Local));
                }
            }
        }

        swTemp.Stop();

        long broker = 0;
        var swBroker = Stopwatch.StartNew();
        broker = await TryCountQueueAsync(conn, "fila_alvo_cte_analisador", ct);
        swBroker.Stop();

        // Analisador: uma fila de entrada; sem fan-out de destino
        return new QueueReadResult(
            temp,
            broker,
            oldest,
            swTemp.ElapsedMilliseconds,
            swBroker.ElapsedMilliseconds,
            0,
            0,
            0);
    }

    private async Task<long> TryCountQueueAsync(SqlConnection conn, string tableName, CancellationToken ct)
    {
        try
        {
            await using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = _options.SqlTimeoutSeconds;
            cmd.CommandText = $"SELECT COUNT(1) AS total FROM {tableName} WITH (READPAST)";
            var result = await cmd.ExecuteScalarAsync(ct);
            return Convert.ToInt64(result);
        }
        catch
        {
            return 0;
        }
    }

    public async Task<DocumentsReadResult> GetRecentDocumentsAsync(int take, CancellationToken ct)
        => await ReadDocumentsAsync(null, take, ct);

    public async Task<DocumentsReadResult> GetDocumentsSinceAsync(
        DateTimeOffset? sessionStart,
        int take,
        CancellationToken ct)
        => await ReadDocumentsAsync(sessionStart, take, ct);

    private async Task<DocumentsReadResult> ReadDocumentsAsync(
        DateTimeOffset? sessionStart,
        int take,
        CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        await using var conn = await OpenAsync(ct);
        await using var cmd = conn.CreateCommand();
        cmd.CommandTimeout = _options.SqlTimeoutSeconds;
        cmd.CommandText = """
            SELECT TOP (@take)
              num_sequencial_unico,
              num_sequencial_unico_final,
              qtd_documento,
              des_mensagem_erro,
              dtc_autorizacao AS dtc_documento,
              dtc_atualizacao
            FROM cte.tmp_analise_conhecimento_transporte_eletronico WITH (READPAST)
            WHERE (@sessionStart IS NULL OR dtc_atualizacao >= @sessionStart)
            ORDER BY dtc_atualizacao DESC
            """;
        cmd.Parameters.AddWithValue("@take", take);
        cmd.Parameters.AddWithValue(
            "@sessionStart",
            sessionStart.HasValue ? sessionStart.Value.LocalDateTime : DBNull.Value);

        var list = new List<RecentDocument>();
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            var erro = reader["des_mensagem_erro"] as string;
            list.Add(new RecentDocument(
                Convert.ToInt64(reader["num_sequencial_unico"]),
                reader["num_sequencial_unico_final"] is DBNull
                    ? null
                    : Convert.ToInt64(reader["num_sequencial_unico_final"]),
                reader["qtd_documento"] is DBNull ? 0 : Convert.ToInt32(reader["qtd_documento"]),
                reader["dtc_documento"] is DateTime d1
                    ? new DateTimeOffset(DateTime.SpecifyKind(d1, DateTimeKind.Local))
                    : null,
                reader["dtc_atualizacao"] is DateTime d2
                    ? new DateTimeOffset(DateTime.SpecifyKind(d2, DateTimeKind.Local))
                    : null,
                erro,
                !string.IsNullOrWhiteSpace(erro)));
        }

        sw.Stop();
        return new DocumentsReadResult(list, sw.ElapsedMilliseconds);
    }

    public async Task<LogsReadResult> GetLogsAfterAsync(long afterSeq, int take, CancellationToken ct)
        => await ReadLogsAsync(afterSeq, null, take, ct);

    public async Task<LogsReadResult> GetLogsSinceAsync(
        DateTimeOffset? sessionStart,
        int take,
        CancellationToken ct)
        => await ReadLogsAsync(0, sessionStart, take, ct);

    private async Task<LogsReadResult> ReadLogsAsync(
        long afterSeq,
        DateTimeOffset? sessionStart,
        int take,
        CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        await using var conn = await OpenAsync(ct);

        var list = await TryReadLogsAsync(
            conn,
            afterSeq,
            sessionStart,
            take,
            filterByCodServico: true,
            useDesLog: true,
            ct);

        if (list is null)
        {
            list = await TryReadLogsAsync(
                conn,
                afterSeq,
                sessionStart,
                take,
                filterByCodServico: false,
                useDesLog: true,
                ct);
        }

        if (list is null)
        {
            list = await TryReadLogsAsync(
                conn,
                afterSeq,
                sessionStart,
                take,
                filterByCodServico: true,
                useDesLog: false,
                ct) ?? await TryReadLogsAsync(
                conn,
                afterSeq,
                sessionStart,
                take,
                filterByCodServico: false,
                useDesLog: false,
                ct) ?? [];
        }

        sw.Stop();
        return new LogsReadResult(list.OrderBy(l => l.SeqLog).ToList(), sw.ElapsedMilliseconds);
    }

    private async Task<List<LogEntry>?> TryReadLogsAsync(
        SqlConnection conn,
        long afterSeq,
        DateTimeOffset? sessionStart,
        int take,
        bool filterByCodServico,
        bool useDesLog,
        CancellationToken ct)
    {
        try
        {
            await using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = _options.SqlTimeoutSeconds;

            var msgCol = useDesLog ? "l.des_log AS des_log" : "l.des_mensagem AS des_log";
            var timeCol = useDesLog ? "l.dtc_insercao" : "l.dtc_atualizacao";
            var codFilter = filterByCodServico
                ? "AND l.cod_servico_sintetico_conhecimento_transporte_eletronico = @cod"
                : string.Empty;

            cmd.CommandText = $"""
                SELECT TOP (@take)
                  l.seq_log_sintetico_conhecimento_transporte_eletronico AS seq_log,
                  {timeCol} AS dtc_log,
                  {msgCol}
                FROM cte.log_sintetico_conhecimento_transporte_eletronico l WITH (READPAST)
                WHERE l.seq_log_sintetico_conhecimento_transporte_eletronico > @after
                  AND (@sessionStart IS NULL OR {timeCol} >= @sessionStart)
                  {codFilter}
                ORDER BY l.seq_log_sintetico_conhecimento_transporte_eletronico DESC
                """;
            cmd.Parameters.AddWithValue("@take", take);
            cmd.Parameters.AddWithValue("@after", afterSeq);
            cmd.Parameters.AddWithValue(
                "@sessionStart",
                sessionStart.HasValue ? sessionStart.Value.LocalDateTime : DBNull.Value);
            if (filterByCodServico)
            {
                cmd.Parameters.AddWithValue("@cod", _options.CodServicoAnalisador);
            }

            var list = new List<LogEntry>();
            await using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                list.Add(MapLog(reader));
            }

            return list;
        }
        catch (SqlException)
        {
            return null;
        }
    }

    private static LogEntry MapLog(SqlDataReader reader)
    {
        var msg = reader["des_log"] as string;
        var (threadId, cStat) = AlertEngine.ParseLogMessage(msg);
        return new LogEntry(
            Convert.ToInt64(reader["seq_log"]),
            reader["dtc_log"] is DateTime d
                ? new DateTimeOffset(DateTime.SpecifyKind(d, DateTimeKind.Local))
                : null,
            msg,
            threadId,
            cStat,
            ClassifyLogSeverity(msg, cStat));
    }

    private static string ClassifyLogSeverity(string? msg, string? cStat)
    {
        if (cStat is "118" or "117" or "146" or "730" or "992")
        {
            return "success";
        }

        if (cStat is "108" or "285")
        {
            return "warning";
        }

        if (string.IsNullOrWhiteSpace(msg))
        {
            return "info";
        }

        if (msg.Contains("lote já existente", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("lote ja existente", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("sintetiz", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("exclu", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("chave retirada", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("lote obtido", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("doc atualizado", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("doc excluido", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("doc excluído", StringComparison.OrdinalIgnoreCase))
        {
            return "success";
        }

        if (msg.Contains("exception", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("stack trace", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("falha", StringComparison.OrdinalIgnoreCase)
            || ContainsStandaloneErro(msg))
        {
            return "error";
        }

        if (msg.Contains("sucesso", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("ok", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("gravado", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("recebido", StringComparison.OrdinalIgnoreCase))
        {
            return "success";
        }

        return "info";
    }

    private static bool ContainsStandaloneErro(string msg)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(
            msg,
            @"\b(erro|error|errors)\b",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
            | System.Text.RegularExpressions.RegexOptions.CultureInvariant);
    }

    public async Task SetExecutarAsync(int codServico, int value, CancellationToken ct)
    {
        await using var conn = await OpenAsync(ct);
        await using var cmd = conn.CreateCommand();
        cmd.CommandTimeout = _options.SqlTimeoutSeconds;
        cmd.CommandText = """
            UPDATE cte.configuracao_sintetico_conhecimento_transporte_eletronico
            SET nom_configuracao = @valor,
                dtc_atualizacao = GETDATE()
            WHERE cod_servico_sintetico_conhecimento_transporte_eletronico = @cod
              AND des_configuracao = 'Executar'
              AND sts_ativo = 1
            """;
        cmd.Parameters.AddWithValue("@valor", value.ToString());
        cmd.Parameters.AddWithValue("@cod", codServico);
        await cmd.ExecuteNonQueryAsync(ct);
    }
}
