# Contrato de microserviço — Monitor CT-e (Receptor)

Versão: **1.3** · Domínio: `receptor`

Espelho: [../../docs/CONTRATO_MICROSERVICO_MONITOR.md](../../docs/CONTRATO_MICROSERVICO_MONITOR.md).

## Identidade

| Campo | Valor |
|-------|--------|
| serviceId | `dfend-cte-monitor-receptor` |
| domain | `receptor` |
| monitoredService | `DFEND_CTe_Receptor` |
| CodServico | `2` |
| API / UI / Swagger | **5010** / **4200** / **7116** |

## Endpoints v1.3

`info`, `health`/`live`/`ready`, `service/status`, `snapshot`, `logs`, `tables/{key}` (servico · configuracao · temporaria · log · fila), `service/start|stop`, Hub `/hubs/monitor`.  
Auth: `X-Cte-Internal-Api-Key`.
