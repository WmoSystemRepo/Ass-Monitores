# Documentação Técnica — Monitor Receptor Fiscal CT-e

> **Sistema monitorado:** DFEND_CTe_Receptor  
> **Status:** Implementado  
> **Atualizado:** 06/08/2026

## 1. Resumo

O **Monitor Receptor** observa o Windows Service que recebe distribuição SEFAZ (NSU), grava temporária/broker e alimenta o Arquivador.

| Aspecto | Valor |
|---------|--------|
| CodServico | **2** |
| Front | `:4200` |
| BFF | `:5010` / Swagger `:7116` |
| Host POC | `tools/Receptor.DevHost` |
| serviceId | `dfend-cte-monitor-receptor` |
| domain | `receptor` |
| SQL DEV típico | `bd_cte_recepcao` |
| Paleta | cyan |

## 2. Objetivo

1. Ligado + `Executar=1`.
2. Anatomia UI: SEFAZ → tmp → Broker → Arquivador.
3. Threads T1–T5, histórico, tabelas (5 keys), config somente leitura.
4. Ligar/Desligar sem InstallUtil.

## 3. Estrutura

```text
1-Receptor/
├── dfend-cte-receptor-windowsservices/
├── Frontend/
├── Receptor.Api/
├── tools/Receptor.DevHost/
└── Doc/
```

## 4. Contrato e runbook

- [CONTRATO_MICROSERVICO_MONITOR.md](CONTRATO_MICROSERVICO_MONITOR.md) (v1.3)
- [Passo a passo execução do sistema Monitor Receptor.md](Passo%20a%20passo%20execução%20do%20sistema%20Monitor%20Receptor.md)
- README do pacote: [../README.md](../README.md)
- BFF: [../Receptor.Api/README.md](../Receptor.Api/README.md)

## 5. Regras

- Não alterar o Windows Service original.
- Escrita SQL de controle: `UPDATE Executar`.
- Snapshot ~1 s; logs via SignalR.
- Sem mock no caminho feliz DEV.
