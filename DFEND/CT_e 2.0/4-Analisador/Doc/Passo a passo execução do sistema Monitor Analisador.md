# Passo a passo — Monitor Analisador CT-e

## Pré-requisitos

- .NET 8 SDK + VS 2022
- Node.js LTS (`npm.cmd` no PowerShell)
- SQL Server DEV (BD do estágio / sintético conforme appsettings)
- Compilar `tools\Analisador.DevHost` (Debug) ao menos uma vez

## 1. API

1. Abrir `Analisador.Api/Monitor.sln`
2. Startup **Monitor.Api** · perfil **http** ou **https**
3. F5 → HTTP `http://localhost:5040` · Swagger HTTPS `https://localhost:7146/swagger`

```powershell
cd Analisador.Api
dotnet run --project src/Monitor.Api --launch-profile http
```

Confirmar em `appsettings.Development.json`:

- `CodServicoAnalisador: 6`
- `InternalApiKey: dev-cte-chain-key` (alinhar com Orquestrador)
- path do DevHost

## 2. Front

```powershell
cd Frontend
npm.cmd install
npm.cmd start
# http://localhost:4240
```

## 3. Ligar

Na UI do Monitor: **Ligar Analisador CT-e**.

Isso sobe o DevHost e seta `Executar=1`. **Desligar** encerra o host e `Executar=0`.

> F5 só no exe original **não** alimenta o Debug online. Use Ligar pelo Monitor.

## 4. Orquestrador

Com a cadeia: incluir `analisador` no registry (`BaseUrl` `:5040`, `FrontendUrl` `:4240`, `Enabled: true`) e mesma API key.
