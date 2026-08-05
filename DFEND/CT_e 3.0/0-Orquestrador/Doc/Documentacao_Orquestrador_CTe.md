# Documentação — Orquestrador CT-e

> Dashboard central da cadeia DFEND CT-e · registry, autenticação interna e multiambiente  
> Atualizado: 05/08/2026

## 1. Objetivo

Observar e controlar (Ligar/Desligar em cascata) os monitores da cadeia:

**Receptor (R) → Arquivador (A) → Sintetizador (S) → Analisador (An) → Integrador (I) → Carga (C)**

O Orquestrador é um **BFF**: não processa CT-e. Agrega saúde/telemetria dos monitores e dispara start/stop em ordem.

No dashboard (`:4220`):

- **Clique em um estágio** → navega **in-app** para `/monitores/{servico}` (monitor rico com anatomia/animações, paridade CT_e 2.0).
- Dados do monitor vêm de `/api/monitores/{servico}/*` no mesmo host `:5000`.
- `FrontendUrl` / “front legado” é opcional; a operação principal é o Angular único.

Quem garante engines/DevHosts online antes do worker é o **Ligar** (cascata) ou o **container** em Docker.

Para plugar um sistema novo quando quiser: [ONBOARDING_MICROSERVICO.md](ONBOARDING_MICROSERVICO.md).  
SDD / gates: `Assefaz\CT_e\.cursor\SDD\Monitor Unificado CT-e`.

## 2. Princípio definitivo (todos os ambientes)

O mesmo desenho vale em **Development**, **Homologacao** e **Production**:

1. Identidade estável por `Id` (`receptor`, `arquivador`, …)
2. `BaseUrl` (API do monitor) e `FrontendUrl` (UI Angular) sempre por configuração / env
3. Autenticação serviço-a-serviço com header `X-Cte-Internal-Api-Key`
4. Resiliência HTTP (timeout, retry limitado, circuit breaker) + falha suave

Não há caminho especial só para DEV: a diferença entre ambientes é **só o valor** de URL e da key.

## 3. Ambientes

| Ambiente | `ASPNETCORE_ENVIRONMENT` | Onde fica BaseUrl / FrontendUrl / Key |
|----------|--------------------------|--------------------------------------|
| Development | `Development` | `appsettings.Development.json` (localhost + key local `dev-cte-chain-key`) |
| Homologação | `Homologacao` | env / secret store + `appsettings.Homologacao.json` (estrutura sem secrets) |
| Produção | `Production` | env / secret store + `appsettings.Production.json` (estrutura sem secrets) |

`InternalApiKey` é **obrigatória nos três** (`ValidateOnStart`). Homolog/Prod: **nunca** commitar secret no git.

Arquivos de config do Orquestrador:

```text
Orquestrador.Api/src/Orquestrador.Api/
  appsettings.json                 # defaults / registry base
  appsettings.Development.json     # localhost + key DEV
  appsettings.Homologacao.json     # placeholders (preencher via env)
  appsettings.Production.json      # placeholders (preencher via env)
```

Monitores (Receptor e Arquivador) seguem o mesmo padrão:

```text
Monitor:InternalApiKey
appsettings.Development.json | Homologacao.json | Production.json
```

## 4. Registry da cadeia

Seção `Orchestrator` em `appsettings*.json` + override por env.

| Id | Símbolo | Responsabilidade | Enabled (default) |
|----|---------|------------------|-------------------|
| `receptor` | R | Entrada CT-e / NSU | true |
| `arquivador` | A | Persistência / filas | true |
| `sintetizador` | S | Síntese | false (aguarda monitor) |
| `analisador` | An | Análise | false |
| `integrador` | I | Integração | false |
| `carga` | C | Carga | false |

**Regra:** o código conhece o `Id`. `BaseUrl` e `FrontendUrl` vêm do ambiente.  
Sistemas futuros (`sintetizador` … `carga`) ficam com `Enabled: false` até o monitor existir — aí basta contrato + URLs + `Enabled: true` (ver [ONBOARDING_MICROSERVICO.md](ONBOARDING_MICROSERVICO.md)).

### Campos por sistema

| Campo | Uso |
|-------|-----|
| `BaseUrl` | API do Monitor.Api (health, snapshot, start/stop) |
| `FrontendUrl` | UI Angular do sistema |
| `ProjectPath` | (DEV) `.csproj` do Monitor.Api — auto-start silencioso no boot/Ligar |
| `FrontendProjectPath` | (DEV) pasta do front — auto-start silencioso no Ligar se a UI estiver offline |
| `Enabled` | participa da cascata e do snapshot agregado |

### Exemplos de URLs (Receptor)

| Contexto | BaseUrl (API) | FrontendUrl (UI) |
|----------|---------------|------------------|
| Development (F5) | `http://localhost:5010` | `http://localhost:4200` |
| Docker compose da cadeia | `http://monitor-receptor-api:5010` | (front no host: `:4200` ou URL publicada) |
| Homologacao / Production | `https://monitor-receptor.<dominio>` | `https://receptor.<dominio>` |

Arquivador em DEV: API `:5020` · UI `:4210`. Orquestrador UI: `:4220`.

### Validação no boot

`ValidateOnStart` falha se:

- sistema `Enabled=true` sem `BaseUrl` absoluta `http`/`https`
- `Id` vazio ou duplicado
- `InternalApiKey` vazia (qualquer ambiente)

`FrontendUrl` é opcional (link legado). O estágio da cadeia **sempre** navega para `/monitores/{id}` no Angular único.

## 5. Contrato HTTP

### Orquestrador → monitores

| Método | Path | Auth |
|--------|------|------|
| GET | `/health/ready` | pública (probe) |
| GET | `/api/monitor/snapshot` | `X-Cte-Internal-Api-Key` |
| GET | `/api/monitor/service/status` | `X-Cte-Internal-Api-Key` |
| POST | `/api/monitor/service/start` | `X-Cte-Internal-Api-Key` |
| POST | `/api/monitor/service/stop` | `X-Cte-Internal-Api-Key` |

### Orquestrador expõe (front / ops)

| Método | Path | Uso |
|--------|------|-----|
| GET | `/api/orchestrator/snapshot` | dashboard em tempo real (inclui `frontendUrl` por sistema) |
| POST | `/api/orchestrator/start` | cascata ligar: (1) API+Angular online em paralelo (2) `service/start` |
| POST | `/api/orchestrator/stop` | cascata desligar (… → A → R) |
| POST | `/api/orchestrator/ensure-stacks` | boot do front Orquestrador: sobe API+Angular de todos `Enabled` (sem workers) |
| GET | `/api/orchestrator/status` | fase da cascata |
| GET | `/api/orchestrator/info` | meta + registry (`BaseUrl`, `FrontendUrl`, `Enabled`) |
| POST | `/api/orchestrator/systems/{id}/ensure-open` | Clique no estágio: sobe/valida API+front; só abre URL se `frontendReachable` |
| GET | `/api/chain/health` | por sistema: `online` \| `offline` \| `disabled` \| `unauthorized` |
| GET | `/health` | liveness |
| GET | `/health/ready` | readiness do BFF (config OK; monitores offline **não** derrubam) |

### Status no snapshot (`MetricPill` / reachability)

| Valor | Significado |
|-------|-------------|
| `online` / métrica (NSU, fila…) | monitor respondendo |
| `offline` | sem `/health/ready` ou conexão recusada |
| `unauthorized` | API key ausente/inválida no monitor |
| `disabled` | sistema `Enabled=false` no registry |

## 6. Autenticação interna

Header: **`X-Cte-Internal-Api-Key`**

| Papel | Comportamento |
|-------|----------------|
| Orquestrador | envia a key em todas as chamadas aos monitores |
| Monitor | middleware em `/api/monitor/*`; rejeita 401 se key errada/ausente |
| Probes | `/health` e `/health/ready` permanecem públicos |

### Development

Key local compartilhada (não usar em Homolog/Prod):

```text
dev-cte-chain-key
```

Presente em:

- `0-Orquestrador/.../appsettings.Development.json` → `Orchestrator:InternalApiKey`
- `1-Receptor/.../appsettings.Development.json` → `Monitor:InternalApiKey`
- `2-Arquivador/.../appsettings.Development.json` → `Monitor:InternalApiKey`

### Homologacao e Production

```text
ASPNETCORE_ENVIRONMENT=Homologacao   # ou Production

Orchestrator__InternalApiKey=<secret>
Orchestrator__Systems__0__BaseUrl=https://monitor-receptor...
Orchestrator__Systems__0__FrontendUrl=https://receptor...
Orchestrator__Systems__1__BaseUrl=https://monitor-arquivador...
Orchestrator__Systems__1__FrontendUrl=https://arquivador...

# Em cada monitor:
Monitor__InternalApiKey=<mesmo-secret>
```

A key do Orquestrador e a dos monitores **devem ser iguais** no mesmo ambiente.

## 7. Resiliência

- Pacote `Microsoft.Extensions.Http.Resilience` no client `MonitorClient`
- Configurável em `Orchestrator`: `HttpTimeoutSeconds`, `CircuitBreaker*`
- Conectividade falhou: Warning **uma vez** por `BaseUrl` até recuperar (evita spam no Output do VS)

### LocalDev (somente Development)

Em `Orchestrator:LocalDev`:

| Flag | Efeito |
|------|--------|
| `AutoStartMonitors` | no boot, sobe **API + Angular** de todos `Enabled` no registry (N sistemas; hoje R+A) |
| `EnsureBeforeCascade` | no Ligar: (1) sobe **em paralelo** API+Angular de todos `Enabled` (2) só depois `service/start` na ordem `Order`/`DependsOn` |

Se API ou Angular não ficarem online: mensagem explícita e o serviço daquele sistema **não** é ligado.

Não se aplica em Homolog/Prod (process spawn desligado — use deploy/container).

## 8. Front

| Item | Detalhe |
|------|---------|
| Porta DEV | `http://localhost:4220` |
| API DEV | `http://localhost:5000` |
| Runtime config | `public/config.json` → `{ "apiBaseUrl": "..." }` |
| Override deploy | `window.__CTE_ORQ_API_BASE__` no `index.html` |
| Bootstrap | `loadRuntimeApiConfig()`; UI fala com Orquestrador `:5000` |
| Clique no estágio | navega para `/monitores/{servico}` (anatomia CT_e 2.0, lib `service-monitors`) |
| Dados do monitor | poll REST `/api/monitores/{servico}/*` |
| Ligar cadeia | garante engines online (DEV) → depois `service/start` |

Homolog/Prod: publicar `config.json` (ou script inline) com a URL do Orquestrador daquele ambiente — **não** embutir host no build.  
`FrontendUrl` no registry permanece só como link legado opcional.

## 9. Como subir

### 9.1 Development (Visual Studio)

1. Subir **Receptor.Api** (`:5010`) e **Arquivador.Api** (`:5020`) — mesma key DEV
2. Abrir `0-Orquestrador/Orquestrador.Api/Orquestrador.sln` · perfil **https** ou **http** · F5
3. Front:

```powershell
cd Frontend   # CT_e\0-Orquestrador\Frontend
npm.cmd install
npm.cmd start
# http://localhost:4220
```

Perfis extras no Orquestrador: **Homologacao** (placeholders locais), **Production** (exige env).

### 9.2 Docker — cadeia (paridade Homolog/Prod)

Na raiz `CT_e`:

```powershell
$env:CTE_INTERNAL_API_KEY = "dev-cte-chain-key"
# SQL auth obrigatório no container Linux (ajuste ao seu servidor):
# $env:MONITOR_RECEPTOR_CONNECTION_STRING = "Server=host.docker.internal;Database=bd_cte_recepcao;User Id=...;Password=...;TrustServerCertificate=True"
# $env:MONITOR_ARQUIVADOR_CONNECTION_STRING = "..."
docker compose -f docker-compose.chain.yml up --build
```

| Serviço | Host (browser) | DNS interno |
|---------|----------------|-------------|
| Orquestrador API | `:5000` | `orquestrador-api:5000` |
| Orquestrador Front | `:4220` | `orquestrador-front` |
| Receptor API / Front | `:5010` / `:4200` | `monitor-receptor-api` / `monitor-receptor-front` |
| Arquivador API / Front | `:5020` / `:4210` | `monitor-arquivador-api` / `monitor-arquivador-front` |
| Gateway | `:8080` | `gateway` |

Compose sobe **API + Front**. `LocalDev.EnsureBeforeCascade=false` no container.  
**Ligar** = health → start → poll (fail-fast parcial). Worker no host Windows.  
Novo sistema: [ONBOARDING_MICROSERVICO.md](ONBOARDING_MICROSERVICO.md).

### Estados oficiais e resiliência

Ver ONBOARDING § estados + timeout/retry/CB. Registry schema `Orchestrator:RegistrySchemaVersion`.  
SQL worker↔Monitor = integração **transitória**.

### 9.3 Homologacao / Production (deploy)

1. Definir `ASPNETCORE_ENVIRONMENT`
2. Injetar `Orchestrator__InternalApiKey` e `Monitor__InternalApiKey` (secret store)
3. Injetar `Orchestrator__Systems__N__BaseUrl` e `Orchestrator__Systems__N__FrontendUrl` para cada sistema `Enabled`
4. Front: `config.json` / `__CTE_ORQ_API_BASE__` apontando ao Orquestrador
5. Smoke: `GET /health/ready`, `GET /api/chain/health`, Ligar cadeia, clique no estágio → `/monitores/{servico}` no `:4220`

## 10. Checklist go-live (Homolog e Prod)

1. HTTPS (ou TLS no reverse proxy) no Orquestrador e monitores  
2. Keys iguais Orquestrador ↔ monitores, só em secret store  
3. `BaseUrl` e `FrontendUrl` de cada `Enabled=true` apontando aos hosts do ambiente  
4. Probes: `/health`, `/health/ready`; ops: `/api/chain/health`  
5. CORS restrito ao host do front  
6. Rotação da API key no runbook  
7. Front com URL de API do ambiente (não localhost)  
8. Smoke: snapshot sem `unauthorized` / `offline` inesperado; cascade start/stop OK; clique no estágio abre o monitor  

## 11. Estrutura do pacote

```text
0-Orquestrador/
  Frontend/                 # Nx cte-orquestrador :4220
  Orquestrador.Api/         # BFF :5000 / Swagger :7100
  Dockerfile
  Doc/
    Documentacao_Orquestrador_CTe.md   # este arquivo
    ONBOARDING_MICROSERVICO.md         # plugar sistema novo + Docker
    Passo a passo execução Orquestrador.md
  README.md
```

Compose da cadeia (raiz CT_e): `docker-compose.chain.yml`

## 12. Paleta UI

Indigo / violet / lime / fuchsia — distinta do Receptor (cyan) e do Arquivador (âmbar).

## 13. Documentos relacionados

- [Onboarding microserviço (plugar + Docker)](ONBOARDING_MICROSERVICO.md)
- [Passo a passo execução](Passo%20a%20passo%20execução%20Orquestrador.md)
- [README do pacote](../README.md)
- Monitores: `1-Receptor/Doc/`, `2-Arquivador/Doc/` (contrato `/api/monitor/*` + `Monitor:InternalApiKey`)
