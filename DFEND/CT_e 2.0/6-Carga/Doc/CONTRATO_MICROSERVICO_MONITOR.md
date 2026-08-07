# Contrato de microserviço — Monitor CT-e (Carga)

Versão: **1.3** · Domínio: `carga`

Espelho: [../../docs/CONTRATO_MICROSERVICO_MONITOR.md](../../docs/CONTRATO_MICROSERVICO_MONITOR.md).

## Identidade

| Campo | Valor |
|-------|--------|
| serviceId | `dfend-cte-monitor-carga` |
| domain | `carga` |
| monitoredService | `DFEND_CTe_Carga` |
| CodServico | `99` |
| API / UI / Swagger | **5080** / **4260** / **7166** |

Negócio: download pontual por chave (`cteConsultaDFe`), não pipeline contínuo.  
Auth: `X-Cte-Internal-Api-Key`.
