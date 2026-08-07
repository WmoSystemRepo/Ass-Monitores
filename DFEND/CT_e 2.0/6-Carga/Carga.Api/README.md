# Carga.Api — BFF Monitor CT-e Carga

Pasta irmã de `Frontend/`, `tools/Carga.DevHost/` e `dfend-cte-carga-windowsservices/`, dentro de **`6-Carga/`**.

## Rodar (VS 2022)

1. Abrir `Monitor.sln`
2. Startup **Monitor.Api** · perfil **https** ou **http**
3. F5 → `https://localhost:7166/swagger`
4. HTTP (front): `http://localhost:5080`

```powershell
cd Carga.Api
dotnet run --project src/Monitor.Api --launch-profile https
```

## Config DEV

`src/Monitor.Api/appsettings.Development.json`:

- Connection strings → SQL DEV recepção + sintético (conforme appsettings)
- `PreferLocalProcess: true`
- `InternalApiKey: dev-cte-chain-key`
- `CodServicoCarga: 99`
- `CargaExeRelativePath: tools\Carga.DevHost\bin\Debug\Carga.DevHost.exe`
- `SnapshotIntervalMs: 1000`
- `RecentLogsTake: 300`

Cliente: `dotnet user-secrets set "Monitor:ConnectionString" "..."` (não commitar segredo).  
Homolog/Prod: `PreferLocalProcess=false` + `Monitor__InternalApiKey` via secret store.

## Contratos (v1.3)

| Endpoint | Nota |
|----------|------|
| `GET /api/monitor/info` | Identidade · `domain=carga` |
| `GET /api/monitor/snapshot` | telemetria / liveTrace / tableHealth |
| `GET /api/monitor/logs` | afterSeq / take |
| `GET /api/monitor/tables/{key}` | tabelas vigiladas |
| `GET /api/monitor/service/status` | |
| `POST .../start` · `POST .../stop` | DevHost + `UPDATE Executar` (cod **99**) |
| Hub `/hubs/monitor` | `snapshot`, `logsAppend` |
| `GET /health` · `/health/live` | Liveness (público) |
| `GET /health/ready` | Readiness · 503 se down |
| `/api/v1/monitor/*` | Aliases versionados |

Auth em `/api/monitor/*`: header `X-Cte-Internal-Api-Key`.  
Doc completa: `../Doc/Documentacao_Monitor_Carga_Fiscal_CTe.md` · contrato: `../Doc/CONTRATO_MICROSERVICO_MONITOR.md`.

## Testes

```powershell
cd Carga.Api
dotnet test
```

Host POC: `../tools/Carga.DevHost`. Negócio: download pontual por chave — ver também `../README.md` e `../../7-Resgate/`.
