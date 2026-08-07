# Documentação Técnica — Monitor Arquivador Fiscal CT-e

> **Sistema monitorado:** DFEND_CTe_Arquivador  
> **Status:** Implementado  
> **Atualizado:** 06/08/2026

## 1. Resumo

Consome fila de entrada e roteia para destinos (Sintetizador / Analisador / Integrador).

| Aspecto | Valor |
|---------|--------|
| CodServico | **3** |
| Front | `:4210` |
| BFF | `:5020` / Swagger `:7126` |
| Host POC | `tools/Arquivador.DevHost` |
| serviceId | `dfend-cte-monitor-arquivador` |
| tableHealth | **8** keys (entrada + filas destino) |
| Paleta | zinc + âmbar + teal |

## 2. Ciclo

Fila → Temp → Sintetizador → Analisador → Integrador (conforme regras do WS).

## 3. Docs

- [CONTRATO_MICROSERVICO_MONITOR.md](CONTRATO_MICROSERVICO_MONITOR.md)
- [Passo a passo execução do sistema Monitor Arquivador.md](Passo%20a%20passo%20execução%20do%20sistema%20Monitor%20Arquivador.md)
- [../README.md](../README.md)

## 4. Regras

Não alterar `dfend-cte-arquivador-windowsservices/**`. Controle via DevHost + `Executar`.
