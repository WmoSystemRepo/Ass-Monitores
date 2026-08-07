# Contrato de microserviço — Monitor CT-e (compartilhado)

Versão do contrato: **1.3**  
Consumidor: **Orquestrador CT-e** (`0-Orquestrador` em CT_e 2.0 ou 3.0)

> Cópia canônica da cadeia 2.0. Cada módulo pode ter espelho em `Doc/CONTRATO_MICROSERVICO_MONITOR.md` com identidade do domínio.

Onboarding: [CT_e 3.0 Doc/ONBOARDING_MICROSERVICO.md](../../CT_e%203.0/0-Orquestrador/Doc/ONBOARDING_MICROSERVICO.md).

## Ideia

O Orquestrador **não conhece** a lógica fiscal. Fala apenas HTTP:

- Health (live / ready)
- Status operacional
- Snapshot / métricas
- Start / Stop do worker (via Monitor → DevHost / `Executar`)

**Fonte do estado:** `Worker → Banco → Monitor.Api → Orquestrador`.

## Endpoints obrigatórios

| Método | Path | Auth | Papel |
|--------|------|------|-------|
| GET | `/health` ou `/health/live` | pública | Liveness (sem SQL) |
| GET | `/health/ready` | pública | Readiness SQL |
| GET | `/api/monitor/info` | API key | Identidade + endpoints |
| GET | `/api/monitor/service/status` | API key | Estado oficial |
| GET | `/api/monitor/snapshot` | API key | Telemetria / AGORA |
| GET | `/api/monitor/logs` | API key | afterSeq / take |
| GET | `/api/monitor/tables/{key}` | API key | Detalhe tabelas vigiladas |
| POST | `/api/monitor/service/start` | API key | Aceite → Starting + commandId |
| POST | `/api/monitor/service/stop` | API key | Aceite → Stopping + commandId |
| Hub | `/hubs/monitor` | app | `snapshot`, `logsAppend` |

Aliases: `/api/v1/monitor/*`.

Auth: header **`X-Cte-Internal-Api-Key`** = `Monitor:InternalApiKey` (DEV: `dev-cte-chain-key`).

Headers de identidade: `X-Monitor-Service` · `X-Monitor-Version`.

## Estados oficiais

`Disabled` · `Offline` · `Starting` · `Running` · `Stopping` · `Stopped` · `Failed` · `Unknown`

## Portas locais (mapa)

| Domínio | API | UI | Swagger | CodServico |
|---------|-----|-----|---------|------------|
| receptor | 5010 | 4200 | 7116 | 2 |
| arquivador | 5020 | 4210 | 7126 | 3 |
| sintetizador | 5030 | 4230 | 7136 | 8 |
| analisador | 5040 | 4240 | 7146 | 6 |
| integrador | 5050 | 4250 | 7156 | 7 |
| carga | 5080 | 4260 | 7166 | 99 |
| orquestrador | 5000 | 4220 | 7100 | — |
| resgate | 5070 | /resgate no Orq. | — | — |

## Resiliência (padrão Orquestrador)

| Operação | Timeout | Retry |
|----------|---------|-------|
| Health | 3s | até 2 |
| Status/snapshot | 5s | 1 |
| Start/Stop HTTP | 10s | 0 |
| Poll Running/Stopped | 60s | poll 2s |
| Circuit breaker | 5 falhas · break 30s | — |

## Checklist para `Enabled: true`

- [ ] Contrato HTTP completo
- [ ] Dockerfile + Dockerfile.front
- [ ] Entrada em `docker-compose.chain.yml`
- [ ] Registry `Orchestrator:Systems` (Id, Order, DependsOn, BaseUrl, FrontendUrl)
- [ ] Mesma InternalApiKey Orquestrador ↔ monitor
- [ ] Smoke: `GET /api/chain/health` + Ligar inclui o estágio
