# Analisador.Api — BFF Monitor CT-e Analisador

Pasta irmã de `Frontend/`, `tools/Analisador.DevHost/` e `dfend-cte-analisador-windowsservices/`, dentro de **`4-Analisador/`**.

## Rodar (VS 2022)

1. Abrir `Monitor.sln`
2. Startup **Monitor.Api** · perfil **https** ou **http**
3. F5 → `https://localhost:7146/swagger`
4. HTTP (front): `http://localhost:5040`

```powershell
cd Analisador.Api
dotnet run --project src/Monitor.Api --launch-profile https
```

## Config DEV

`src/Monitor.Api/appsettings.Development.json`:

- Connection string → SQL DEV do estágio
- `PreferLocalProcess: true`
- `InternalApiKey: dev-cte-chain-key` (recomendado alinhar ao Orquestrador)
- `CodServicoAnalisador: 6`
- Path do DevHost (`tools\Analisador.DevHost\bin\Debug\Analisador.DevHost.exe`)
- `SnapshotIntervalMs: 1000`

Cliente: `dotnet user-secrets set "Monitor:ConnectionString" "..."` (não commitar segredo).  
Homolog/Prod: `PreferLocalProcess=false` + `Monitor__InternalApiKey` via secret store.

## Contratos (v1.3)

| Endpoint | Nota |
|----------|------|
| `GET /api/monitor/info` | Identidade · `domain=analisador` |
| `GET /api/monitor/snapshot` | telemetria / liveTrace / tableHealth |
| `GET /api/monitor/logs` | afterSeq / take |
| `GET /api/monitor/tables/{key}` | tabelas vigiladas |
| `GET /api/monitor/service/status` | |
| `POST .../start` · `POST .../stop` | DevHost + `UPDATE Executar` (cod 6) |
| Hub `/hubs/monitor` | `snapshot`, `logsAppend` |
| `GET /health` · `/health/live` | Liveness (público) |
| `GET /health/ready` | Readiness · 503 se down |
| `/api/v1/monitor/*` | Aliases versionados |

Auth em `/api/monitor/*`: header `X-Cte-Internal-Api-Key`.  
Doc completa: `../Doc/Documentacao_Monitor_Analisador_Fiscal_CTe.md` · contrato: `../Doc/CONTRATO_MICROSERVICO_MONITOR.md`.

## Testes

```powershell
cd Analisador.Api
dotnet test
```

Host POC (Debug online): `../tools/Analisador.DevHost`.
