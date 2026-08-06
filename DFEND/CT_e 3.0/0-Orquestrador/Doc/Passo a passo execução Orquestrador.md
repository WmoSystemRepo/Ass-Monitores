# Passo a passo — Orquestrador CT-e

Cobre **Development**, smoke **Homologacao** e **Docker**. Detalhes de contrato: [Documentacao_Orquestrador_CTe.md](Documentacao_Orquestrador_CTe.md).  
Paths / one-click / troca de PC: [DEV_PORTATIL.md](DEV_PORTATIL.md).

## Pré-requisitos

- .NET 8 SDK + Visual Studio 2022
- Node.js **20 ou 22** LTS (não usar Node 24 com Nx 20)
- Clone válido em `...\DFEND\CT_e 3.0\0-Orquestrador` (sem `Ass-Monitores` duplicado, sem Lixeira)
- Mesma API key nos lados que usarem monitor HTTP (DEV padrão: `dev-cte-chain-key`)

## A0) Limpar e abrir (recomendado — qualquer PC)

1. Feche o Visual Studio.  
2. No Explorer, pasta `0-Orquestrador`:  
   - Duplo clique **`LIMPAR-E-BUILDAR.cmd`** → espere OK  
   - Duplo clique **`ABRIR-SOLUTION.cmd`**  
3. Se não achar a pasta: **`PROCURAR-E-CONSERTAR.cmd`** (Desktop ou raiz Ass-Monitores).

Instruções curtas: [../COMO-USAR.txt](../COMO-USAR.txt).

## A) Development — Visual Studio

### 1) Orquestrador.Api

1. Abrir **apenas** via `ABRIR-SOLUTION.cmd` ou `Orquestrador.Api/Orquestrador.sln` do clone válido  
2. Startup: **Orquestrador.Api** · perfil **https** (ou **http**)  
3. **F5** → Swagger `https://localhost:7100/swagger` · HTTP `http://localhost:5000`

Checagens:

| Endpoint | Esperado |
|----------|----------|
| `GET /health/ready` | `status: ready`, `hasInternalApiKey: true` |
| `GET /api/orchestrator/info` | `domain: orquestrador`, systems com BaseUrl |
| `GET /api/chain/health` | sistemas enabled com status coerente |
| `GET /api/orchestrator/snapshot` | 6 sistemas da cadeia (+ resgate fora da cascata) |

Se aparecer `unauthorized`: key do Orquestrador ≠ key do monitor (quando HTTP fallback).  
Se aparecer `offline`: serviço/engine não está no ar na BaseUrl.

> Layout unificado: monitores ricos ficam em `/monitores/{id}` no mesmo front `:4220`; engines sobem via DevHost em `engines\` no Ligar (DEV).

### 2) Front

```powershell
cd Frontend   # pasta: ...\0-Orquestrador\Frontend (prefixo da máquina irrelevante)
npm.cmd install
npm.cmd start
```

Abrir `http://localhost:4220` (Ctrl+F5).  
API base: `public/config.json` → `http://localhost:5000`.

### 3) Fluxo operacional

1. UI indigo/violet (visão da cadeia)
2. Seis símbolos **R A S An I C**
3. **Ligar as filas** → sobe engines (se preciso) → sobe workers **e** grava `Executar=1` na ordem (Receptor → … → Carga). Não existe estado “pausado” na cascata: ligado = processo + trabalho ativo.
4. **Clique em um estágio** → abre `/monitores/{servico}` no mesmo Angular (anatomia/animações CT_e 2.0)
5. No monitor: Ligar/Desligar **daquele** serviço; threads/logs/tabelas/config nas rotas filhas
6. **Desligar filas** → `Executar=0` + para processos na ordem inversa (sempre tenta parar; não depende de health/ready)

> Se engines não ficarem online no Ligar, a barra mostra a falha e **as filas daquele sistema não ligam**.  
> Plugar outro sistema / Docker: [ONBOARDING_MICROSERVICO.md](ONBOARDING_MICROSERVICO.md).

## B) Homologacao (smoke local)

Perfil VS **Homologacao** no Orquestrador:

- `ASPNETCORE_ENVIRONMENT=Homologacao`
- injeta key placeholder + BaseUrl localhost (ver `launchSettings.json`)

Nos monitores (se HTTP), alinhar:

```text
ASPNETCORE_ENVIRONMENT=Homologacao
Monitor__InternalApiKey=<mesma key do Orquestrador>
```

Em Homolog real: keys e BaseUrl **só** via secret/pipeline (não git).

## C) Docker — cadeia completa (API + Front + gateway)

Na raiz do monorepo (quando o compose existir):

```powershell
$env:CTE_INTERNAL_API_KEY = "dev-cte-chain-key"
docker compose -f docker-compose.chain.yml up --build
```

- Orquestrador: API `:5000` · Front `:4220`
- Gateway (se houver): `http://localhost:8080`
- Ligar **não** chama Docker — só `/health/ready` + `service/start`
- `PreferLocalProcess=false`; worker no host Windows

Plugar sistema / Registry: [ONBOARDING_MICROSERVICO.md](ONBOARDING_MICROSERVICO.md).

## Aceite

- [ ] Pacote em `0-Orquestrador` (clone válido; não Lixeira)
- [ ] `LIMPAR-E-BUILDAR.cmd` concluiu OK neste PC
- [ ] UI `:4220` · API `:5000`
- [ ] `/health/ready` e `/api/chain/health` OK
- [ ] Cascade Ligar / Desligar na ordem da cadeia
- [ ] Clique no estágio → `/monitores/{servico}` com anatomia (não tela JSON)
- [ ] Start/Stop e threads/logs no monitor do serviço
- [ ] Snapshot sem `unauthorized` com monitores/engines no ar
