# Passo a passo — execução do Windows Service Integrador CT-e

Guia operacional do **DFEND_CTe_Integrador** (sem Monitor/BFF neste pacote).  
Alinhado a `Doc/DOCUMENTACAO_TECNICA_DFEND_CTe_Integrador2.0.md` e `Doc/Analise_Regras_Negocio_DFEND_CTe_Integrador.md`.

| Item | Valor |
|------|--------|
| Pacote | `5-Integrador/` |
| Projeto | `dfend-cte-integrador-windowsservices` |
| CodServico | **7** |
| Papel | Integrações externas (Netezza / DocVinculado / FICS) |
| Monitor | **Não existe** neste pacote |

```text
5-Integrador/
├── dfend-cte-integrador-windowsservices/
└── Doc/
```

---

## 1. Pré-requisitos

| Software | Uso |
|----------|-----|
| Visual Studio 2022 | `DFEND_CTe_Integrador.sln` |
| SQL Server + SSMS | Sintético, Analítico, Histórico, Staging |
| .NET Framework 4.7 | Runtime |

---

## 2. Conferir SQL (CodServico = 7)

No banco **sintético** (configs do serviço 7):

```sql
-- Ajustar nome da tabela conforme BdCTeSintetico.ObterConfiguracao
SELECT des_configuracao, nom_configuracao, sts_ativo
FROM cte.configuracao_sintetico_conhecimento_transporte_eletronico WITH (READPAST)
WHERE cod_servico_sintetico_conhecimento_transporte_eletronico = 7
  AND sts_ativo = 1
ORDER BY des_configuracao;
```

**Flags críticas:** `Executar`, `IntegrarNetezza`, `IntegrarDocVinculado`, `IntegrarFICS`, `ReEnviarFila`, `QtdeMaxFila`, `Intervalo`, `Threads`.

**Fila:** `fila_alvo_cte_integrador` + temp `cte.tmp_integracao_conhecimento_transporte_eletronico`.

---

## 3. Configuração (App.config)

| Chave | Uso |
|-------|-----|
| `CodServicoIntegrador` | **7** |
| `BDCTeSintetico` | Fila / temp / XML sintético |
| `BDCTeAnalitico` | Filas FICS / DocVinculado |
| `BDNFeDefinitivo` | Histórico (consulta cancelamento) |
| `BDStaging` | Staging Netezza (primeira/segunda) |

Ver `Doc/RESUMO_ALTERACOES_DESENVOLVIMENTO.md` (Windows Auth ainda não aplicado).

---

## 4. Executar em Debug

1. Abrir `DFEND_CTe_Integrador.sln`.
2. **F5** → `ServWindows.StartDebug`.
3. Com `Executar=1`, o serviço drena a fila e aplica destinos conforme flags.

**Matriz rápida:**

| Tipo | Netezza | DocVinculado | FICS |
|------|---------|--------------|------|
| Autorização CT-e (57) | se flag | se flag | — |
| Lote `retDistCTeSVD` | — | — | se flag |
| Evento / Inut / GTV | no-op | no-op | — |

---

## 5. Instalar como Windows Service (opcional)

```powershell
InstallUtil.exe "C:\...\DFEND_CTe_Integrador.exe"
```

Operação: Services.msc + flag SQL `Executar` (cod **7**).

---

## 6. Aceite rápido

- [ ] Compila e sobe em F5
- [ ] Flags de integração lidas do BD
- [ ] Autorização modelo 57 com flags on → staging / DocVinc conforme config
- [ ] Evento na fila não quebra o serviço (no-op)
- [ ] Erro grava `des_mensagem_erro` na temp (reenvio à SB comentado)

---

## 7. Documentos relacionados

| Doc | Conteúdo |
|-----|----------|
| `Analise_Regras_Negocio_DFEND_CTe_Integrador.md` | RN-001 … RN-046 |
| `DOCUMENTACAO_TECNICA_DFEND_CTe_Integrador.md` / `…2.0.md` | Técnica |
| `RESUMO_ALTERACOES_DESENVOLVIMENTO.md` | Status POC DEV |
