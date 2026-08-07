using System.Text.Json;
using CTe.Modules.Monitors.Abstractions;

namespace CTe.Modules.Monitors.Infrastructure;

/// <summary>
/// Agrega a validação estrita de fila dos 6 monitores para a visão da cadeia.
/// </summary>
public static class QueueProofAggregator
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<object> BuildAsync(IMonitorModuleRegistry registry, CancellationToken ct)
    {
        var services = new List<object>();
        var errors = new List<string>();
        var allClear = true;
        var allOk = true;
        long totalTemp = 0;
        long totalBroker = 0;
        long totalErrorRows = 0;
        var verifiedAt = DateTimeOffset.UtcNow;

        foreach (var id in DependencyInjection.KnownMonitorServiceIds)
        {
            var module = registry.Get(id);
            if (module is null)
            {
                allOk = false;
                allClear = false;
                errors.Add($"Serviço '{id}' não encontrado.");
                continue;
            }

            try
            {
                var raw = await module.GetQueueProofAsync(ct).ConfigureAwait(false);
                if (raw is null)
                {
                    allOk = false;
                    allClear = false;
                    errors.Add($"Sem validação disponível para '{id}'.");
                    continue;
                }

                var json = JsonSerializer.Serialize(raw);
                var proof = JsonSerializer.Deserialize<ProofDto>(json, JsonOptions);
                if (proof is null)
                {
                    allOk = false;
                    allClear = false;
                    errors.Add($"Resposta inválida para '{id}'.");
                    continue;
                }

                totalTemp += proof.TempCount;
                totalBroker += proof.BrokerCount;
                totalErrorRows += proof.TempErrorCount;
                if (!proof.Ok)
                {
                    allOk = false;
                    allClear = false;
                    if (proof.Errors is { Length: > 0 })
                    {
                        errors.AddRange(proof.Errors.Select(e => $"{id}: {e}"));
                    }
                    else
                    {
                        errors.Add($"{id}: validação falhou.");
                    }
                }
                else if (!proof.IsClear)
                {
                    allClear = false;
                }

                services.Add(new
                {
                    serviceId = proof.ServiceId ?? id,
                    domain = proof.Domain,
                    verifiedAtUtc = proof.VerifiedAtUtc,
                    tempTable = proof.TempTable,
                    brokerQueue = proof.BrokerQueue,
                    tempCount = proof.TempCount,
                    brokerCount = proof.BrokerCount,
                    tempErrorCount = proof.TempErrorCount,
                    isEmpty = proof.IsEmpty,
                    isClear = proof.IsClear,
                    ok = proof.Ok,
                    errors = proof.Errors ?? Array.Empty<string>()
                });
            }
            catch (Exception ex)
            {
                allOk = false;
                allClear = false;
                errors.Add($"{id}: {ex.Message}");
            }
        }

        var isEmpty = totalTemp == 0 && totalBroker == 0;
        return new
        {
            verifiedAtUtc = verifiedAt,
            tempCount = totalTemp,
            brokerCount = totalBroker,
            tempErrorCount = totalErrorRows,
            isEmpty,
            isClear = allClear && allOk && isEmpty && totalErrorRows == 0,
            ok = allOk,
            errors,
            services
        };
    }

    private sealed class ProofDto
    {
        public string? ServiceId { get; set; }
        public string? Domain { get; set; }
        public DateTimeOffset VerifiedAtUtc { get; set; }
        public string? TempTable { get; set; }
        public string? BrokerQueue { get; set; }
        public long TempCount { get; set; }
        public long BrokerCount { get; set; }
        public long TempErrorCount { get; set; }
        public bool IsEmpty { get; set; }
        public bool IsClear { get; set; }
        public bool Ok { get; set; }
        public string[]? Errors { get; set; }
    }
}
