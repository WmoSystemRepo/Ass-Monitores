# Passo a passo — Orquestrador CT-e (linha 2.0)

## Canônico (3.0)

Runbook completo / one-click:  
[CT_e 3.0 Passo a passo](../../../CT_e%203.0/0-Orquestrador/Doc/Passo%20a%20passo%20execução%20Orquestrador.md)

## DEV local (2.0)

1. Subir monitores necessários (ex.: Receptor `:5010` / `:4200`, Arquivador `:5020` / `:4210`) com a mesma key  
2. Limpar/compilar API:

```powershell
cd Orquestrador.Api
.\reset-build.cmd
```

3. Abrir `Orquestrador.sln` → F5 (http `:5000` / https Swagger)  
4. Front:

```powershell
cd Frontend
npm.cmd install
npm.cmd start
# http://localhost:4220
```

## Docker

```powershell
cd ..
$env:CTE_INTERNAL_API_KEY = "dev-cte-chain-key"
docker compose -f docker-compose.chain.yml up --build
```

Gateway `:8080`. Ver [../../README.md](../../README.md).
