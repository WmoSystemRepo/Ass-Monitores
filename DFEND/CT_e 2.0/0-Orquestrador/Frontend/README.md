# Frontend — Orquestrador CT-e

App Nx `cte-orquestrador` (porta **4220**). Consome a **Orquestrador.Api** (`:5000` em DEV).

Pacote pai: `0-Orquestrador/` · doc: [../Doc/Documentacao_Orquestrador_CTe.md](../Doc/Documentacao_Orquestrador_CTe.md)

## Pré-requisito Node

Use **Node 20, 22 ou 24** (`.nvmrc` = `24`). Stack: **Nx 21.6** + **Angular 19.2** (Node 24 oficial via Nx 21).

No Windows PowerShell, use `npm.cmd` (a ExecutionPolicy pode bloquear `npm.ps1`).

## Como rodar

```powershell
cd ..\CT_e\0-Orquestrador\Frontend   # a partir de qualquer clone; ajuste só até CT_e
# ou, já estando em 0-Orquestrador:
cd Frontend
node --version   # v20.x / v22.x / v24.x
npm.cmd install
Test-Path .\node_modules\nx\bin\nx.js   # deve ser True
npm.cmd start
# ou: .\serve.cmd
# ou: node .\node_modules\nx\bin\nx.js serve cte-orquestrador
```

Abrir `http://localhost:4220` (Ctrl+F5 após mudanças de UI).

Antes: subir Orquestrador.Api em `http://localhost:5000` (e monitores se for usar Ligar/Desligar).

## URL da API (runtime)

| Fonte | Uso |
|-------|-----|
| `public/config.json` | `{ "apiBaseUrl": "http://localhost:5000" }` — DEV padrão |
| `window.__CTE_ORQ_API_BASE__` | override no `index.html` (Homolog/Prod) |

O bootstrap chama `loadRuntimeApiConfig()` antes do Angular. Em Homolog/Prod, publique o `config.json` (ou o script) com a URL do Orquestrador **daquele** ambiente — não embutir no build.

## Menu / rotas

| Menu | Rota |
|------|------|
| Monitor (Visão da cadeia) | `/` |
| Threads / Histórico / Tabelas / Config | páginas stub (não fazem parte do Orquestrador) |

## Observações

- Paleta indigo / violet / lime / fuchsia
- Cascata e snapshot vêm de `/api/orchestrator/*`
- **Clique no estágio** → `ensure-open` (sobe API/front se preciso) e abre a UI (Receptor `:4200`, Arquivador `:4210`)
- Auth usuário do dashboard ainda não existe; a proteção serviço-a-serviço é a API key interna no BFF ↔ monitores
