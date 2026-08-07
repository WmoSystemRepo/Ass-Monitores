# Passo a passo — Monitor Carga CT-e

## 1. API

`Carga.Api/Monitor.sln` → perfil `http` (`:5080`) / Swagger `:7166`.

Key DEV: `dev-cte-chain-key` · CodServico **99**.

## 2. Front

```powershell
cd Frontend
npm.cmd install
npm.cmd start
# http://localhost:4260
```

## 3. Ligar

UI → Ligar Carga (DevHost + `Executar=1` no cod **99**).

Para Resgate: também `ExecutarAuto=1` conforme runbook do Resgate.

Ver [../README.md](../README.md) · [../../7-Resgate/README.md](../../7-Resgate/README.md).
