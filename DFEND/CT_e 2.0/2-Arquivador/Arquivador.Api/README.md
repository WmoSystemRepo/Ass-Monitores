# Arquivador.Api — BFF Monitor CT-e Arquivador

Pasta irmã de `Frontend/`, `tools/Arquivador.DevHost/` e `dfend-cte-arquivador-windowsservices/`, dentro de **`2-Arquivador/`**.

## Rodar (VS 2022)

1. Abrir `Monitor.sln`
2. Startup **Monitor.Api** · perfil **https**
3. F5 → `https://localhost:7126/swagger`
4. HTTP (front): `http://localhost:5020`

```powershell
cd Arquivador.Api
dotnet run --project src/Monitor.Api --launch-profile https
```

## Config DEV

`src/Monitor.Api/appsettings.Development.json`:

- Connection string → SQL DEV `bd_cte_recepcao`
- `PreferLocalProcess: true`
- `InternalApiKey: dev-cte-chain-key`
- `CodServicoArquivador: 3`
- `ArquivadorExeRelativePath: tools\Arquivador.DevHost\bin\Debug\Arquivador.DevHost.exe`
- `SnapshotIntervalMs: 1000`
- `RecentLogsTake: 300`
- `ConnectionStringSintetico` opcional (filas destino em outro BD)

Cliente: `dotnet user-secrets set "Monitor:ConnectionString" "..."` (não commitar segredo).  
Homolog/Prod: `PreferLocalProcess=false` + `Monitor__InternalApiKey` via secret store.

## Contratos (v1.3)

| Endpoint | Nota |
|----------|------|
| `GET /api/monitor/info` | Identidade + endpoints · `domain=arquivador` |
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
Doc completa: `../Doc/Documentacao_Monitor_Arquivador_Fiscal_CTe.md` · contrato: `../Doc/CONTRATO_MICROSERVICO_MONITOR.md`.

## Testes

```powershell
cd Arquivador.Api
dotnet test
```

Host POC (Debug online): `../tools/Arquivador.DevHost/Program.cs` → `monitor-live.log`.
