# Onboarding — microserviço na cadeia CT-e

> Como plugar **qualquer** novo sistema no Orquestrador e garantir que esteja **online**  
> (DEV local **ou** Docker Compose com API + Front).  
> Atualizado: 24/07/2026 · Registry schema **1.0**

## Ideia

O Orquestrador **não conhece** a lógica fiscal de cada etapa. Ele só fala o **contrato de microserviço** (`/health/live`, `/health/ready`, `/api/monitor/*` + API key).

**Fonte oficial do estado:** `Worker → Banco → Monitor.Api → Orquestrador`  
O Orquestrador **nunca grava** estado do worker; só consulta e dispara comandos.

**SQL como integração** worker↔Monitor é **transitório** (fase híbrida). Revisar quando houver mensageria.

## Dois modos (não misturar)

| Modo | Como sobe API/Front | Ligar |
|------|---------------------|-------|
| **Development (F5)** | Spawn local (`ProjectPath` / `FrontendProjectPath`) se `LocalDev.EnsureBeforeCascade=true` | (1) API+Angular de **todos** `Enabled` em paralelo (2) `service/start` + poll na ordem `Order`/`DependsOn` |
| **Docker / Homolog / Prod** | `docker compose` sobe API + Front; `EnsureBeforeCascade=false` | só health → start → poll (sem spawn) |

## Contrato mínimo (obrigatório)

Espelho de [`CONTRATO_MICROSERVICO_MONITOR.md`](../../1-Receptor/Doc/CONTRATO_MICROSERVICO_MONITOR.md):

| Método | Path | Auth | Papel |
|--------|------|------|-------|
| GET | `/health` ou `/health/live` | pública | Liveness (sem SQL) |
| GET | `/health/ready` | pública | Readiness; Ligar **não** chama start se falhar |
| GET | `/api/monitor/service/status` | API key | Operational status (estados oficiais) |
| GET | `/api/monitor/snapshot` | API key | Metrics / telemetria |
| POST | `/api/monitor/service/start` | API key | Aceite → `Starting` + `commandId` |
| POST | `/api/monitor/service/stop` | API key | Aceite → `Stopping` + `commandId` |

Aliases `/api/v1/monitor/*` espelham os paths atuais.

### Estados oficiais

`Disabled` · `Offline` · `Starting` · `Running` · `Stopping` · `Stopped` · `Failed` · `Unknown`

## Checklist — `Enabled: true` (obrigatório)

- [ ] Contrato HTTP implementado (live / ready / status / start / stop / snapshot)
- [ ] `Dockerfile` da API + `Dockerfile.front` (nginx)
- [ ] Entrada em `CT_e/docker-compose.chain.yml` (API + Front + healthcheck)
- [ ] Entrada no Registry (`Orchestrator:Systems`) com schema abaixo
- [ ] Mesma `InternalApiKey` Orquestrador ↔ monitor
- [ ] `BaseUrl` / `FrontendUrl` (ou `Endpoints.*`) do ambiente
- [ ] Smoke: `GET /api/chain/health` → online; Ligar inclui o estágio

**Nenhum** novo estágio (S/An/I/C) entra com `Enabled: true` sem este checklist.

## Registry (schema 1.0)

```json
{
  "Id": "sintetizador",
  "DisplayName": "Sintetizador",
  "Symbol": "S",
  "Version": "1.0",
  "Order": 3,
  "DependsOn": ["arquivador"],
  "BaseUrl": "http://localhost:5030",
  "FrontendUrl": "http://localhost:4230",
  "Enabled": true,
  "Endpoints": {
    "Api": "http://localhost:5030",
    "Frontend": "http://localhost:4230",
    "HealthLive": "/health",
    "HealthReady": "/health/ready",
    "Status": "/api/monitor/service/status",
    "Start": "/api/monitor/service/start",
    "Stop": "/api/monitor/service/stop",
    "Metrics": "/api/monitor/snapshot"
  },
  "Ui": { "Icon": "layers", "Color": "#7c3aed" },
  "Instances": { "Mode": "active", "Targets": [] }
}
```

DEV local (só F5): opcional `ProjectPath` / `FrontendProjectPath`.  
Homolog/Prod / Docker: **sem** spawn — URLs estáveis + compose.

Ordem do **Ligar** = `Order` (depois `DependsOn`). **Desligar** = ordem inversa. Só `Enabled=true`.

### Timeout / Retry / Circuit Breaker (padrão)

| Operação | Timeout | Retry |
|----------|---------|-------|
| Health | 3s | até 2 |
| Status/snapshot | 5s | 1 |
| Start/Stop HTTP | 10s | 0 (idempotente + Idempotency-Key) |
| Poll até Running/Stopped | 60s | poll 2s |
| Circuit breaker | 5 falhas · break 30s | — |

Falha parcial na cascata: **fail-fast** (para a sequência; mantém o que já ligou).

### Multi-instância

MVP: `Instances.Mode = active` (uma instância por Id). `all` / `roundRobin` / `byEnvironment` = evolução.

## Docker — sistema novo na cadeia

1. `Dockerfile` + `Dockerfile.front` no pacote  
2. Entrada em `CT_e/docker-compose.chain.yml` (API + Front)  
3. Registry via env no `orquestrador-api` (`BaseUrl` = DNS interno, `FrontendUrl` = URL do browser)  
4. `PreferLocalProcess=false` (worker no host Windows)  
5. Connection string SQL via env (SQL auth — Integrated Security não funciona no container Linux)

```powershell
$env:CTE_INTERNAL_API_KEY = "dev-cte-chain-key"
$env:MONITOR_RECEPTOR_CONNECTION_STRING = "Server=host.docker.internal;Database=bd_cte_recepcao;User Id=...;Password=...;TrustServerCertificate=True"
docker compose -f docker-compose.chain.yml up --build
```

- APIs: `:5000` / `:5010` / `:5020`  
- Fronts: `:4220` / `:4200` / `:4210`  
- Gateway (opcional): `http://localhost:8080`

**Ligar não executa** `docker compose` — só contrato HTTP.

## Documentos relacionados

- [Documentação Orquestrador](Documentacao_Orquestrador_CTe.md)
- [Contrato Monitor](../../1-Receptor/Doc/CONTRATO_MICROSERVICO_MONITOR.md)
- Compose: `CT_e/docker-compose.chain.yml` · Gateway: `CT_e/gateway/`
