# Documentação Técnica — Monitor Analisador Fiscal CT-e

> **Sistema monitorado:** DFEND_CTe_Analisador  
> **Status:** Implementado (Frontend / Analisador.Api / DevHost)  
> **Atualizado:** 06/08/2026

## 1. Resumo

O **Monitor Analisador** observa o Windows Service que consome a fila do analisador (`fila_alvo_cte_analisador` / NSU), processa a temporária e avança o fluxo rumo ao Integrador.

| Aspecto | Valor |
|---------|--------|
| CodServico | **6** |
| Front | `:4240` |
| BFF | `:5040` / Swagger `:7146` |
| Host POC | `tools/Analisador.DevHost` |
| serviceId | `dfend-cte-monitor-analisador` |
| domain | `analisador` |

## 2. Objetivo do Monitor

1. Ligado + executando (`Executar=1`, cod **6**).
2. Etapa **AGORA**: Fila → retirada NSU → temporária → continuidade.
3. Threads / histórico / tabelas / config (somente leitura das configs de origem).
4. Ligar/Desligar sem InstallUtil (DevHost).

## 3. Estrutura

```text
4-Analisador/
├── dfend-cte-analisador-windowsservices/
├── Frontend/                 # Nx cte-analisador
├── Analisador.Api/
├── tools/Analisador.DevHost/
└── Doc/
```

## 4. Contrato

Ver [CONTRATO_MICROSERVICO_MONITOR.md](CONTRATO_MICROSERVICO_MONITOR.md) (v1.3).

Auth: `X-Cte-Internal-Api-Key`. Hub: `/hubs/monitor`.

## 5. Como rodar

Ver [Passo a passo execução do sistema Monitor Analisador.md](Passo%20a%20passo%20execução%20do%20sistema%20Monitor%20Analisador.md).

## 6. Regras

- Não alterar o Windows Service original.
- Controle via DevHost + `UPDATE Executar`.
- Sem mock no caminho feliz DEV.
