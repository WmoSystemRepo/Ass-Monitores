# Decisões e hipóteses — Resgate CT-e

Documento operacional em `7-Resgate/docs`.  
Para incorporar ao SDD (`.cursor/SDD/...`), usar o comando **`documenta`** / **`aprovado`**.

## Princípio confirmado

Resgate **não** implementa download — só informa chaves ao serviço responsável (Carga / `ProcessarDownload`, decisão técnica).

## Confirmado (reunião / cliente)

- Entrada somente por chave de acesso CT-e  
- NSU fora do escopo  
- Somente CT-e  
- Objetivo: baixar de novo e ficar **disponível para continuidade do processamento normal**

## Decisões técnicas (não literais na reunião)

- Reuso de `ProcessarDownload`  
- Reuso de `fila_alvo_cte_integrador` + temp  
- Implementação atual da Carga baseada em `des_esquema` = chave (análise de código — **não** contrato oficial da reunião)  
- Sem novas tabelas de lote  
- API `:5070`, UI `/resgate`, endpoints de fila/status  

## Status das hipóteses

| Item | Status |
|------|--------|
| Entrada somente por chave | Confirmado |
| NSU fora do escopo | Confirmado |
| Somente CT-e | Confirmado |
| Reuso do ProcessarDownload | Decisão técnica |
| Reuso da fila existente | Decisão técnica |
| Continuidade do fluxo normal | Validar em V2 |
| Reentrada via Receptor | Em validação (hipótese V2) |
| Duplicidade | Em validação |
| Critérios finais de aceite | Em validação |

## Aderência

**10/10 condicionada** à validação V1–V6 na homologação (ver `ROTEIRO-HOMOLOGACAO-RESGATE.md`).
