# Passo a passo — Monitor Sintetizador CT-e

## 1. API

`Sintetizador.Api/Monitor.sln` → perfil `http` (`:5030`) / Swagger `:7136`.

## 2. Front

```powershell
cd Frontend
npm.cmd install
npm.cmd start
# http://localhost:4230
```

## 3. Ligar

UI → Ligar Sintetizador (DevHost + `Executar=1`, cod **8**).

Ver [../README.md](../README.md).
