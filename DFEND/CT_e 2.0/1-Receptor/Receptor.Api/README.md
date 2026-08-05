# Receptor.Api — BFF Monitor CT-e Receptor

Pasta irmã de `Frontend/`, `tools/Receptor.DevHost/` e `dfend-cte-receptor-windowsservices/`, dentro de **`1-Receptor/`**.

## Rodar (VS 2022)

1. Abrir `Monitor.sln`
2. Startup **Monitor.Api** · perfil **https**
3. F5 → `https://localhost:7116/swagger`
4. HTTP (front): `http://localhost:5010`

```powershell
cd Receptor.Api
dotnet run --project src/Monitor.Api --launch-profile https
```

## Config DEV

`src/Monitor.Api/appsettings.Development.json`:

- Connection string → SQL DEV `bd_cte_recepcao`
- `PreferLocalProcess: true`
- `InternalApiKey: dev-cte-chain-key`
- `ReceptorExeRelativePath: tools\Receptor.DevHost\bin\Debug\Receptor.DevHost.exe`
- `SnapshotIntervalMs: 1000`
- `RecentLogsTake: 300`

Cliente: `dotnet user-secrets set "Monitor:ConnectionString" "..."` (não commitar segredo).  
Homolog/Prod: `PreferLocalProcess=false` + `Monitor__InternalApiKey` via secret store.

## Contratos (v1.3)

| Endpoint | Nota |
|----------|------|
| `GET /api/monitor/info` | Identidade + endpoints |
| `GET /api/monitor/snapshot` | + `liveTrace`, `tableHealth`, `sessionStartUtc` |
| `GET /api/monitor/logs` | afterSeq / take |
| `GET /api/monitor/tables/{key}` | servico \| configuracao \| temporaria \| log \| fila |
| `GET /api/monitor/service/status` | |
| `POST .../start` · `POST .../stop` | DevHost + `UPDATE Executar` |
| Hub `/hubs/monitor` | `snapshot`, `logsAppend` |
| `GET /health` · `/health/live` | Liveness (público) |
| `GET /health/ready` | SQL readiness · 503 se down |
| `/api/v1/monitor/*` | Aliases versionados |

Auth em `/api/monitor/*`: header `X-Cte-Internal-Api-Key`.  
Doc completa: `../Doc/Documentacao_Monitor_Receptor_Fiscal_CTe.md` · contrato: `../Doc/CONTRATO_MICROSERVICO_MONITOR.md`.

## Testes

```powershell
cd Receptor.Api
dotnet test
```

Host POC (Debug online): `../tools/Receptor.DevHost/Program.cs` → `monitor-live.log`.  
Config do serviço original (referência): `../dfend-cte-receptor-windowsservices/DFEND_CTe_Receptor/AppConfig/Desenvolvimento/`.
