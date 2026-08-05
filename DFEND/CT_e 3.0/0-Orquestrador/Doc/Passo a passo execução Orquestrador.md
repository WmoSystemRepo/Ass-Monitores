# Passo a passo — Orquestrador CT-e

Cobre **Development**, smoke **Homologacao** e **Docker**. Detalhes de contrato: [Documentacao_Orquestrador_CTe.md](Documentacao_Orquestrador_CTe.md).

## Pré-requisitos

- .NET 8 SDK + Visual Studio 2022
- Node.js **20 ou 22** LTS (não usar Node 24 com Nx 20)
- Para Ligar/Desligar real: Receptor (`:5010`) e Arquivador (`:5020`) no ar
- Mesma API key nos três lados (DEV padrão: `dev-cte-chain-key`)

## A) Development — Visual Studio

### 1) Monitores (obrigatório para cascata real)

1. Abrir e F5 **Receptor.Api** (perfil http/https → `:5010`)  
   Confirmar `Monitor:InternalApiKey` = `dev-cte-chain-key` no `appsettings.Development.json`
2. Abrir e F5 **Arquivador.Api** → `:5020`  
   Confirmar a mesma key
3. Checagem rápida no browser:
   - `http://localhost:5010/health/ready`
   - `http://localhost:5020/health/ready`

### 2) Orquestrador.Api

1. Abrir `Orquestrador.Api/Orquestrador.sln`
2. Startup: **Orquestrador.Api** · perfil **https** (ou **http**)
3. **F5** → Swagger `https://localhost:7100/swagger` · HTTP `http://localhost:5000`

Checagens:

| Endpoint | Esperado |
|----------|----------|
| `GET /health/ready` | `status: ready`, `hasInternalApiKey: true` |
| `GET /api/orchestrator/info` | `domain: orquestrador`, systems com BaseUrl |
| `GET /api/chain/health` | receptor/arquivador `online` (se monitores up) |
| `GET /api/orchestrator/snapshot` | 6 sistemas; R/A enabled; demais `disabled` |

Se aparecer `unauthorized`: key do Orquestrador ≠ key do monitor.  
Se aparecer `offline`: monitor não está rodando na BaseUrl.

### 3) Front

```powershell
cd Frontend   # pasta: CT_e\0-Orquestrador\Frontend (prefixo da máquina irrelevante)
npm.cmd install
npm.cmd start
```

Abrir `http://localhost:4220` (Ctrl+F5).  
API base: `public/config.json` → `http://localhost:5000`.

### 4) Fluxo operacional

1. UI indigo/violet (visão da cadeia)
2. Seis símbolos **R A S An I C**
3. **Ligar cadeia CT-e** → garante engines online (silencioso) → sobe workers na ordem
4. **Clique em um estágio** → abre `/monitores/{servico}` no mesmo Angular (anatomia/animações CT_e 2.0)
5. No monitor: Ligar/Desligar **daquele** serviço; threads/logs/tabelas/config nas rotas filhas
6. **Desligar cadeia** → para na ordem inversa

> Se engines não ficarem online no Ligar, a barra mostra a falha e **o serviço daquele sistema não liga**.  
> Plugar outro sistema / Docker: [ONBOARDING_MICROSERVICO.md](ONBOARDING_MICROSERVICO.md).  
> SDD: `Assefaz\CT_e\.cursor\SDD\Monitor Unificado CT-e`.

## B) Homologacao (smoke local)

Perfil VS **Homologacao** no Orquestrador:

- `ASPNETCORE_ENVIRONMENT=Homologacao`
- injeta key placeholder + BaseUrl localhost (ver `launchSettings.json`)

Nos monitores, alinhar:

```text
ASPNETCORE_ENVIRONMENT=Homologacao
Monitor__InternalApiKey=<mesma key do Orquestrador>
```

Em Homolog real: keys e BaseUrl **só** via secret/pipeline (não git).

## C) Docker — cadeia completa (API + Front + gateway)

Na raiz `CT_e`:

```powershell
$env:CTE_INTERNAL_API_KEY = "dev-cte-chain-key"
docker compose -f docker-compose.chain.yml up --build
```

- Orquestrador: API `:5000` · Front `:4220`
- Receptor: API `:5010` · Front `:4200`
- Arquivador: API `:5020` · Front `:4210`
- Gateway: `http://localhost:8080`
- Entre containers: DNS `monitor-receptor-api` / `monitor-arquivador-api`
- Ligar **não** chama Docker — só `/health/ready` + `service/start`
- `PreferLocalProcess=false`; worker no host Windows
- Injete `MONITOR_*_CONNECTION_STRING` (SQL auth) para `/health/ready` verde

Plugar sistema / Registry: [ONBOARDING_MICROSERVICO.md](ONBOARDING_MICROSERVICO.md).

## Aceite

- [ ] Pacote em `0-Orquestrador`
- [ ] UI `:4220` · API `:5000`
- [ ] Key DEV alinhada Orquestrador ↔ engines
- [ ] `/health/ready` e `/api/chain/health` OK
- [ ] Cascade Ligar / Desligar na ordem da cadeia
- [ ] Clique no estágio → `/monitores/{servico}` com anatomia (não tela JSON)
- [ ] Start/Stop e threads/logs no monitor do serviço
- [ ] Snapshot sem `unauthorized` com monitores no ar
