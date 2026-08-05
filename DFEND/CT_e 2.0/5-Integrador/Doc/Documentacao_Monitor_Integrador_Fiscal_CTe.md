# Documentação Técnica — Monitor Integrador Fiscal CT-e

> **Sistema monitorado:** DFEND_CTe_Integrador (integrações externas · SEFAZ-BA)  
> **Status:** **Implementado** (Frontend / Integrador.Api / DevHost)  
> **Gerado em:** 25/07/2026  

> **Fontes:** `Analise_Regras_Negocio_…`, `DOCUMENTACAO_TECNICA_…2.0`, `RESUMO_ALTERACOES_…`, `CONTRATO_MICROSERVICO_MONITOR.md`, blueprint Receptor.

---

## 1. Resumo executivo

O **Monitor Integrador** observará o Windows Service que consome `fila_alvo_cte_integrador` e dispara destinos:

| Destino | Condição |
|---------|----------|
| Netezza (staging dual) | `IntegrarNetezza=1` |
| DocVinculado | `IntegrarDocVinculado=1` **e** modelo **57** |
| FICS | `IntegrarFICS=1` **e** esquema `retDistCTeSVD` |

Evento / inutilização / GTV = **no-op** no fluxo atual.

| Aspecto | Valor |
|---------|--------|
| CodServico | **7** |
| Front (alvo) | `:4250` |
| BFF (alvo) | `:5050` / Swagger `:7156` |
| Host POC (alvo) | `tools/Integrador.DevHost` |
| `serviceId` | `dfend-cte-monitor-integrador` |
| Status | Implementado |

---

## 2. Objetivo do Monitor

1. Ligado + executando (`Executar=1`).  
2. Etapa **AGORA**: Fila → Temp → Autorização → Destinos (Netezza / DocVinc / FICS).  
3. Visibilidade das flags de integração e quantidade na fila.  
4. Saúde de temps, staging e filas outbound.  
5. Ligar/Desligar sem InstallUtil.

### Fluxo UI (5 etapas — alvo)

1. Fila Integrador  
2. Obter lote/chave  
3. Classificar schema  
4. Integrar destinos  
5. Limpar temp  

---

## 3. Decisões

| Decisão | Valor |
|---------|--------|
| Mock | Não |
| Contrato | v1.3 |
| Accent sugerido | laranja / coral |
| Blueprint | Receptor + flags do Integrador |

---

## 4. Estrutura alvo

```text
5-Integrador/
├── dfend-cte-integrador-windowsservices/
├── Frontend/
├── Integrador.Api/
├── tools/Integrador.DevHost/
└── Doc/
```

---

## 5. Referências

| Doc | Uso |
|-----|-----|
| `Passo a passo execução do sistema Monitor Integrador.md` | Subir Monitor (futuro) |
| `Passo a passo execução do Windows Service Integrador.md` | Debug WS **hoje** |
| `CONTRATO_MICROSERVICO_MONITOR.md` | Portas / identidade |
