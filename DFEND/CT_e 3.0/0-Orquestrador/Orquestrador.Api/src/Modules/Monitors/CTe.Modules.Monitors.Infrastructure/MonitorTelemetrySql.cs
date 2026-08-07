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
        DateTimeOffset? DtcAtualizacao,
        string? MainNsu);

    private static readonly string[] ProcessConfigKeys =
    [
        "Executar",
        "Intervalo",
        "Threads",
        "PacoteCompleto",
        "ReBuscar",
        "NSUAux",
        "NSUAuxAut",
        "NSUAuxDest",
        "WSURL",
        "WSTimeOut",
        "WSVersao",
        "WSTipoAmbiente",
        "LogBanco",
        "LogEvento",
        "LogCompleto"
    ];

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

    /// <summary>
    /// Detalhe de tabela (servico/configuracao/temporaria/log/fila) no shape do TableDetailDto do front.
    /// Sem connection string ainda devolve payload vazio estruturado (não 404).
    /// </summary>
    public static async Task<object?> ReadTableDetailAsync(
        string? connectionString,
        string domain,
        int codServico,
        string displayName,
        string key,
        int take,
        int timeoutSeconds,
        ILogger logger,
        CancellationToken ct)
    {
        key = (key ?? string.Empty).Trim().ToLowerInvariant();
        take = Math.Clamp(take, 1, 1000);

        var meta = ResolveTableMeta(key, domain);
        if (meta is null)
        {
            return null;
        }

        var sw = System.Diagnostics.Stopwatch.StartNew();
        IReadOnlyDictionary<string, string> configs =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        ServiceTelemetry? service = null;
        long tempBacklog = 0;
        long brokerDepth = 0;
        IReadOnlyList<object> docs = Array.Empty<object>();
        IReadOnlyList<object> logs = Array.Empty<object>();
        IReadOnlyList<object> configRows = Array.Empty<object>();
        var hasSql = !string.IsNullOrWhiteSpace(connectionString) && codServico > 0;

        if (hasSql)
        {
            try
            {
                await using var conn = new SqlConnection(connectionString);
                using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct);
                linked.CancelAfter(TimeSpan.FromSeconds(Math.Max(3, timeoutSeconds * 3)));
                await conn.OpenAsync(linked.Token);

                var schema = ResolveSchema(domain);
                configs = await ReadConfigsAsync(conn, schema, codServico, timeoutSeconds, linked.Token);
                service = await ReadServiceAsync(conn, schema, codServico, timeoutSeconds, linked.Token);
                (tempBacklog, brokerDepth, _) = await ReadQueuesAsync(conn, domain, timeoutSeconds, linked.Token);

                switch (key)
                {
                    case "servico":
                    case "log":
                        logs = await ReadLogsAsync(
                            conn, schema, afterSeq: 0, take, timeoutSeconds, linked.Token);
                        break;
                    case "temporaria":
                        docs = await ReadRecentDocsAsync(
                            conn, domain, timeoutSeconds, linked.Token, take);
                        break;
                    case "configuracao":
                        configRows = await ReadConfigDetailRowsAsync(
                            conn, schema, codServico, timeoutSeconds, linked.Token);
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Detalhe de tabela {Key} falhou para domain={Domain}", key, domain);
            }
        }

        sw.Stop();
        var queryMs = (int)Math.Min(int.MaxValue, sw.ElapsedMilliseconds);

        int? executar = null;
        if (configs.TryGetValue("Executar", out var exRaw) && int.TryParse(exRaw, out var executarVal))
        {
            executar = executarVal;
        }

        var receptionOn = executar is 1;
        var banner = receptionOn
            ? null
            : "Recepção desligada — mostrando dados da última sessão (desde a última alteração de Executar).";

        var ageSeconds = service?.DtcExecucao is DateTimeOffset batida
            ? (int?)Math.Max(0, (DateTimeOffset.UtcNow - batida.ToUniversalTime()).TotalSeconds)
            : null;

        object? serviceRows = null;
        object? tempRows = null;
        object? logRows = null;
        object? fila = null;
        object? contextLogs = null;
        var primaryValue = meta.Value.EmptyPrimary;
        var status = hasSql ? "Ok" : "Atencao";
        var hint = hasSql ? meta.Value.HintOk : "Sem connection string — não foi possível ler o SQL.";
        var rowCount = 0;

        switch (key)
        {
            case "servico":
            {
                if (service is not null)
                {
                    serviceRows = new[]
                    {
                        new
                        {
                            desServico = service.DesServico ?? displayName,
                            nomServidor = service.NomServidor,
                            nsu = service.MainNsu,
                            dtcExecucao = service.DtcExecucao,
                            dtcAtualizacao = service.DtcAtualizacao
                        }
                    };
                    rowCount = 1;
                    primaryValue = string.IsNullOrWhiteSpace(service.MainNsu)
                        ? "Sem NSU"
                        : $"NSU {service.MainNsu}";
                }
                else
                {
                    serviceRows = Array.Empty<object>();
                    status = hasSql ? "Atencao" : status;
                    hint = hasSql ? "Sem linha de serviço no SQL." : hint;
                }

                contextLogs = logs
                    .OfType<object>()
                    .Where(IsNsuOrCStatLog)
                    .Take(take)
                    .ToList();
                break;
            }
            case "configuracao":
            {
                if (configRows.Count == 0 && configs.Count > 0)
                {
                    configRows = ProcessConfigKeys
                        .Where(k => configs.ContainsKey(k))
                        .Select(k => (object)new
                        {
                            key = k,
                            value = configs[k],
                            dtcAtualizacao = (DateTimeOffset?)null,
                            isProcessKey = true
                        })
                        .ToList();
                }

                rowCount = configRows.Count;
                primaryValue = rowCount == 0 ? "Sem configs" : $"{rowCount} chave(s)";
                if (rowCount == 0 && hasSql)
                {
                    status = "Atencao";
                    hint = "Nenhuma configuração de processo encontrada.";
                }

                break;
            }
            case "temporaria":
            {
                tempRows = docs;
                rowCount = docs.Count;
                primaryValue = tempBacklog > 0
                    ? $"{tempBacklog} na temporária"
                    : (rowCount == 0 ? "Vazia" : $"{rowCount} doc(s)");
                break;
            }
            case "log":
            {
                logRows = logs.Reverse().Take(take).ToList();
                rowCount = ((IReadOnlyList<object>)logRows).Count;
                primaryValue = rowCount == 0 ? "Sem eventos" : $"{rowCount} evento(s)";
                break;
            }
            case "fila":
            {
                fila = new
                {
                    depth = brokerDepth,
                    depthTrend = Array.Empty<long>(),
                    highThreshold = 50,
                    trendHint = brokerDepth == 0
                        ? "Fila vazia."
                        : $"{brokerDepth} item(ns) aguardando."
                };
                rowCount = 1;
                primaryValue = brokerDepth == 0 ? "Fila vazia" : $"Profundidade {brokerDepth}";
                break;
            }
        }

        if (ageSeconds is > 7200 && key is "servico")
        {
            status = "Atencao";
            hint = "Última batida desatualizada (>2h).";
        }

        return new
        {
            key,
            label = meta.Value.Label,
            sessionStartUtc = (DateTimeOffset?)null,
            receptionOn,
            bannerMessage = banner,
            health = new
            {
                key,
                label = meta.Value.Label,
                status,
                primaryValue,
                dataAgeSeconds = ageSeconds,
                queryMs,
                hint,
                route = $"/tabelas/{key}"
            },
            serviceRows,
            configRows = key == "configuracao" ? configRows : null,
            tempRows,
            logRows,
            fila,
            contextLogs,
            takeApplied = take,
            rowCount
        };
    }

    private static (string Label, string EmptyPrimary, string HintOk)? ResolveTableMeta(string key, string domain)
    {
        var filaLabel = domain.Trim().ToLowerInvariant() switch
        {
            "receptor" => "Fila Arquivador",
            "arquivador" => "Fila Sintetizador",
            "sintetizador" => "Fila Analisador",
            "analisador" => "Fila Integrador",
            "integrador" or "carga" => "Fila saída",
            _ => "Fila"
        };

        return key switch
        {
            "servico" => ("Serviço (NSU)", "Sem linha de serviço", "Linha de serviço / posição NSU."),
            "configuracao" => ("Configuração", "Sem configs", "Chaves de processo ativas."),
            "temporaria" => ("Temporária", "Vazia", "Documentos na tabela temporária."),
            "log" => ("Log", "Sem eventos", "Eventos recentes da sessão."),
            "fila" => (filaLabel, "Fila vazia", "Profundidade da fila Service Broker."),
            _ => null
        };
    }

    private static bool IsNsuOrCStatLog(object log)
    {
        // Anonymous log shape: mensagem / cStat via reflection-free ToString check is fragile;
        // use dynamic dictionary-like via pattern on known anon type properties.
        var type = log.GetType();
        var cStatProp = type.GetProperty("cStat");
        var msgProp = type.GetProperty("mensagem");
        var cStat = cStatProp?.GetValue(log) as string;
        if (!string.IsNullOrEmpty(cStat))
        {
            return true;
        }

        var msg = msgProp?.GetValue(log) as string;
        return msg?.Contains("NSU", StringComparison.OrdinalIgnoreCase) == true
            || msg?.Contains("nsu", StringComparison.OrdinalIgnoreCase) == true;
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

    /// <summary>
    /// Contagem estrita (sem READPAST/NOLOCK) para validar fila/temporária vazia sob demanda.
    /// </summary>
    internal sealed record QueueProofResult(
        string ServiceId,
        string Domain,
        DateTimeOffset VerifiedAtUtc,
        string TempTable,
        string? BrokerQueue,
        long TempCount,
        long BrokerCount,
        long TempErrorCount,
        bool IsEmpty,
        bool IsClear,
        bool Ok,
        IReadOnlyList<string> Errors);

    public static async Task<QueueProofResult> ReadQueueProofAsync(
        string? connectionString,
        string serviceId,
        string domain,
        int timeoutSeconds,
        ILogger logger,
        CancellationToken ct)
    {
        var (tempTable, brokerQueue) = ResolveTempAndBroker(domain);
        var verifiedAt = DateTimeOffset.UtcNow;
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return new QueueProofResult(
                serviceId,
                domain,
                verifiedAt,
                tempTable,
                brokerQueue,
                TempCount: 0,
                BrokerCount: 0,
                TempErrorCount: 0,
                IsEmpty: false,
                IsClear: false,
                Ok: false,
                Errors: ["ConnectionString vazio — não é possível validar a fila."]);
        }

        try
        {
            await using var conn = new SqlConnection(connectionString);
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct);
            linked.CancelAfter(TimeSpan.FromSeconds(Math.Max(3, timeoutSeconds * 3)));
            await conn.OpenAsync(linked.Token);

            // Integrador/Carga: temp sem des_mensagem_erro (paridade CT_e 2.0).
            var errorColumn = ResolveTempErrorColumn(domain);
            var tempSql = errorColumn is null
                ? $"""
                    SELECT COUNT(1) AS total, CAST(0 AS bigint) AS erros
                    FROM cte.{tempTable}
                    """
                : $"""
                    SELECT COUNT(1) AS total,
                           SUM(CASE WHEN NULLIF(LTRIM(RTRIM({errorColumn})), '') IS NOT NULL THEN 1 ELSE 0 END) AS erros
                    FROM cte.{tempTable}
                    """;

            long tempCount = 0;
            long tempErrorCount = 0;
            await using (var cmd = conn.CreateCommand())
            {
                cmd.CommandTimeout = Math.Max(1, timeoutSeconds);
                cmd.CommandText = tempSql;
                await using var reader = await cmd.ExecuteReaderAsync(linked.Token);
                if (await reader.ReadAsync(linked.Token))
                {
                    tempCount = reader.IsDBNull(0) ? 0 : Convert.ToInt64(reader.GetValue(0));
                    tempErrorCount = reader.IsDBNull(1) ? 0 : Convert.ToInt64(reader.GetValue(1));
                }
            }

            long brokerCount = 0;
            if (!string.IsNullOrWhiteSpace(brokerQueue))
            {
                var brokerSql = $"""
                    SELECT COUNT(1)
                    FROM {brokerQueue}
                    """;
                await using var cmd = conn.CreateCommand();
                cmd.CommandTimeout = Math.Max(1, timeoutSeconds);
                cmd.CommandText = brokerSql;
                var raw = await cmd.ExecuteScalarAsync(linked.Token);
                brokerCount = raw is null or DBNull ? 0 : Convert.ToInt64(raw);
            }

            var isEmpty = tempCount == 0 && brokerCount == 0;
            var isClear = isEmpty && tempErrorCount == 0;
            return new QueueProofResult(
                serviceId,
                domain,
                verifiedAt,
                tempTable,
                brokerQueue,
                tempCount,
                brokerCount,
                tempErrorCount,
                isEmpty,
                isClear,
                Ok: true,
                Errors: Array.Empty<string>());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Validação estrita de fila falhou para domain={Domain}", domain);
            // Não propaga: a cadeia agrega por serviço; um domínio quebrado não derruba o HTTP.
            return new QueueProofResult(
                serviceId,
                domain,
                verifiedAt,
                tempTable,
                brokerQueue,
                TempCount: 0,
                BrokerCount: 0,
                TempErrorCount: 0,
                IsEmpty: false,
                IsClear: false,
                Ok: false,
                Errors: [ex.Message]);
        }
    }

    /// <summary>
    /// Coluna de erro na temp — Integrador/Carga não têm <c>des_mensagem_erro</c>.
    /// </summary>
    private static string? ResolveTempErrorColumn(string domain) =>
        domain.Trim().ToLowerInvariant() switch
        {
            "integrador" or "carga" => null,
            _ => "des_mensagem_erro"
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
                  dtc_atualizacao,
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
                reader["dtc_atualizacao"] is DateTime dtcUpd
                    ? new DateTimeOffset(DateTime.SpecifyKind(dtcUpd, DateTimeKind.Local))
                    : null,
                reader["num_sequencial_unico"]?.ToString());
        }
        catch
        {
            return null;
        }
    }

    private static async Task<IReadOnlyList<object>> ReadConfigDetailRowsAsync(
        SqlConnection conn,
        Schema schema,
        int codServico,
        int timeoutSeconds,
        CancellationToken ct)
    {
        var processSet = new HashSet<string>(ProcessConfigKeys, StringComparer.OrdinalIgnoreCase);
        var found = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        try
        {
            await using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = Math.Max(1, timeoutSeconds);
            cmd.CommandText = $"""
                SELECT c.des_configuracao, c.nom_configuracao, c.dtc_atualizacao
                FROM cte.{schema.ConfigTable} c WITH (READPAST)
                WHERE c.sts_ativo = 1
                  AND c.{schema.ConfigCodColumn} = @cod
                """;
            cmd.Parameters.AddWithValue("@cod", codServico);
            await using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                var key = reader["des_configuracao"]?.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(key) || !processSet.Contains(key))
                {
                    continue;
                }

                found[key] = new
                {
                    key,
                    value = reader["nom_configuracao"]?.ToString() ?? string.Empty,
                    dtcAtualizacao = reader["dtc_atualizacao"] is DateTime dtc
                        ? new DateTimeOffset(DateTime.SpecifyKind(dtc, DateTimeKind.Local))
                        : (DateTimeOffset?)null,
                    isProcessKey = true
                };
            }
        }
        catch
        {
            return Array.Empty<object>();
        }

        return ProcessConfigKeys
            .Where(found.ContainsKey)
            .Select(k => found[k])
            .ToList();
    }

    private static async Task<IReadOnlyList<object>> ReadRecentDocsAsync(
        SqlConnection conn,
        string domain,
        int timeoutSeconds,
        CancellationToken ct,
        int take = 12)
    {
        var (table, _) = ResolveTempAndBroker(domain);
        take = Math.Clamp(take, 1, 1000);

        var list = new List<object>();
        try
        {
            await using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = Math.Max(1, timeoutSeconds);
            cmd.CommandText = $"""
                SELECT TOP (@take)
                  num_sequencial_unico AS nsu,
                  num_sequencial_unico_final AS nsuFinal,
                  qtd_documento AS qtdDocumento,
                  des_mensagem_erro AS mensagemErro,
                  dtc_documento AS dtcDocumento,
                  dtc_atualizacao AS dtcAtualizacao
                FROM cte.{table} WITH (READPAST)
                ORDER BY dtc_atualizacao DESC
                """;
            cmd.Parameters.AddWithValue("@take", take);
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
