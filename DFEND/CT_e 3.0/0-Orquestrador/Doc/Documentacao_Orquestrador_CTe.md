# Documentação — Orquestrador CT-e

> Dashboard central da cadeia DFEND CT-e · registry, autenticação interna e multiambiente  
> Atualizado: 06/08/2026 (SignalR · UX cadeia · FAQ Desligar + NA FILA)

## 1. Objetivo

Observar e controlar (**Ligar as filas** / **Desligar filas** em cascata) os monitores da cadeia:

**Receptor (R) → Arquivador (A) → Sintetizador (S) → Analisador (An) → Integrador (I) → Carga (C)**

O Orquestrador é um **BFF**: não processa CT-e. Agrega saúde/telemetria dos monitores e dispara start/stop em ordem.

### Regra operacional das filas

| Ação UI | Efeito |
|---------|--------|
| **Ligar as filas** | Sobe o processo (DevHost/SCM) **e** grava `Executar=1` no SQL de cada serviço |
| **Desligar filas** | Grava `Executar=0` **e** para o processo (ordem inversa) |

Não há estado **“pausado”** na cascata: processo no ar com filas ligadas = **Ativo**; desligado = **Parado**.  
`service/start` e `service/stop` do módulo in-process seguem a mesma regra.

**Desligar não esvazia as filas.** Os documentos/itens que já estavam aguardando continuam no backlog (SQL / Service Broker / temp). A tela pode mostrar **Fase: Parada** com **Com fila > 0** e estágios em laranja **NA FILA** — isso é esperado. Ao **Ligar as filas** de novo, a cadeia retoma e consome esse backlog.

| Depois de Desligar (esperado) | Significado |
|-------------------------------|-------------|
| Fase **Parada** · Serviços ativos **0** · Processos no ar **0** | Cascata encerrada; ninguém processa |
| **Ligar as filas** habilitado · **Desligar** desabilitado | Pronto para religar |
| **Com fila** / **Arquivos** > 0 · badge **NA FILA** (âmbar) | Backlog pendente — **não** foi apagado |
| Estágios sem backlog em cinza **PARADO** | Sem processo e sem fila naquele ponto |

Ver também §14 (dúvidas frequentes).

No dashboard (`:4220`):

- Menu lateral: **Dashboard** (cadeia) e **Resgate CT-e**.
- **Clique em um estágio** → navega **in-app** para `/monitores/{servico}` (monitor rico com anatomia/animações, paridade CT_e 2.0).
- Dados do monitor: **SignalR** `/hubs/monitor` (push ~1s) com **fallback REST** `/api/monitores/{servico}/*` no mesmo host `:5000`.
- `FrontendUrl` / “front legado” é opcional; a operação principal é o Angular único.
- Confirmações (Ligar/Desligar e “Ver erro”) usam o modal compartilhado `ConfirmDialog` — não `window.confirm`.

Quem garante engines/DevHosts online antes do worker é o **Ligar as filas** (cascata) ou o **container** em Docker.

Para plugar um sistema novo quando quiser: [ONBOARDING_MICROSERVICO.md](ONBOARDING_MICROSERVICO.md).  
Dev em qualquer PC (paths, one-click, troubleshooting): [DEV_PORTATIL.md](DEV_PORTATIL.md).

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
| POST | `/api/orchestrator/start` | **Ligar as filas**: (1) API+Angular online (2) `service/start` = processo + `Executar=1` |
| POST | `/api/orchestrator/stop` | **Desligar filas**: `service/stop` = `Executar=0` + parar processo (… → A → R) |
| POST | `/api/orchestrator/ensure-stacks` | boot do front Orquestrador: sobe API+Angular de todos `Enabled` (sem workers) |
| GET | `/api/orchestrator/status` | fase da cascata |
| GET | `/api/orchestrator/info` | meta + registry (`BaseUrl`, `FrontendUrl`, `Enabled`) |
| POST | `/api/orchestrator/systems/{id}/ensure-open` | Clique no estágio: sobe/valida API+front; só abre URL se `frontendReachable` |
| GET | `/api/chain/health` | por sistema: `online` \| `offline` \| `disabled` \| `unauthorized` |
| WS/HTTP | `/hubs/monitor` | **SignalR** — monitores ricos: `JoinService(servico)` → eventos `snapshot` / `logsAppend` (~1s) |
| GET | `/api/monitores/{servico}/*` | REST do monitor unificado (snapshot, logs, start/stop, …) — fallback se SignalR cair |
| GET | `/health` | liveness |
| GET | `/health/ready` | readiness do BFF (config OK; monitores offline **não** derrubam) |

### SignalR — monitores ricos (paridade CT_e 2.0)

| Item | Detalhe |
|------|---------|
| Hub | `/hubs/monitor` (`MonitorHub`) |
| Cliente | `ServiceMonitorStore` → `getHubUrl()` = `{apiBaseUrl}/hubs/monitor` |
| Entrada | `JoinService("receptor" \| "arquivador" \| …)` |
| Push | `snapshot` (telemetria completa) e `logsAppend` (logs novos) |
| Hosted service | `MonitorPushHostedService` — intervalo ~1s para serviços com assinantes |
| Fallback | se o hub cair, o store volta a poll REST (~2s) e o badge de conexão indica SignalR vs REST |

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
| `AutoStartMonitors` | no boot, sobe stacks de sistemas `Enabled` (quando configurado) |
| `EnsureBeforeCascade` | no Ligar: garante engines/API ready antes do `service/start` na ordem `Order`/`DependsOn` |
| `RepoRoot` | **deixar vazio** — descoberta automática da pasta `0-Orquestrador` (não use path absoluto de outra máquina) |

Engines DevHost ficam em `0-Orquestrador\engines\{servico}\` (`PackageFolder`: `engines\receptor`, …).  
Saída de build: `0-Orquestrador\_artifacts\` (gitignored). Ver [DEV_PORTATIL.md](DEV_PORTATIL.md).

Se API ou engine não ficarem online: mensagem explícita e o serviço daquele sistema **não** é ligado.

Não se aplica em Homolog/Prod (process spawn desligado — use deploy/container).

## 8. Front

| Item | Detalhe |
|------|---------|
| Porta DEV | `http://localhost:4220` |
| API DEV | `http://localhost:5000` |
| Runtime config | `public/config.json` → `{ "apiBaseUrl": "..." }` |
| Override deploy | `window.__CTE_ORQ_API_BASE__` no `index.html` |
| Bootstrap | `loadRuntimeApiConfig()`; UI fala com Orquestrador `:5000` |
| Menu | **Dashboard** (`/`) · **Resgate CT-e** (`/resgate`) |
| Clique no estágio | navega para `/monitores/{servico}` (anatomia CT_e 2.0, lib `service-monitors`) |
| Dados do monitor | SignalR `/hubs/monitor` + fallback REST `/api/monitores/{servico}/*` |
| Ligar as filas | CTA **só no header** (evita duplicata no idle) → engines (DEV) → `service/start` |
| Desligar filas | `ConfirmDialog` → `service/stop` (= `Executar=0` + parar processo), ordem inversa; **não limpa backlog** |
| Modal | `ConfirmDialogService` / `ConfirmDialogComponent` (`shared-ui`) — confirm + modo `info` (erro original) |

Homolog/Prod: publicar `config.json` (ou script inline) com a URL do Orquestrador daquele ambiente — **não** embutir host no build.  
`FrontendUrl` no registry permanece só como link legado opcional.

### 8.1 Dashboard da cadeia — UX

Hierarquia visual (lib `monitor-dashboard`):

1. **AGORA** — serviço com trabalho ativo / telemetria “quente”
2. **Fila** — profundidade de arquivos (`QueueMeterComponent`: chips sobem ao encher, encolhem ao drenar)
3. **Ativo** — processo + `Executar=1`, sem fila
4. **Parado** / ligando / desligando

Componentes-chave:

| Componente | Papel |
|------------|-------|
| `ChainAnatomyComponent` | 6 estações + idle hero + boot em cascata ao Ligar |
| `StationCardComponent` | card clicável (badge AGORA / NA FILA / … + medidor) |
| `QueueMeterComponent` | profundidade visual; animação `rising` / `draining` |
| `StatusLegendComponent` | legenda Agora / Feito / Parado |

Ao **Ligar as filas**, cada estação anima com atraso (`--boot-delay`) enquanto `cascadePhase === starting`.  
CTA “Ligar” fica **apenas no header** — o idle hero só orienta o usuário (sem segundo botão).

O idle hero (“Nenhuma fila ligada”) só aparece quando a fase está idle **e** não há backlog nem erros. Se a cadeia está **Parada** mas ainda há arquivos na fila, o rail **Foco agora** continua listando os estágios com badge **FILA** (âmbar) e o rodapé mostra **Com fila** / **Arquivos na cadeia** — estado normal após Desligar com trabalho pendente.

### 8.2 Monitores de serviço — UX padrão (todos)

O **Receptor** definiu o padrão; os outros 5 (Arquivador, Sintetizador, Analisador, Integrador, Carga) seguem o mesmo.

| Área | Comportamento unificado |
|------|-------------------------|
| Título do painel | `{Serviço} CT-e` + uma frase de negócio (sem “Monitor do…”) |
| Anatomia | Copy leiga; fila/temp sobe/desce; boot em cascata ao Ligar; chips na profundidade |
| Mais informações | **Uma** implementação: `SharedServiceDetailsPageComponent` (meta por `serviceId`) |
| Layout detalhes | Grid 2×2 na viewport, sem scroll da página: atividade · eventos · **Saúde dos bancos** · avisos |
| Erro SQL | Botão **Ver erro** → `ConfirmDialog` `mode: 'info'` + texto original |
| Avisos | `snapshot.alerts` via `BuildHealthAlerts` (mensagens genéricas da fila) |
| Desligar | `ConfirmDialog` (sem `window.confirm`) |

Rotas: `/monitores/{servico}` e `/monitores/{servico}/mais-informacoes`.

Aliases finos (`ReceptorDetailsPageComponent`, …) só reexportam o shared — **não** duplicar layout por serviço.

Script de sync de anatomia (dev): `Frontend/tools/sync-receptor-anatomy-ux.py` + `fix-anatomy-motion-body.py`.

### 8.3 Alertas de saúde (API)

`InProcessMonitorModule` **não** devolve mais `alerts = []`.  
`BuildHealthAlerts(...)` sintetiza avisos a partir da telemetria:

| Código (ex.) | Quando |
|--------------|--------|
| `SQL_CFG` | sem connection string |
| `PROC_OFF` | processo parado |
| `EXEC_0` | processo no ar com `Executar=0` |
| `SVC_STALE` | batida (`DtcExecucao`) atrasada (>2h) |
| `TEMP_BACKLOG` / `FILA_BACKLOG` | profundidade > 0 |
| `OK` | serviço ligado e sem outros alertas |

Mensagens são **genéricas** (qualquer serviço da cadeia). O card “Avisos e saúde” consome `snapshot.alerts`. **Reinicie a API** após deploy.

### 8.4 Animações — o que existe (e o que não)

Não há toggle `demoMode` / “fluxo de demonstração” no 2.0 nem no 3.0.  
As animações de documento são a **jornada do lote** (`playLoteJourney` + chips CT-e voando + esteira), acionadas pela telemetria quando o serviço está ligado.

Extras de UX recentes:

- Medidor / plataforma **sobe** quando a fila enche e **diminui** quando drena
- Cascata visual ao **Ligar** (cadeia e Receptor)
- `prefers-reduced-motion` respeitado nos CSS (`styles.css`, `service-monitor-extras.css`)

## 9. Como subir

### 9.1 Development (Visual Studio)

**Preferencial:** `LIMPAR-E-BUILDAR.cmd` → `ABRIR-SOLUTION.cmd` ([DEV_PORTATIL.md](DEV_PORTATIL.md)).

1. Abrir `0-Orquestrador/Orquestrador.Api/Orquestrador.sln` · perfil **https** ou **http** · F5  
   (layout unificado: monitores em `/monitores/{id}`; engines em `engines\`)
2. Front:

```powershell
cd Frontend   # ...\0-Orquestrador\Frontend
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
5. Smoke: `GET /health/ready`, `GET /api/chain/health`, Ligar as filas, clique no estágio → `/monitores/{servico}` no `:4220`

## 10. Checklist go-live (Homolog e Prod)

1. HTTPS (ou TLS no reverse proxy) no Orquestrador e monitores  
2. Keys iguais Orquestrador ↔ monitores, só em secret store  
3. `BaseUrl` e `FrontendUrl` de cada `Enabled=true` apontando aos hosts do ambiente  
4. Probes: `/health`, `/health/ready`; ops: `/api/chain/health`  
5. CORS restrito ao host do front  
6. Rotação da API key no runbook  
7. Front com URL de API do ambiente (não localhost)  
8. Smoke: snapshot sem `unauthorized` / `offline` inesperado; cascade start/stop OK; clique no estágio abre o monitor; SignalR conecta (ou REST fallback); Receptor Mais informações + alertas OK  

## 11. Estrutura do pacote

```text
0-Orquestrador/
  LIMPAR-E-BUILDAR.cmd / ABRIR-SOLUTION.cmd / PROCURAR-E-CONSERTAR.cmd
  COMO-USAR.txt
  Directory.Build.props / .gitignore   # _artifacts (paths curtos; nao versionar)
  Frontend/                            # Nx cte-orquestrador :4220
    apps/cte-orquestrador/
      src/styles.css                   # tokens + queue-meter + reduced-motion
      src/service-monitor-extras.css   # anatomia Receptor / plataformas / fila
    libs/
      monitor-dashboard/               # Dashboard cadeia (station-card, queue-meter)
      service-monitors/                # Monitores ricos (receptor…carga)
      shared-ui/                       # ConfirmDialog (+ severity helpers)
      monitor-core/                    # ChainOrchestratorStore, getHubUrl()
  Orquestrador.Api/                    # BFF :5000 / Swagger :7100
    …/Realtime/                        # MonitorHub + push ~1s
    …/Monitors/.../InProcessMonitorModule.cs  # BuildHealthAlerts
  engines/                             # DevHosts (receptor…carga)
  libs/resgate/
  tools/                               # fix-dev.ps1, verify-structure.ps1, start-chain-fronts.cjs
  Doc/
    DEV_PORTATIL.md
    Documentacao_Orquestrador_CTe.md   # este arquivo
    ONBOARDING_MICROSERVICO.md
    Passo a passo execução Orquestrador.md
  README.md
```

## 12. Paleta UI

Indigo / violet / lime / fuchsia — distinta do Receptor (cyan) e do Arquivador (âmbar).  
Fila / AGORA: âmbar e azul neon nos medidores; erro: rose.

## 13. Histórico recente (UX / realtime)

| Quando | Entrega |
|--------|---------|
| 06/08/2026 | SignalR `/hubs/monitor` + fallback REST; layout anatomia alinhado ao 2.0 |
| 06/08/2026 | Redesign da cadeia: AGORA > fila > `QueueMeter` / `StationCard`; idle hero; CTA Ligar só no header |
| 06/08/2026 | Receptor: copy para leigo; Mais informações 2×2; **Ver erro** original; `BuildHealthAlerts` |
| 06/08/2026 | Animação fila sobe/desce + boot ao Ligar (cadeia e Receptor); `ConfirmDialog` compartilhado |
| 06/08/2026 | Padrão Receptor replicado nos 6 monitores: shared Mais informações, animações fila/boot, copy leiga, docs |
| 06/08/2026 | Mais informações: card **Saúde dos bancos** (conexão + `tableHealth`) no lugar de Configuração e lotes |
| 06/08/2026 | Doc + UX: após **Desligar**, Fase Parada com **NA FILA** / backlog pendente é esperado (não limpa fila) |

## 14. Dúvidas frequentes (operação)

### Depois de Desligar, a tela ficou “Parada” mas ainda mostra NA FILA / Com fila — está quebrado?

**Não.** É o comportamento correto.

1. **Desligar filas** só para processos e grava `Executar=0`. **Não apaga** documentos já enfileirados.
2. Badge **NA FILA** (âmbar) = `queueDepth > 0` naquele estágio (`HasQueueWork`), independente de a cascata estar ligada.
3. **Fase: Parada** + **Serviços ativos 0** + **Processos no ar 0** = ninguém está consumindo a fila agora.
4. **Com fila** / **Arquivos na cadeia** no rodapé contam esse backlog parado.
5. Para retomar: **Ligar as filas** — a cadeia sobe e drena o que ficou pendente.

Se a fase for Parada **e** Com fila = 0 **e** todos os estágios cinza PARADO, aí sim a cadeia está vazia e parada (idle completo; idle hero “Nenhuma fila ligada”).

### Ligar / Desligar apaga ou cria lotes?

Não. Ligar/Desligar controla **processo + flag Executar**. Lotes, NSU e profundidade de fila vêm da telemetria SQL/broker de cada serviço.

### O que significa Status 215 / “Falha no esquema XML”?

É rejeição da **SEFAZ**: o XML enviado na chamada (ex.: `retDistCTeSVD` / Carga `ProcessarDownload`) não passou na validação do schema oficial.  
No monitor, **Mais informações → Ver erro** mostra a **frase clara** (catálogo) + o **texto original** do banco.  
Histórico de traduções: `Frontend/libs/shared-utils/src/lib/log-error-catalog.ts` — acrescentar novas entradas quando aparecerem casos novos.

## 15. Documentos relacionados

- [Dev portátil (one-click, paths, troubleshooting)](DEV_PORTATIL.md)
- [Onboarding microserviço (plugar + Docker)](ONBOARDING_MICROSERVICO.md)
- [Passo a passo execução](Passo%20a%20passo%20execução%20Orquestrador.md)
- [README do pacote](../README.md)
- [README do Frontend](../Frontend/README.md)
- [COMO-USAR.txt](../COMO-USAR.txt)
