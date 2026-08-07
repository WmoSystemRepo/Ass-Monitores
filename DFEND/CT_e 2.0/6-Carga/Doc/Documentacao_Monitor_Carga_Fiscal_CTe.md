# Documentação Técnica — Monitor Carga Fiscal CT-e

> **Sistema monitorado:** DFEND_CTe_Carga  
> **Status:** Implementado  
> **Atualizado:** 06/08/2026

## 1. Resumo

A **Carga** faz **download pontual por chave** (não fluxo contínuo) via WS `cteConsultaDFe`. CodServico **99**.

O **Resgate** apenas informa chaves; a Carga executa o download e devolve o documento ao fluxo normal.

| Aspecto | Valor |
|---------|--------|
| CodServico | **99** |
| Front | `:4260` |
| BFF | `:5080` / Swagger `:7166` |
| Host POC | `tools/Carga.DevHost` |
| serviceId | `dfend-cte-monitor-carga` |
| Accent | `#0f766e` (teal) |
| SQL | recepção + sintético |

## 2. Relação com Resgate

Ver [../../7-Resgate/README.md](../../7-Resgate/README.md):

1. Operador envia chaves no Resgate  
2. Temp + fila (`fila_alvo_cte_integrador` / desenho atual)  
3. Carga `ProcessarDownload` consome e persiste  

**Enfileirado ≠ resgatado** — acompanhar fila no Monitor Carga.

## 3. Docs

- [CONTRATO_MICROSERVICO_MONITOR.md](CONTRATO_MICROSERVICO_MONITOR.md)
- [Passo a passo execução do sistema Monitor Carga.md](Passo%20a%20passo%20execução%20do%20sistema%20Monitor%20Carga.md)
- [../README.md](../README.md)

## 4. Regras

Não alterar `dfend-cte-carga-windowsservices/**`. Flags típicas: `Executar=1`, `ExecutarAuto=1`.
