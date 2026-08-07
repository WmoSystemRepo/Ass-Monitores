# Documentação do sistema Ass-Monitores (DFEND CT-e)

Índice oficial do monorepo. Atualizado em **07/08/2026** (modo Apresentação do Orquestrador 3.0 + correções de QA).

## 1. O que é este repositório

Monorepo **DFEND** com monitores em tempo real da cadeia fiscal **CT-e** (Conhecimento de Transporte eletrônico):

```text
SEFAZ → Receptor → Arquivador → Sintetizador → Analisador → Integrador → Carga
                         ↑                                         ↑
                    Orquestrador (dashboard / Ligar-Desligar)   Resgate (chaves)
```

| Linha | Pasta | Papel |
|-------|-------|--------|
| **CT_e 3.0** (uso diário) | `DFEND/CT_e 3.0/0-Orquestrador` | Orquestrador unificado: BFF + front único + engines DevHost + Resgate embutido |
| **CT_e 2.0** (cadeia multi-app) | `DFEND/CT_e 2.0/` | Um pacote por estágio (API + Front + WS + DevHost) + gateway nginx |

**Regra:** para desenvolvimento do dia a dia, preferir **CT_e 3.0**. A linha **2.0** permanece como referência da cadeia desacoplada e do `docker-compose.chain.yml`.

## 2. Mapa de portas (Development)

| Sistema | CodServico | API | Front | Swagger HTTPS |
|---------|------------|-----|-------|---------------|
| Orquestrador | — | **5000** | **4220** | **7100** |
| Receptor | **2** | **5010** | **4200** | **7116** |
| Arquivador | **3** | **5020** | **4210** | **7126** |
| Sintetizador | **8** | **5030** | **4230** | **7136** |
| Analisador | **6** | **5040** | **4240** | **7146** |
| Integrador | **7** | **5050** | **4250** | **7156** |
| Carga | **99** | **5080** | **4260** | **7166** |
| Resgate | — | **5070** | UI em `/resgate` (Orq. `:4220`) | — |
| Gateway (Docker) | — | **8080** | — | — |

API key local: `dev-cte-chain-key` (`X-Cte-Internal-Api-Key` / `Monitor__InternalApiKey` / `Orchestrator__InternalApiKey`).

## 3. Arquitetura em uma frase

Cada estágio tem um **Windows Service fiscal** (congelado) + **DevHost** (POC sem InstallUtil) + **BFF Monitor** (.NET 8, SQL + SignalR) + **Front Nx/Angular**. O **Orquestrador** só fala o **contrato v1.3** (health / status / start / stop / snapshot); não grava estado do worker.

Fonte do estado: `Worker → Banco → Monitor.Api → Orquestrador`.

## 4. Índice de documentação por módulo

### Raiz

| Doc | Conteúdo |
|-----|----------|
| [README.md](README.md) | Início rápido CT_e 3.0 |
| [DOCUMENTACAO-SISTEMA.md](DOCUMENTACAO-SISTEMA.md) | Este índice + auditoria |
| [PROCURAR-E-CONSERTAR.cmd](PROCURAR-E-CONSERTAR.cmd) | Achar clone válido no disco |

### CT_e 3.0 — Orquestrador (canônico)

| Doc | Conteúdo |
|-----|----------|
| [CT_e 3.0/README.md](DFEND/CT_e%203.0/README.md) | Entrada da linha 3.0 |
| [0-Orquestrador/README.md](DFEND/CT_e%203.0/0-Orquestrador/README.md) | Visão geral, one-click, Ligar/Desligar, Apresentação |
| [Doc/Documentacao_Orquestrador_CTe.md](DFEND/CT_e%203.0/0-Orquestrador/Doc/Documentacao_Orquestrador_CTe.md) | Contrato, ambientes, go-live, **§8.5 modo Apresentação** |
| [Doc/ONBOARDING_MICROSERVICO.md](DFEND/CT_e%203.0/0-Orquestrador/Doc/ONBOARDING_MICROSERVICO.md) | Plugar estágio novo |
| [Doc/DEV_PORTATIL.md](DFEND/CT_e%203.0/0-Orquestrador/Doc/DEV_PORTATIL.md) | Paths portáteis / troubleshooting |
| [Doc/Passo a passo…](DFEND/CT_e%203.0/0-Orquestrador/Doc/Passo%20a%20passo%20execução%20Orquestrador.md) | Runbook |
| [Frontend/README.md](DFEND/CT_e%203.0/0-Orquestrador/Frontend/README.md) | Nx `:4220` |

### CT_e 2.0 — Cadeia

| Módulo | README | Doc técnica |
|--------|--------|-------------|
| [CT_e 2.0](DFEND/CT_e%202.0/README.md) | Visão da cadeia + compose | — |
| [0-Orquestrador](DFEND/CT_e%202.0/0-Orquestrador/README.md) | Dashboard multi-front | [Doc/](DFEND/CT_e%202.0/0-Orquestrador/Doc/) |
| [1-Receptor](DFEND/CT_e%202.0/1-Receptor/README.md) | Monitor Receptor | [Doc/](DFEND/CT_e%202.0/1-Receptor/Doc/) |
| [2-Arquivador](DFEND/CT_e%202.0/2-Arquivador/README.md) | Monitor Arquivador | [Doc/](DFEND/CT_e%202.0/2-Arquivador/Doc/) |
| [3-Sintetizador](DFEND/CT_e%202.0/3-Sintetizador/README.md) | Monitor Sintetizador | [Doc/](DFEND/CT_e%202.0/3-Sintetizador/Doc/) |
| [4-Analisador](DFEND/CT_e%202.0/4-Analisador/README.md) | Monitor Analisador | [Doc/](DFEND/CT_e%202.0/4-Analisador/Doc/) |
| [5-Integrador](DFEND/CT_e%202.0/5-Integrador/README.md) | Monitor Integrador | [Doc/](DFEND/CT_e%202.0/5-Integrador/Doc/) (mais completa) |
| [6-Carga](DFEND/CT_e%202.0/6-Carga/README.md) | Monitor Carga (download por chave) | [Doc/](DFEND/CT_e%202.0/6-Carga/Doc/) |
| [7-Resgate](DFEND/CT_e%202.0/7-Resgate/README.md) | Enfileira chaves para a Carga | [docs/](DFEND/CT_e%202.0/7-Resgate/docs/) |
| [gateway](DFEND/CT_e%202.0/gateway/README.md) | nginx `:8080` | — |
| [Contrato v1.3](DFEND/CT_e%202.0/docs/CONTRATO_MICROSERVICO_MONITOR.md) | Contrato compartilhado | — |

## 5. Contrato de microserviço (resumo)

Obrigatório em todo monitor plugado no Orquestrador:

| Método | Path | Auth |
|--------|------|------|
| GET | `/health` · `/health/live` | pública |
| GET | `/health/ready` | pública |
| GET | `/api/monitor/info` | API key |
| GET | `/api/monitor/service/status` | API key |
| GET | `/api/monitor/snapshot` | API key |
| GET | `/api/monitor/logs` | API key |
| GET | `/api/monitor/tables/{key}` | API key |
| POST | `/api/monitor/service/start` · `/stop` | API key |
| Hub | `/hubs/monitor` | conforme app |

Estados: `Disabled` · `Offline` · `Starting` · `Running` · `Stopping` · `Stopped` · `Failed` · `Unknown`.

Detalhes: [CONTRATO_MICROSERVICO_MONITOR.md](DFEND/CT_e%202.0/docs/CONTRATO_MICROSERVICO_MONITOR.md).

## 6. Como subir

### CT_e 3.0 (recomendado)

1. Abrir `DFEND\CT_e 3.0\0-Orquestrador`
2. `LIMPAR-E-BUILDAR.cmd` → `ABRIR-SOLUTION.cmd`
3. F5 na API; front: `cd Frontend` → `npm.cmd install` → `npm.cmd start` → `http://localhost:4220`

### CT_e 2.0 (cadeia Docker)

```powershell
cd "DFEND\CT_e 2.0"
$env:CTE_INTERNAL_API_KEY = "dev-cte-chain-key"
docker compose -f docker-compose.chain.yml up --build
```

Gateway: `http://localhost:8080` · Orquestrador front: `:4220`.

### Monitor isolado (ex.: Receptor)

Ver README do módulo: API F5 + `npm.cmd start` no Frontend + **Ligar** pela UI (DevHost + `Executar=1`).

## 7. Regras de ouro

1. **Não alterar** pastas `dfend-cte-*-windowsservices/**` (serviço fiscal original).
2. Controle via **DevHost** + flag SQL `Executar` (e afins).
3. Paths **relativos** — sem `LocalDev:RepoRoot` absoluto de outra máquina.
4. Não versionar `bin` / `obj` / `_artifacts` / connection strings reais.
5. Commits em **português** (ver `.cursor/rules/commits-em-portugues.mdc`).

## 8. Auditoria de documentação (06/08/2026)

### Estava bem documentado

- CT_e 3.0 Orquestrador (`README` + `Doc/` completa)
- Integrador 2.0 (`Doc/` rica: contrato, regras, runbooks)
- Resgate 2.0 (`docs/` + homologação)
- READMEs de Receptor, Arquivador, Sintetizador, Carga (operacionais)

### Estava parcial / quebrado

- READMEs de Receptor/Arquivador/Sintetizador/Carga/Orquestrador 2.0 apontavam para `Doc/` **inexistente**
- Root README só cobria CT_e 3.0 one-click
- Sem README da cadeia CT_e 2.0 nem do gateway
- Analisador sem README na raiz do módulo
- `Analisador.Api/README.md` estava com texto copiado do Sintetizador

### Corrigido nesta rodada

- Índice `DOCUMENTACAO-SISTEMA.md` + README raiz ampliado
- `CT_e 2.0/README.md`, `gateway/README.md`, `4-Analisador/README.md` + `Doc/`
- Pastas `Doc/` recriadas onde faltavam (Orquestrador 2.0, Receptor, Arquivador, Sintetizador, Carga)
- Contrato compartilhado em `CT_e 2.0/docs/`
- Correção do README do `Analisador.Api`
- Porta real da **Carga API = 5080** (não 5060) alinhada em docs + `Carga.Api/README`
- Link do ONBOARDING 3.0 apontando para o contrato compartilhado (não mais Doc Receptor ausente)

### Atualização 07/08/2026

- Modo **Apresentação** documentado (tour guiado + simulação) em `Documentacao_Orquestrador_CTe.md` §8.5, `Frontend/README.md` e `0-Orquestrador/README.md`
- QA: spotlight fixed, `panelPlacement`, Reiniciar, nomenclatura **Amarelo** (âmbar `#fbbf24`) para fila / NA FILA
