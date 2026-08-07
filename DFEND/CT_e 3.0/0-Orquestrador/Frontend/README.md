# Frontend — Orquestrador CT-e

App Nx `cte-orquestrador` (porta **4220**). Consome a **Orquestrador.Api** (`:5000` em DEV).

Pacote pai: `0-Orquestrador/` · doc: [../Doc/Documentacao_Orquestrador_CTe.md](../Doc/Documentacao_Orquestrador_CTe.md)  
SDD: `Assefaz\CT_e\.cursor\SDD\Monitor Unificado CT-e`

## Pré-requisito Node

Use **Node 20, 22 ou 24** (`.nvmrc` = `24`). Stack: **Nx 21.6** + **Angular 19.2** (Node 24 oficial via Nx 21).

No Windows PowerShell, use `npm.cmd` (a ExecutionPolicy pode bloquear `npm.ps1`).

## Como rodar

```powershell
cd Frontend
node --version   # v20.x / v22.x / v24.x
npm.cmd install
Test-Path .\node_modules\nx\bin\nx.js   # deve ser True
npm.cmd start
# ou: .\serve.cmd
# ou: node .\node_modules\nx\bin\nx.js serve cte-orquestrador
```

Abrir `http://localhost:4220` (Ctrl+F5 após mudanças de UI).

Antes: subir Orquestrador.Api em `http://localhost:5000`.

## URL da API (runtime)

| Fonte | Uso |
|-------|-----|
| `public/config.json` | `{ "apiBaseUrl": "http://localhost:5000" }` — DEV padrão |
| `window.__CTE_ORQ_API_BASE__` | override no `index.html` (Homolog/Prod) |

O bootstrap chama `loadRuntimeApiConfig()` antes do Angular. Em Homolog/Prod, publique o `config.json` (ou o script) com a URL do Orquestrador **daquele** ambiente — não embutir no build.

Hub SignalR: `{apiBaseUrl}/hubs/monitor` (`getHubUrl()` em `@orquestrador/monitor-core`).

## Menu / rotas

| Menu / ação | Rota |
|-------------|------|
| **Dashboard** (ligar e acompanhar filas) | `/` |
| Clique no estágio R/A/S/An/I/C | `/monitores/{servico}` |
| Mais informações (todos os serviços) | `/monitores/{servico}/mais-informacoes` |
| Threads / Logs / Tabelas / Config | `/monitores/{servico}/…` |
| Resgate CT-e | `/resgate` |

`{servico}`: `receptor` · `arquivador` · `sintetizador` · `analisador` · `integrador` · `carga`.

## Libs principais

| Alias | Papel |
|-------|--------|
| `@orquestrador/monitor-dashboard` | Cadeia: Ligar/Desligar, `ChainAnatomy`, `StationCard`, `QueueMeter`, legenda |
| `@orquestrador/monitor-core` | `ChainOrchestratorStore`, `getApiBaseUrl()`, `getHubUrl()` |
| `@orquestrador/service-monitors` | Monitor rico por serviço (paridade **CT_e 2.0** + SignalR); `SharedServiceDetailsPageComponent` |
| `@orquestrador/shared-ui` | `ConfirmDialog` (confirm / info com `detail` monoespaçado) |
| `@orquestrador/resgate-cte` | Resgate |

CSS de anatomia/animações:

- `apps/cte-orquestrador/src/styles.css` — tokens, queue-meter, station-card, reduced-motion
- `apps/cte-orquestrador/src/service-monitor-extras.css` — pipeline Receptor (plataformas, fila sobe/desce, boot)

### Dados do monitor

`ServiceMonitorStore`:

1. Conecta SignalR → `JoinService(servico)` → eventos `snapshot` / `logsAppend`
2. Se o hub falhar → poll REST `/api/monitores/{servico}/*` (~2s)
3. Badge de conexão indica **SignalR** vs **REST**

### Dashboard — fila visual

- Hierarquia: **AGORA** > profundidade de fila > ativo > parado
- `QueueMeterComponent`: chips sobem ao encher (`rising`) e encolhem ao drenar (`draining`)
- Ao **Ligar as filas**: estações animam em cascata (`booting` + `--boot-delay`)
- CTA Ligar **só no header** (idle hero só explica)

### Receptor — Mais informações

Grid 2×2 sem scroll da página:

| Card | Função |
|------|--------|
| O que aconteceu agora | Passos + logs recentes |
| Últimos eventos do banco | Severidade; **Ver erro** / **Ver detalhes** |
| **Saúde dos bancos** | `connectionHealth` + `tableHealth`; **Ver jornada dos lotes →** / **Tabelas →** / **Config →** |
| Avisos e saúde | `snapshot.alerts` (`BuildHealthAlerts`) |

**Ver erro** abre explicação leiga (`log-error-catalog.ts`) + texto original; **Copiar texto** no modal.  
NSU/filas/lotes: painel (anatomia) e rotas Config/Tabelas — não ficam mais neste card.

### Padrão nos 6 monitores

O mesmo layout/comportamento de **Mais informações**, copy leiga no painel e animações de fila/boot vale para Receptor, Arquivador, Sintetizador, Analisador, Integrador e Carga.  
Fonte única: `SharedServiceDetailsPageComponent` (meta por `serviceId`).

## Observações

- Paleta navy / indigo / lime (cadeia); monitores de serviço mantêm o visual do CT_e 2.0
- Cascata e snapshot da cadeia: `/api/orchestrator/*`
- Operação principal **não** depende de micro-fronts `:4200`–`:42xx`
- Auth usuário do dashboard ainda não existe; a proteção serviço-a-serviço é a API key interna no BFF ↔ engines
- Não existe `demoMode`: animações de CT-e = jornada do lote + telemetria ao vivo
