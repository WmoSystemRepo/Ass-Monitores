# Passo a passo — Monitor Receptor CT-e

## Pré-requisitos

- .NET 8 + VS 2022 · Node LTS · SQL `bd_cte_recepcao`
- Build Debug de `tools\Receptor.DevHost`

## 1. API

Abrir `Receptor.Api/Monitor.sln` → F5 (perfil **https**).  
Swagger: `https://localhost:7116/swagger` · Front usa `http://localhost:5010`.

```powershell
cd Receptor.Api
dotnet run --project src/Monitor.Api --launch-profile https
```

Key DEV: `Monitor:InternalApiKey` = `dev-cte-chain-key`.

## 2. Front

```powershell
cd Frontend
npm.cmd install
npm.cmd start
# http://localhost:4200
```

## 3. Ligar

UI → **Ligar Receptor CT-e** (DevHost + `Executar=1`). Desligar: host off + `Executar=0`.

Detalhes: [../README.md](../README.md).
