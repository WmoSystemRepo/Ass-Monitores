# Contrato de microserviço — Monitor CT-e (Integrador)

Versão do contrato: **1.3**  
Domínio deste pacote: `integrador`  
Consumidor: **Orquestrador CT-e** (`0-Orquestrador`)

> **Status:** contrato **implementado** em `5-Integrador/` (`Frontend` / `Integrador.Api` / `tools/Integrador.DevHost`).

Guia: `0-Orquestrador/Doc/ONBOARDING_MICROSERVICO.md`.

## Identidade

| Campo | Valor |
|-------|--------|
| `serviceId` | `dfend-cte-monitor-integrador` |
| `domain` | `integrador` |
| `monitoredService` | `DFEND_CTe_Integrador` |
| `apiVersion` | `1.0` |
| `CodServico` | `7` |

## Endpoints obrigatórios

Mesmo conjunto v1.3: `info`, `health`/`live`/`ready`, `service/status`, `snapshot`, `logs`, `tables/{key}`, `service/start|stop`, Hub `/hubs/monitor`.  
Aliases: `/api/v1/monitor/*`.  
Auth: `X-Cte-Internal-Api-Key`.

## Estados oficiais

`Disabled` · `Offline` · `Starting` · `Running` · `Stopping` · `Stopped` · `Failed` · `Unknown`

## Portas locais

| Serviço | API | UI | Swagger |
|---------|-----|-----|---------|
| Integrador | **5050** | **4250** | **7156** |

## Flags de telemetria no snapshot

`Executar` · `IntegrarNetezza` · `IntegrarDocVinculado` · `IntegrarFICS` · `ReEnviarFila` · `QtdeMaxFila`

## Tabelas vigiladas

| Key | Objeto |
|-----|--------|
| `servico` | Serviço cod **7** |
| `configuracao` | Flags de integração |
| `temporaria` | `cte.tmp_integracao_conhecimento_transporte_eletronico` |
| `fila_integrador` / `fila` | `fila_alvo_cte_integrador` |
| `staging` | Semáforo / temps Netezza |
| `fila_fics` / `fila_doc_vinculado` | Filas outbound analítico |
| `log` | Logs sintéticos do serviço |
