# Orquestrador CT-e

Dashboard da cadeia CT-e (Receptor → Arquivador → Sintetizador → Analisador → Integrador → Carga).

**Clone canônico (relativo):** `...\DFEND\CT_e 3.0\0-Orquestrador`  
O prefixo da máquina (`C:\Users\...`) é irrelevante. **Não** use `Ass-Monitores\Ass-Monitores` (pasta duplicada) nem projeto só na Lixeira.

Contrato único em **Development**, **Homologacao** e **Production**: registry por `Id`, `BaseUrl` por config/env, header `X-Cte-Internal-Api-Key`, resiliência HTTP.

## Início rápido (qualquer PC — sem copiar caminho)

1. Feche o Visual Studio.
2. No Explorer, abra a pasta `0-Orquestrador`.
3. Duplo clique em **`LIMPAR-E-BUILDAR.cmd`** (limpa cache + restore + build).
4. Duplo clique em **`ABRIR-SOLUTION.cmd`** (abre a `.sln` certa).

| Arquivo | Função |
|---------|--------|
| `LIMPAR-E-BUILDAR.cmd` | Limpa `bin`/`obj`/`.vs`/`_artifacts`, valida estrutura, `dotnet restore` + `build` |
| `ABRIR-SOLUTION.cmd` | Abre `Orquestrador.Api\Orquestrador.sln` no Visual Studio |
| `PROCURAR-E-CONSERTAR.cmd` | Procura clone **válido** no disco (ignora Lixeira/Temp; exige `LIMPAR-E-BUILDAR.cmd`) |
| `COMO-USAR.txt` | Instruções curtas em texto |

Detalhes: [Doc/DEV_PORTATIL.md](Doc/DEV_PORTATIL.md) · [COMO-USAR.txt](COMO-USAR.txt)

## Estrutura

```text
0-Orquestrador/
  LIMPAR-E-BUILDAR.cmd     # one-click limpar + build
  ABRIR-SOLUTION.cmd       # one-click abrir .sln
  PROCURAR-E-CONSERTAR.cmd # one-click achar clone válido
  COMO-USAR.txt
  Directory.Build.props    # bin/obj curtos em _artifacts (evita MAX_PATH)
  .gitignore               # nunca versionar bin/obj/_artifacts
  Frontend/                # Nx cte-orquestrador :4220
  Orquestrador.Api/        # BFF :5000 / Swagger :7100
  engines/                 # DevHosts Windows (receptor…carga)
  libs/resgate/            # Resgate CT-e AN
  tools/                   # fix-dev.ps1, verify-structure.ps1, build-devhosts.ps1
  _artifacts/              # gerado local (gitignored)
  Doc/
  README.md
```

Compose da cadeia (quando existir na raiz do monorepo): `docker-compose.chain.yml` (APIs + Fronts + gateway :8080).

## Paths portáteis (regra de ouro)

- Referências de projeto são **relativas** (`.csproj` / `.sln`).
- Build gera saída em `0-Orquestrador\_artifacts\` (paths curtos; evita limite ~260 do Windows).
- **Nunca** configure `LocalDev:RepoRoot` com caminho absoluto de outra máquina.
- **Nunca** copie `bin` / `obj` / `_artifacts` entre PCs.
- Discovery de engines: pasta `0-Orquestrador` (tem `engines` + `Orquestrador.Api`), independente do nome `CT_e` / `CT_e 3.0`.

## Ambientes

| Ambiente | Config | API key |
|----------|--------|---------|
| Development | `appsettings.Development.json` (localhost) | `dev-cte-chain-key` (local) |
| Homologacao | env + `appsettings.Homologacao.json` | secret (`Orchestrator__InternalApiKey`) |
| Production | env + `appsettings.Production.json` | secret (idem) |

Nos monitores: a **mesma** key em `Monitor__InternalApiKey`.

## Como rodar (Development)

### Recomendado — one-click

Ver seção **Início rápido** acima.

### Manual (PowerShell)

```powershell
# a partir de 0-Orquestrador (nao use Prompt de Comando com sintaxe PowerShell)
powershell -NoProfile -ExecutionPolicy Bypass -File .\tools\fix-dev.ps1
# depois abra Orquestrador.Api\Orquestrador.sln
```

1. `Orquestrador.Api/Orquestrador.sln` → F5 (perfil **https** / **http**)
2. Front:

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

Sobe Orquestrador (+ monitores no compose) e gateway `:8080`.  
Entre containers: DNS. `LocalDev.EnsureBeforeCascade=false` no container — Ligar só fala HTTP.  
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
- **Desligar** — stop na ordem inversa (`Executar=0` + parar processo). **Não esvazia filas**: backlog permanece; tela pode mostrar Fase **Parada** com badge **NA FILA** e “Com fila” > 0 (esperado). Ver [Doc §14](Doc/Documentacao_Orquestrador_CTe.md)
- Sistemas sem monitor (`Enabled=false`) → `disabled`
- Estados oficiais: disabled / offline / starting / running / stopping / stopped / failed / unknown
- Plugar sistema novo: [Doc/ONBOARDING_MICROSERVICO.md](Doc/ONBOARDING_MICROSERVICO.md)

## Navegação entre monitores

No **Dashboard** da cadeia (`:4220`), **clicar em um estágio** abre **in-app** o monitor rico daquele serviço:

`http://localhost:4220/monitores/{id}`

| Sistema | Id rota | UI (Angular único) |
|---------|---------|---------------------|
| Receptor | `receptor` | `/monitores/receptor` |
| Arquivador | `arquivador` | `/monitores/arquivador` |
| Sintetizador | `sintetizador` | `/monitores/sintetizador` |
| Analisador | `analisador` | `/monitores/analisador` |
| Integrador | `integrador` | `/monitores/integrador` |
| Carga | `carga` | `/monitores/carga` |

Cada monitor reutiliza a anatomia/animações do **CT_e 2.0**, com push **SignalR** (`/hubs/monitor`) e fallback REST em `/api/monitores/{id}/*` no Orquestrador `:5000` (lib `Frontend/libs/service-monitors`).  
Link “front legado” / `FrontendUrl` (se existir) é **opcional** — a operação principal é o monitor unificado.

No Ligar (DEV): paths do registry sobem engines/DevHosts em silêncio. Em Docker/Homolog/Prod: serviços já online via container/deploy.

### UX recente (resumo)

- Dashboard: foco em **AGORA** e profundidade de fila; medidor sobe/desce; boot visual ao Ligar; após Desligar, **Parada + NA FILA** com backlog é esperado
- **Todos os monitores** (R→C): Mais informações 2×2 com **Saúde dos bancos** (`connectionHealth` + `tableHealth`); catálogo de erros leigo + **Copiar texto**; avisos; fila/boot
- Confirmações via `ConfirmDialog` (shared-ui)

Detalhes: [Doc/Documentacao_Orquestrador_CTe.md](Doc/Documentacao_Orquestrador_CTe.md) §8.2 e §14.

## Paleta

Indigo / violet / lime / fuchsia (distinta de Receptor cyan e Arquivador âmbar).

## Documentação

- [Dev portátil / one-click / troubleshooting de path](Doc/DEV_PORTATIL.md)
- [Onboarding microserviço (plugar sistema + Docker)](Doc/ONBOARDING_MICROSERVICO.md)
- [Documentação técnica (contrato, ambientes, go-live)](Doc/Documentacao_Orquestrador_CTe.md)
- [Passo a passo](Doc/Passo%20a%20passo%20execução%20Orquestrador.md)
- [COMO-USAR.txt](COMO-USAR.txt)
