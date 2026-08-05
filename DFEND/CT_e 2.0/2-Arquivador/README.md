# DFEND CT-e — Pacote Monitor Arquivador

Pacote portátil do **Monitor em tempo real** do Windows Service `DFEND_CTe_Arquivador` (operação validada em DEV).

## Estrutura

```text
2-Arquivador/
  dfend-cte-arquivador-windowsservices/  # WS original (INTATO)
  Frontend/                              # Nx cte-arquivador :4210
  Arquivador.Api/                        # BFF Clean :5020 / Swagger :7126
  tools/Arquivador.DevHost/              # Host POC → StartDebug
  Doc/                                   # Documentação técnica, contrato, runbook
  Dockerfile · docker-compose.yml        # API como microserviço
```

## Documentação

- **Documento mestre:** [Doc/Documentacao_Monitor_Arquivador_Fiscal_CTe.md](Doc/Documentacao_Monitor_Arquivador_Fiscal_CTe.md)
- **Runbook:** [Doc/Passo a passo execução do sistema Monitor Arquivador.md](Doc/Passo%20a%20passo%20execução%20do%20sistema%20Monitor%20Arquivador.md)
- **Contrato:** [Doc/CONTRATO_MICROSERVICO_MONITOR.md](Doc/CONTRATO_MICROSERVICO_MONITOR.md) (**v1.3**)

## Como rodar

Ver o runbook acima.

## Microserviço

Contrato compartilhado **v1.3** (API key interna + multiambiente).

- `GET /api/monitor/info`
- `GET /health` · `/health/live` · `GET /health/ready` (dual: primary + sintético)
- `/api/monitor/*` protegido por `X-Cte-Internal-Api-Key` (`Monitor:InternalApiKey`)
- Headers `X-Monitor-Service` / `X-Monitor-Version`
- Orquestrador: `0-Orquestrador/Doc/Documentacao_Orquestrador_CTe.md`

## Diferenças vs Monitor Receptor

| Item | Arquivador |
|------|------------|
| Portas | 5020 / 7126 / 4210 |
| CodServico | **3** |
| Ciclo | Fila → Temp → Sintetizador → Analisador → Integrador |
| tableHealth | **8** keys (entrada + 3 filas destino) |
| Paleta | zinc + âmbar + teal |

## Menu UI

Monitor · Threads · Histórico · Tabelas · Configurações  
(rota auxiliar: `/mais-informacoes`)

## Regra-mãe

Não alterar `dfend-cte-arquivador-windowsservices/**`. Controle via DevHost + flag SQL `Executar`.
