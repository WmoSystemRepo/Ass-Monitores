# Sintetizador.Api — BFF Monitor CT-e Sintetizador

Pasta irmã de `Frontend/`, `tools/Sintetizador.DevHost/` e `dfend-cte-sintetizador-windowsservices/`, dentro de **`2-Sintetizador/`**.

## Rodar (VS 2022)

1. Abrir `Monitor.sln`
2. Startup **Monitor.Api** · perfil **https**
3. F5 → `https://localhost:7136/swagger`
4. HTTP (front): `http://localhost:5030`

```powershell
cd Sintetizador.Api
dotnet run --project src/Monitor.Api --launch-profile https
```

## Config DEV

`src/Monitor.Api/appsettings.Development.json`:

- Connection string → SQL DEV `bd_cte_recepcao`
- `PreferLocalProcess: true`
- `InternalApiKey: dev-cte-chain-key`
- `CodServicoSintetizador: 3`
- `SintetizadorExeRelativePath: tools\Sintetizador.DevHost\bin\Debug\Sintetizador.DevHost.exe`
- `SnapshotIntervalMs: 1000`
- `RecentLogsTake: 300`
- `ConnectionStringSintetico` opcional (filas destino em outro BD)

Cliente: `dotnet user-secrets set "Monitor:ConnectionString" "..."` (não commitar segredo).  
Homolog/Prod: `PreferLocalProcess=false` + `Monitor__InternalApiKey` via secret store.

## Contratos (v1.3)

| Endpoint | Nota |
|----------|------|
| `GET /api/monitor/info` | Identidade + endpoints · `domain=sintetizador` |
| `GET /api/monitor/snapshot` | + `liveTrace`, `tableHealth` (8), filas destino |
| `GET /api/monitor/logs` | afterSeq / take |
| `GET /api/monitor/tables/{key}` | 8 keys (fila_entrada + 3 destinos) |
| `GET /api/monitor/service/status` | |
| `POST .../start` · `POST .../stop` | DevHost + `UPDATE Executar` (cod 3) |
| Hub `/hubs/monitor` | `snapshot`, `logsAppend` |
| `GET /health` · `/health/live` | Liveness (público) |
| `GET /health/ready` | Dual ping primary + sintético · 503 se down |
| `/api/v1/monitor/*` | Aliases versionados |

Auth em `/api/monitor/*`: header `X-Cte-Internal-Api-Key`.  
Doc completa: `../Doc/Documentacao_Monitor_Sintetizador_Fiscal_CTe.md` · contrato: `../Doc/CONTRATO_MICROSERVICO_MONITOR.md`.

## Testes

```powershell
cd Sintetizador.Api
dotnet test
```

Host POC (Debug online): `../tools/Sintetizador.DevHost/Program.cs` → `monitor-live.log`.
