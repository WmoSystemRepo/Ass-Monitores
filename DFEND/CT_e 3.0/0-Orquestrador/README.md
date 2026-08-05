# Orquestrador CT-e

Dashboard da cadeia CT-e (Receptor → Arquivador → Sintetizador → Analisador → Integrador → Carga).

**Local canônico (relativo ao clone):** `CT_e/0-Orquestrador` — o prefixo da máquina é irrelevante.

Contrato único em **Development**, **Homologacao** e **Production**: registry por `Id`, `BaseUrl` por config/env, header `X-Cte-Internal-Api-Key`, resiliência HTTP.

## Estrutura

```text
0-Orquestrador/
  Frontend/            # Nx cte-orquestrador :4220
  Orquestrador.Api/    # BFF :5000 / Swagger :7100
  Dockerfile
  Dockerfile.front
  docker/
  Doc/
  README.md
```

Compose da cadeia (raiz CT_e): `../docker-compose.chain.yml` (APIs + Fronts + gateway :8080)

## Ambientes

| Ambiente | Config | API key |
|----------|--------|---------|
| Development | `appsettings.Development.json` (localhost) | `dev-cte-chain-key` (local) |
| Homologacao | env + `appsettings.Homologacao.json` | secret (`Orchestrator__InternalApiKey`) |
| Production | env + `appsettings.Production.json` | secret (idem) |

Nos monitores: a **mesma** key em `Monitor__InternalApiKey`.

## Como rodar (Development)

Limpar cache e compilar (qualquer clone; prefixo da maquina irrelevante):

```powershell
cd Orquestrador.Api
# Se .\reset-build.ps1 falhar por ExecutionPolicy, use:
.\reset-build.cmd
# ou:
powershell -NoProfile -ExecutionPolicy Bypass -File .\reset-build.ps1
```

Depois abra `Orquestrador.sln` no Visual Studio.

1. Receptor.Api `:5010` e Arquivador.Api `:5020` (key DEV alinhada)
2. `Orquestrador.Api/Orquestrador.sln` → F5 (perfil **https** / **http**)
3. Front:

```powershell
cd Frontend
npm.cmd install
npm.cmd start
# http://localhost:4220
```

Passo a passo completo: [Doc/Passo a passo execução Orquestrador.md](Doc/Passo%20a%20passo%20execução%20Orquestrador.md)

## Docker

```powershell
cd ..
$env:CTE_INTERNAL_API_KEY = "dev-cte-chain-key"
docker compose -f docker-compose.chain.yml up --build
```

Sobe Orquestrador + Receptor + Arquivador (**API e Front**) e gateway `:8080`.  
Entre containers: DNS (`http://monitor-receptor-api:5010`).  
`LocalDev.EnsureBeforeCascade=false` no container — Ligar só fala HTTP.  
Worker fiscal no host Windows (`PreferLocalProcess=false`).

## Homolog / Produção (resumo)

```text
ASPNETCORE_ENVIRONMENT=Homologacao   # ou Production
Orchestrator__InternalApiKey=<secret>
Orchestrator__Systems__0__BaseUrl=https://...
Orchestrator__Systems__1__BaseUrl=https://...
Monitor__InternalApiKey=<mesmo-secret>
```

Front: `/config.json` ou `window.__CTE_ORQ_API_BASE__` com a URL do Orquestrador do ambiente.

Health: `/health` · `/health/ready` · `/api/chain/health`

## Ligar / Desligar

- **Ligar** — (1) API + Front ready (DEV: spawn se `EnsureBeforeCascade`; Docker: já no compose) (2) `service/start` + poll até Running — ordem `Order`/`DependsOn`, fail-fast parcial
- **Desligar** — stop na ordem inversa
- Sistemas sem monitor (`Enabled=false`) → `disabled`
- Estados oficiais: disabled / offline / starting / running / stopping / stopped / failed / unknown
- Plugar sistema novo: [Doc/ONBOARDING_MICROSERVICO.md](Doc/ONBOARDING_MICROSERVICO.md)

## Navegação entre monitores

No dashboard da cadeia (`:4220`), **clicar em um estágio** abre **in-app** o monitor rico daquele serviço:

`http://localhost:4220/monitores/{id}`

| Sistema | Id rota | UI (Angular único) |
|---------|---------|---------------------|
| Receptor | `receptor` | `/monitores/receptor` |
| Arquivador | `arquivador` | `/monitores/arquivador` |
| Sintetizador | `sintetizador` | `/monitores/sintetizador` |
| Analisador | `analisador` | `/monitores/analisador` |
| Integrador | `integrador` | `/monitores/integrador` |
| Carga | `carga` | `/monitores/carga` |

Cada monitor reutiliza a anatomia/animações do **CT_e 2.0**, consumindo `/api/monitores/{id}/*` no Orquestrador `:5000` (lib `Frontend/libs/service-monitors`).  
Link “front legado” / `FrontendUrl` (se existir) é **opcional** — a operação principal é o monitor unificado.

No Ligar (DEV): paths do registry sobem engines/DevHosts em silêncio. Em Docker/Homolog/Prod: serviços já online via container/deploy.

## Paleta

Indigo / violet / lime / fuchsia (distinta de Receptor cyan e Arquivador âmbar).

## Documentação

- [Onboarding microserviço (plugar sistema + Docker)](Doc/ONBOARDING_MICROSERVICO.md)
- [Documentação técnica (contrato, ambientes, go-live)](Doc/Documentacao_Orquestrador_CTe.md)
- [Passo a passo](Doc/Passo%20a%20passo%20execução%20Orquestrador.md)
