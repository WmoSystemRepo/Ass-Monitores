# CT_e 2.0 — Cadeia multi-app DFEND

Linha **desacoplada**: cada estágio é um pacote portátil (Windows Service + DevHost + BFF + Front Nx).

Para o dia a dia unificado, preferir **[CT_e 3.0/0-Orquestrador](../CT_e%203.0/0-Orquestrador/README.md)**.  
Índice do monorepo: [DOCUMENTACAO-SISTEMA.md](../../DOCUMENTACAO-SISTEMA.md).

## Estrutura

```text
CT_e 2.0/
├── 0-Orquestrador/     # Dashboard / cascata Ligar-Desligar :4220 / :5000
├── 1-Receptor/         # SEFAZ → recepção (CodServico 2) :4200 / :5010
├── 2-Arquivador/       # Fila → destinos (CodServico 3) :4210 / :5020
├── 3-Sintetizador/     # Sintético (CodServico 8) :4230 / :5030
├── 4-Analisador/       # Análise (CodServico 6) :4240 / :5040
├── 5-Integrador/       # Netezza / DocVinc / FICS (CodServico 7) :4250 / :5050
├── 6-Carga/            # Download pontual por chave (CodServico 99) :4260 / :5080
├── 7-Resgate/          # Enfileira chaves → Carga :5070
├── gateway/            # nginx entrada única :8080
├── docker-compose.chain.yml
├── docs/CONTRATO_MICROSERVICO_MONITOR.md
└── Logs/               # logs locais (não versionar segredos)
```

## Fluxo de negócio

```text
SEFAZ
  → Receptor (distribuição / NSU)
  → Arquivador (roteia para filas)
  → Sintetizador (persistência sintética)
  → Analisador (regras / fila analisador)
  → Integrador (destinos analíticos)
  → Carga (consulta DFe por chave — sob demanda)

Resgate: operador informa chaves → temp + fila → Carga faz o download.
```

## Docker (cadeia completa)

A partir desta pasta:

```powershell
$env:CTE_INTERNAL_API_KEY = "dev-cte-chain-key"
# Opcional: connection strings SQL via env MONITOR_*_CONNECTION_STRING
docker compose -f docker-compose.chain.yml up --build
```

- Gateway: `http://localhost:8080`
- Orquestrador UI: `http://localhost:4220`
- Workers fiscais continuam no **host Windows** (`PreferLocalProcess=false` nos containers)

## Desenvolvimento isolado

1. Abrir o `Monitor.sln` do estágio (ou Orquestrador.sln)
2. F5 na API (perfil `http` ou `https`)
3. `cd Frontend` → `npm.cmd install` → `npm.cmd start`
4. Na UI: **Ligar** (sobe DevHost + `Executar=1`)

API key DEV: `dev-cte-chain-key`.

## Contrato

Todos os monitores implementam o contrato **v1.3**:  
[docs/CONTRATO_MICROSERVICO_MONITOR.md](docs/CONTRATO_MICROSERVICO_MONITOR.md)

Onboarding no Orquestrador: ver Doc do CT_e 3.0  
[ONBOARDING_MICROSERVICO.md](../CT_e%203.0/0-Orquestrador/Doc/ONBOARDING_MICROSERVICO.md).

## Regras

- Não alterar `dfend-cte-*-windowsservices/**`
- Escrita SQL de controle só via Monitor (`Executar`, etc.)
- Dados reais em DEV — sem mock no caminho feliz
