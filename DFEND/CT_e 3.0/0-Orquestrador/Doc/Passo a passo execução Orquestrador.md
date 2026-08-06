# Passo a passo — Orquestrador CT-e

Cobre **Development**, smoke **Homologacao** e **Docker**. Detalhes de contrato: [Documentacao_Orquestrador_CTe.md](Documentacao_Orquestrador_CTe.md).  
Paths / one-click / troca de PC: [DEV_PORTATIL.md](DEV_PORTATIL.md).

## Pré-requisitos

- .NET 8 SDK + Visual Studio 2022
- Node.js **20, 22 ou 24** LTS (`.nvmrc` = 24; Nx 21 + Angular 19)
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

1. UI indigo/violet — menu **Dashboard** (cadeia) e Resgate
2. Seis símbolos **R A S An I C** com badges AGORA / fila / ativo / parado
3. **Ligar as filas** (só no header) → sobe engines (se preciso) → sobe workers **e** grava `Executar=1` na ordem (Receptor → … → Carga). Não existe estado “pausado” na cascata: ligado = processo + trabalho ativo. Estações animam em cascata durante o start.
4. **Clique em um estágio** → abre `/monitores/{servico}` no mesmo Angular (anatomia/animações CT_e 2.0; push SignalR + badge SignalR/REST)
5. No monitor: Ligar/Desligar **daquele** serviço; threads/logs/tabelas/config/mais-informações nas rotas filhas
6. Receptor → **Mais informações**: 4 cards na tela; em erro SQL use **Ver erro** para o texto original
7. **Desligar filas** → confirmação modal → `Executar=0` + para processos na ordem inversa (sempre tenta parar; não depende de health/ready). **Não limpa a fila**: se ainda houver documentos pendentes, a fase fica **Parada** com estágios em **NA FILA** (âmbar) e contadores “Com fila” / “Arquivos” > 0 — isso é normal. Religue com **Ligar as filas** para consumir o backlog.

> Se engines não ficarem online no Ligar, a barra mostra a falha e **as filas daquele sistema não ligam**.  
> Plugar outro sistema / Docker: [ONBOARDING_MICROSERVICO.md](ONBOARDING_MICROSERVICO.md).  
> Dúvida “Parada + NA FILA”: [Documentacao_Orquestrador_CTe.md §14](Documentacao_Orquestrador_CTe.md).

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
- [ ] Cascade Ligar / Desligar na ordem da cadeia (CTA Ligar só no header)
- [ ] Clique no estágio → `/monitores/{servico}` com anatomia (não tela JSON)
- [ ] Badge do monitor indica SignalR (ou REST em fallback)
- [ ] Start/Stop e threads/logs no monitor do serviço
- [ ] Receptor → Mais informações: 4 cards visíveis; **Ver erro** em linha de erro SQL
- [ ] Mesmo padrão de Mais informações / Ver erro nos outros monitores (A/S/An/I/C)
- [ ] Com processo parado, card Avisos mostra alerta (após restart da API com `BuildHealthAlerts`)
- [ ] Snapshot sem `unauthorized` com monitores/engines no ar
