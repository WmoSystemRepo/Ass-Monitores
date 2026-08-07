# DFEND CT-e — Pacote Monitor Analisador

Pacote portátil do **Monitor em tempo real** do Windows Service `DFEND_CTe_Analisador` (CodServico **6**).

## Estrutura

```text
4-Analisador/
  dfend-cte-analisador-windowsservices/  # WS original (INTATO)
  Frontend/                              # Nx cte-analisador :4240
  Analisador.Api/                        # BFF Clean :5040 / Swagger :7146
  tools/Analisador.DevHost/              # Host POC → StartDebug
  Doc/                                   # Documentação técnica, contrato, runbook
  Dockerfile · Dockerfile.front          # API / Front como microserviço
```

## Documentação

- **Documento mestre:** [Doc/Documentacao_Monitor_Analisador_Fiscal_CTe.md](Doc/Documentacao_Monitor_Analisador_Fiscal_CTe.md)
- **Runbook:** [Doc/Passo a passo execução do sistema Monitor Analisador.md](Doc/Passo%20a%20passo%20execução%20do%20sistema%20Monitor%20Analisador.md)
- **Contrato:** [Doc/CONTRATO_MICROSERVICO_MONITOR.md](Doc/CONTRATO_MICROSERVICO_MONITOR.md) (**v1.3**)

## Como rodar (DEV)

1. Compilar Host POC: `tools\Analisador.DevHost` (Debug)
2. API: abrir `Analisador.Api/Monitor.sln` → perfil **http** (`:5040`) ou **https** (Swagger `:7146`)
3. Front:

```powershell
cd Frontend
npm.cmd install
npm.cmd start
# http://localhost:4240
```

4. Na UI: **Ligar Analisador CT-e** → DevHost + `Executar=1`

API key DEV: `Monitor:InternalApiKey` = `dev-cte-chain-key` (alinhar com Orquestrador).

## Identidade

| Campo | Valor |
|-------|--------|
| serviceId | `dfend-cte-monitor-analisador` |
| domain | `analisador` |
| CodServico | **6** |
| API / UI / Swagger | **5040** / **4240** / **7146** |
| Accent sugerido | índigo / azul |

## Microserviço

- `GET /api/monitor/info` · `/health` · `/health/live` · `/health/ready`
- `/api/monitor/*` com `X-Cte-Internal-Api-Key`
- Hub `/hubs/monitor`
- Orquestrador: `0-Orquestrador` / CT_e 3.0 Doc

## Menu UI

Monitor · Threads · Histórico · Tabelas · Configurações  
(rota auxiliar: `/mais-informacoes`)

## Regra-mãe

Não alterar `dfend-cte-analisador-windowsservices/**`. Controle via DevHost + flag SQL `Executar` (cod **6**).
