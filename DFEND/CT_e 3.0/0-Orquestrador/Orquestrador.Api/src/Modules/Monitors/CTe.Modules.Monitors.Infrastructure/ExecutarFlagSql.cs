using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace CTe.Modules.Monitors.Infrastructure;

/// <summary>
/// Liga/desliga o trabalho das filas (flag SQL <c>Executar</c>) — mesma regra dos Monitor.Api CT_e 2.0.
/// Domínios de recepção (receptor/arquivador/carga) e sintético (sintetizador/analisador/integrador).
/// </summary>
internal static class ExecutarFlagSql
{
    public static async Task<(bool Ok, string? Error)> SetAsync(
        string? connectionString,
        string domain,
        int codServico,
        int value,
        int timeoutSeconds,
        ILogger logger,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return (false, "ConnectionString vazio — Executar não gravado no SQL.");
        }

        if (codServico <= 0)
        {
            return (false, "CodServico inválido — Executar não gravado.");
        }

        if (!TryResolveSchema(domain, out var table, out var codColumn))
        {
            return (false, $"Domínio '{domain}' sem mapeamento de tabela Executar.");
        }

        try
        {
            await using var conn = new SqlConnection(connectionString);
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct);
            linked.CancelAfter(TimeSpan.FromSeconds(Math.Max(1, timeoutSeconds)));
            await conn.OpenAsync(linked.Token);
            await using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = Math.Max(1, timeoutSeconds);
            cmd.CommandText = $"""
                UPDATE cte.{table}
                SET nom_configuracao = @valor,
                    dtc_atualizacao = GETDATE()
                WHERE {codColumn} = @cod
                  AND des_configuracao = 'Executar'
                  AND sts_ativo = 1
                """;
            cmd.Parameters.AddWithValue("@valor", value.ToString());
            cmd.Parameters.AddWithValue("@cod", codServico);
            var rows = await cmd.ExecuteNonQueryAsync(linked.Token);
            if (rows <= 0)
            {
                logger.LogWarning(
                    "Executar={Value} não atualizou linhas (domain={Domain}, cod={Cod})",
                    value,
                    domain,
                    codServico);
                return (false, "Nenhuma linha Executar atualizada no SQL.");
            }

            return (true, null);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Falha ao gravar Executar={Value} para {Domain}", value, domain);
            return (false, ex.Message);
        }
    }

    public static async Task<int?> TryGetAsync(
        string? connectionString,
        string domain,
        int codServico,
        int timeoutSeconds,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(connectionString) || codServico <= 0)
        {
            return null;
        }

        if (!TryResolveSchema(domain, out var table, out var codColumn))
        {
            return null;
        }

        try
        {
            await using var conn = new SqlConnection(connectionString);
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct);
            linked.CancelAfter(TimeSpan.FromSeconds(Math.Max(1, timeoutSeconds)));
            await conn.OpenAsync(linked.Token);
            await using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = Math.Max(1, timeoutSeconds);
            cmd.CommandText = $"""
                SELECT TOP (1) nom_configuracao
                FROM cte.{table}
                WHERE {codColumn} = @cod
                  AND des_configuracao = 'Executar'
                  AND sts_ativo = 1
                """;
            cmd.Parameters.AddWithValue("@cod", codServico);
            var raw = await cmd.ExecuteScalarAsync(linked.Token);
            if (raw is null or DBNull)
            {
                return null;
            }

            return int.TryParse(raw.ToString(), out var n) ? n : null;
        }
        catch
        {
            return null;
        }
    }

    private static bool TryResolveSchema(string domain, out string table, out string codColumn)
    {
        switch (domain.Trim().ToLowerInvariant())
        {
            case "receptor":
            case "arquivador":
            case "carga":
                table = "configuracao_recepcao_conhecimento_transporte_eletronico";
                codColumn = "cod_servico_recepcao_conhecimento_transporte_eletronico";
                return true;
            case "sintetizador":
            case "analisador":
            case "integrador":
                table = "configuracao_sintetico_conhecimento_transporte_eletronico";
                codColumn = "cod_servico_sintetico_conhecimento_transporte_eletronico";
                return true;
            default:
                table = string.Empty;
                codColumn = string.Empty;
                return false;
        }
    }
}
