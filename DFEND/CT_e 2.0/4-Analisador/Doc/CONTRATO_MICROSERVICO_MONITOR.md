# Contrato de microserviço — Monitor CT-e (Analisador)

Versão: **1.3** · Domínio: `analisador` · Consumidor: Orquestrador CT-e

Espelho do contrato compartilhado: [../../docs/CONTRATO_MICROSERVICO_MONITOR.md](../../docs/CONTRATO_MICROSERVICO_MONITOR.md).

## Identidade

| Campo | Valor |
|-------|--------|
| serviceId | `dfend-cte-monitor-analisador` |
| domain | `analisador` |
| monitoredService | `DFEND_CTe_Analisador` |
| CodServico | `6` |
| API / UI / Swagger | **5040** / **4240** / **7146** |

## Endpoints

Mesmo conjunto v1.3: `info`, `health`/`live`/`ready`, `service/status`, `snapshot`, `logs`, `tables/{key}`, `service/start|stop`, Hub `/hubs/monitor`.  
Aliases `/api/v1/monitor/*`. Auth: `X-Cte-Internal-Api-Key`.

## Estados

`Disabled` · `Offline` · `Starting` · `Running` · `Stopping` · `Stopped` · `Failed` · `Unknown`
