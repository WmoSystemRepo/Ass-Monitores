# Passo a passo — Monitor Arquivador CT-e

## 1. API

`Arquivador.Api/Monitor.sln` → F5 · HTTP `:5020` · Swagger `:7126`.

```powershell
cd Arquivador.Api
dotnet run --project src/Monitor.Api --launch-profile https
```

Key: `dev-cte-chain-key` · CodServico **3**.

## 2. Front

```powershell
cd Frontend
npm.cmd install
npm.cmd start
# http://localhost:4210
```

## 3. Ligar

UI → Ligar Arquivador (DevHost + `Executar=1`). Ver [../README.md](../README.md).
