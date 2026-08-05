# Monitor CT-e Integrador (POC local · DEV)

Raiz do pacote: **`5-Integrador/`**. Esta pasta = app Nx `cte-integrador`.

Plano / ata: `CT_e/.cursor/plans/monitor_realtime_receptor_fded97f7.plan.md`.

## Como rodar

### API (BFF) — fora desta pasta

Abrir `../Integrador.Api/Monitor.sln` no VS 2022 · perfil **https** · F5 → Swagger `https://localhost:7156/swagger`.  
Front consome `http://localhost:5050`.

### Front (esta pasta)

**Node.js:** use **20 LTS** ou **22 LTS** (arquivo `.nvmrc` = `20`). **Não use Node 24** — o Nx 20 falha no install (`supports-color` / `fs-constants` / post-install).

**Não use** `ng s` / `ng serve` no PowerShell — o `ng` global normalmente **não** está no PATH. Este app é **Nx**.

```powershell
# 1) na pasta Frontend (uma vez) — Node 20/22
node --version   # deve ser v20.x ou v22.x
npm.cmd install

# 2) subir (qualquer um destes)
npm.cmd start
# ou:
.\serve.cmd
# ou:
npx.cmd nx serve cte-integrador
```

`http://localhost:4250` — Ctrl+F5 após mudanças de UI.

Se aparecer erro de ExecutionPolicy no `.ps1`, use `serve.cmd` ou `npm.cmd start`.

#### Se `npm install` / `npm start` quebrar (`supports-color`, `nx.js` missing, EBUSY)

1. Feche Cursor/VS Code na pasta, Explorer e qualquer `node`/`nx` aberto.
2. Troque para Node **20** (nvm-windows / instalador oficial).
3. No PowerShell, **um comando por vez**:

```powershell
cd Frontend
Get-Process node -ErrorAction SilentlyContinue | Stop-Process -Force
Remove-Item -Recurse -Force node_modules
Remove-Item -Force package-lock.json -ErrorAction SilentlyContinue
npm.cmd cache clean --force
npm.cmd install
npm.cmd start
```

Não interrompa o `npm install` com Ctrl+C. Se `Remove-Item` der EBUSY/EPERM, reinicie o PC ou apague `node_modules` pelo Explorer e tente de novo.

## Menu / rotas

| Menu (texto grande) | Hint (texto pequeno) | Rota |
|---------------------|----------------------|------|
| Monitor | Visão operacional | `/` — fluxo + Ligar/Desligar + cards tabelas |
| Threads | Linhas de trabalho | `/threads` |
| Histórico | O que aconteceu | `/logs` |
| Tabelas | Banco em tempo real | `/tabelas` · `/tabelas/:key` |
| Configurações | Somente leitura · origem Integrador | `/config` |

Rota auxiliar (sem item de menu — link no fluxo do Monitor): `/mais-informacoes`.

## Endpoints (BFF)

- `GET /api/monitor/info`
- `GET /api/monitor/snapshot` — inclui `liveTrace`, `tableHealth` (8 keys)
- `GET /api/monitor/logs?afterSeq=&take=`
- `GET /api/monitor/tables/{key}`
- `GET /api/monitor/service/status`
- `POST /api/monitor/service/start` — DevHost + `Executar=1` (cod 3)
- `POST /api/monitor/service/stop` — `Executar=0` + encerra host
- Hub `/hubs/monitor` — `snapshot`, `logsAppend`
- `GET /health` · `/health/ready` (dual SQL)

## Observações

- Dados reais SQL **DEV** — sem mock
- Windows Service original intocado; Ligar usa `tools/Integrador.DevHost`
- Debug online só com Ligar pelo Monitor (`monitor-live.log`)
- Paths relativos a `5-Integrador/`
