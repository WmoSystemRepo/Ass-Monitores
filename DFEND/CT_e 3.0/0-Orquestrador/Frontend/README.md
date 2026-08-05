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

## Menu / rotas

| Menu / ação | Rota |
|-------------|------|
| Monitor (visão da cadeia) | `/` |
| Clique no estágio R/A/S/An/I/C | `/monitores/{servico}` |
| Threads / Logs / Tabelas / Config / Detalhes | `/monitores/{servico}/…` |
| Resgate CT-e | `/resgate` |

`{servico}`: `receptor` · `arquivador` · `sintetizador` · `analisador` · `integrador` · `carga`.

## Libs principais

| Alias | Papel |
|-------|--------|
| `@orquestrador/monitor-dashboard` | Cadeia (Ligar/Desligar + anatomia dos 6 sistemas) |
| `@orquestrador/monitor-core` | `ChainOrchestratorStore` + `getApiBaseUrl()` |
| `@orquestrador/service-monitors` | Monitor rico por serviço (paridade **CT_e 2.0**) |
| `@orquestrador/resgate-cte` | Resgate |

CSS de anatomia/animações dos monitores: `apps/cte-orquestrador/src/service-monitor-extras.css` (importado em `styles.css`).

Dados do monitor: `ServiceMonitorStore` faz poll REST em `/api/monitores/{servico}/*` (a cada ~2s).

## Observações

- Paleta navy / indigo / lime (cadeia); monitores de serviço mantêm o visual do CT_e 2.0
- Cascata e snapshot da cadeia: `/api/orchestrator/*`
- Operação principal **não** depende de micro-fronts `:4200`–`:42xx`
- Auth usuário do dashboard ainda não existe; a proteção serviço-a-serviço é a API key interna no BFF ↔ engines
