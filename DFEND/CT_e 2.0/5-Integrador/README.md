# 5-Integrador — Monitor CT-e + Windows Service

Pacote do **DFEND_CTe_Integrador** (CodServico **7**) com Monitor companion (contrato v1.3).

## Estrutura

```text
5-Integrador/
├── dfend-cte-integrador-windowsservices/   # Windows Service (.NET FX 4.7)
├── Integrador.Api/                         # BFF Monitor (.NET 8) :5050
├── Frontend/                               # Angular Nx :4250
├── tools/Integrador.DevHost/               # Host POC (StartDebug)
└── Doc/
```

## DEV rápido

1. API: `dotnet run --project Integrador.Api/src/Monitor.Api --launch-profile http`
2. Front: `cd Frontend && npm install && npm start`
3. No painel: **Ligar Integrador CT-e** (sobe DevHost + `Executar=1`)

Auth Orquestrador: header `X-Cte-Internal-Api-Key: dev-cte-chain-key`

## Identidade

| Campo | Valor |
|-------|--------|
| serviceId | `dfend-cte-monitor-integrador` |
| domain | `integrador` |
| API / UI / Swagger | 5050 / 4250 / 7156 |
| Accent | coral `#ea580c` |
