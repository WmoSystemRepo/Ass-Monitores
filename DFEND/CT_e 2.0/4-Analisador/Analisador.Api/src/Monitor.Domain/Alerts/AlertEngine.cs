using System.Text.RegularExpressions;
using Monitor.Domain.Models;

namespace Monitor.Domain.Alerts;

public static class AlertEngine
{
    private static readonly Regex ThreadRegex = new(@"Thread\s*(\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex CStatRegex = new(@"\b(108|117|118|146|285|730|992)\b", RegexOptions.Compiled);

    public static (int? ThreadId, string? CStat) ParseLogMessage(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return (null, null);
        }

        int? threadId = null;
        var tm = ThreadRegex.Match(message);
        if (tm.Success && int.TryParse(tm.Groups[1].Value, out var tid))
        {
            threadId = tid;
        }

        string? cStat = null;
        var cm = CStatRegex.Match(message);
        if (cm.Success)
        {
            cStat = cm.Value;
        }

        return (threadId, cStat);
    }

    public static IReadOnlyList<MonitorAlert> Evaluate(
        ServiceStatusView service,
        QueueStats queues,
        IReadOnlyList<RecentDocument> recentDocuments,
        IReadOnlyList<LogEntry> recentLogs,
        int intervaloSeconds,
        AlertThresholdOptions thresholds,
        ConnectionHealth health,
        string? connectionError,
        DateTimeOffset nowUtc)
    {
        var alerts = new List<MonitorAlert>();

        if (health == ConnectionHealth.Down)
        {
            alerts.Add(new MonitorAlert(
                "BD_DOWN",
                AlertSeverity.Critico,
                connectionError ?? "Falha de conexão / timeout SQL DEV.",
                nowUtc));
            return alerts;
        }

        if (service.Executar == 0)
        {
            alerts.Add(new MonitorAlert(
                "SVC_EXECUTAR_OFF",
                AlertSeverity.Atenção,
                "análise ociosa (RN-001)",
                nowUtc));
        }

        var staleSeconds = Math.Max(
            thresholds.StaleMinutesFloor * 60,
            thresholds.StaleIntervalMultiplier * Math.Max(intervaloSeconds, 1));

        if (service.Executar == 1 && service.DtcExecucao is { } dtc)
        {
            var age = nowUtc - dtc.ToUniversalTime();
            if (age.TotalSeconds > staleSeconds)
            {
                alerts.Add(new MonitorAlert(
                    "SVC_STALE",
                    AlertSeverity.Critico,
                    $"dtc_execucao antigo ({age.TotalMinutes:0} min) com Executar=1.",
                    nowUtc));
            }
        }

        if (service.Executar == 1)
        {
            var lastLog = recentLogs.OrderByDescending(l => l.SeqLog).FirstOrDefault();
            if (lastLog?.DtcLog is { } logAt)
            {
                var silence = nowUtc - logAt.ToUniversalTime();
                if (silence.TotalSeconds > staleSeconds)
                {
                    alerts.Add(new MonitorAlert(
                        "LOG_SILENCE",
                        AlertSeverity.Critico,
                        $"Sem log novo há {silence.TotalMinutes:0} min com Executar=1.",
                        nowUtc));
                }
            }
            else if (recentLogs.Count == 0)
            {
                alerts.Add(new MonitorAlert(
                    "LOG_SILENCE",
                    AlertSeverity.Critico,
                    "Nenhum log recente com Executar=1.",
                    nowUtc));
            }
        }

        if (queues.ServiceBrokerDepth >= thresholds.FilaAlta)
        {
            alerts.Add(new MonitorAlert(
                "FILA_ALTA",
                AlertSeverity.Atenção,
                $"Fila Analisador ≥ {thresholds.FilaAlta} (atual: {queues.ServiceBrokerDepth}).",
                nowUtc));
        }

        var trend = queues.BrokerDepthTrend;
        if (trend.Count >= thresholds.FilaCrescendoSnapshots
            && trend[^1] >= thresholds.FilaCrescendoMinDepth)
        {
            var growing = true;
            for (var i = trend.Count - thresholds.FilaCrescendoSnapshots + 1; i < trend.Count; i++)
            {
                if (trend[i] <= trend[i - 1])
                {
                    growing = false;
                    break;
                }
            }

            if (growing)
            {
                alerts.Add(new MonitorAlert(
                    "FILA_CRESCENDO",
                    AlertSeverity.Alerta,
                    $"Fila Analisador crescendo em {thresholds.FilaCrescendoSnapshots} snapshots (atual: {queues.ServiceBrokerDepth}).",
                    nowUtc));
            }
        }

        var window = recentDocuments.Take(thresholds.TmpErroWindowSize).ToList();
        if (window.Any(d => d.HasError))
        {
            alerts.Add(new MonitorAlert(
                "TMP_ERRO",
                AlertSeverity.Alerta,
                "Há des_mensagem_erro nos lotes recentes de tmp_analise.",
                nowUtc));
            alerts.Add(new MonitorAlert(
                "TEMP_COM_ERRO",
                AlertSeverity.Alerta,
                "Temporária com des_mensagem_erro (alias TMP_ERRO).",
                nowUtc));
        }

        foreach (var log in recentLogs.Take(30))
        {
            var msg = log.Mensagem ?? string.Empty;
            var upper = msg.ToUpperInvariant();

            if (upper.Contains("PRIMARY KEY", StringComparison.Ordinal)
                || upper.Contains("DUPLICATE KEY", StringComparison.Ordinal)
                || upper.Contains("LOTE JÁ EXISTENTE", StringComparison.Ordinal)
                || upper.Contains("LOTE JA EXISTENTE", StringComparison.Ordinal)
                || upper.Contains("DOC JA EXISTENTE", StringComparison.Ordinal)
                || upper.Contains("DOC JÁ EXISTENTE", StringComparison.Ordinal))
            {
                alerts.Add(new MonitorAlert(
                    "PK_DUPLICADA",
                    AlertSeverity.Atenção,
                    "Detectado documento/lote já existente / PK.",
                    nowUtc));
            }

            if (upper.Contains("DADO NÃO OBTIDO", StringComparison.Ordinal)
                || upper.Contains("DADO NAO OBTIDO", StringComparison.Ordinal)
                || upper.Contains("MSGDADONAOOBTIDO", StringComparison.Ordinal))
            {
                alerts.Add(new MonitorAlert(
                    "NSU_SEM_TEMP",
                    AlertSeverity.Alerta,
                    "NSU sem temp correspondente (dado não obtido).",
                    nowUtc));
            }

            if (upper.Contains("NSU FALTANTE", StringComparison.Ordinal)
                || upper.Contains("NSU_FALTANTE", StringComparison.Ordinal)
                || upper.Contains("FURO", StringComparison.Ordinal) && upper.Contains("NSU", StringComparison.Ordinal))
            {
                alerts.Add(new MonitorAlert(
                    "NSU_FALTANTE",
                    AlertSeverity.Alerta,
                    "Furo de NSU detectado nos logs.",
                    nowUtc));
            }

            if (upper.Contains("REENVIARFILA", StringComparison.Ordinal)
                || msg.Contains("ReEnviarFila", StringComparison.OrdinalIgnoreCase))
            {
                alerts.Add(new MonitorAlert(
                    "REENVIAR_FILA",
                    AlertSeverity.Info,
                    "ReEnviarFila mencionado nos logs recentes.",
                    nowUtc));
            }
        }

        return alerts
            .GroupBy(a => a.Code)
            .Select(g => g.OrderByDescending(a => a.Severity).First())
            .OrderByDescending(a => a.Severity)
            .ToList();
    }
}
