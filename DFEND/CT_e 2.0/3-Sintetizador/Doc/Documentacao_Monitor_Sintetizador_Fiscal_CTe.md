# Documentação Técnica — Monitor Sintetizador Fiscal CT-e

> **Sistema monitorado:** DFEND_CTe_Sintetizador  
> **Status:** Implementado  
> **Atualizado:** 06/08/2026

## 1. Resumo

Observa o serviço que materializa o **sintético** CT-e (CodServico **8**).

| Aspecto | Valor |
|---------|--------|
| CodServico | **8** |
| Front | `:4230` |
| BFF | `:5030` / Swagger `:7136` |
| Host POC | `tools/Sintetizador.DevHost` |
| serviceId | `dfend-cte-monitor-sintetizador` |
| Accent | `#7c3aed` |
| SQL DEV típico | `bd_cte_sintetico` (`DDFESIN\BDD_DFE_SINTETIC`) |

## 2. Docs

- [CONTRATO_MICROSERVICO_MONITOR.md](CONTRATO_MICROSERVICO_MONITOR.md)
- [Passo a passo execução do sistema Monitor Sintetizador.md](Passo%20a%20passo%20execução%20do%20sistema%20Monitor%20Sintetizador.md)
- [../README.md](../README.md)

## 3. Regras

Windows Service congelado pós-RESUMO. Controle via DevHost + `Executar`.
