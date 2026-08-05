# Roteiro de homologação — Resgate CT-e (V1–V6)

Princípio: Resgate **só informa chaves**; Carga executa o download.  
Entrada: **somente chave** (nunca NSU). Escopo: **só CT-e**.

**Enfileirado ≠ resgatado.** Continuidade do fluxo normal: validar em **V2** (não tratar Receptor como fato).

## Pré-check operacional

- [ ] Carga ligada (`Executar=1`, `ExecutarAuto=1`, CodServico **99**)
- [ ] Resgate.Api `:5070` + Orquestrador `/resgate`
- [ ] Connection string `BDCTeSintetico`
- [ ] Certificado no host da Carga
- [ ] Preferir janela com atenção ao Integrador (fila compartilhada)

---

## V1 — ProcessarDownload ponta a ponta

| Confirmar | Evidência |
|-----------|-----------|
| Chave enfileirada chega à Carga | log Carga “chave retirada da fila” / temp |
| Sem dependências ocultas bloqueantes | sobe Carga + Resgate com config DEV |
| Sem efeito colateral óbvio | smoke Integrador/outros na mesma janela |

**Status:** a confirmar em homologação ao vivo (SQL/Carga no ambiente).

---

## V2 — Continuidade do fluxo normal

| Confirmar | Evidência |
|-----------|-----------|
| Onde o doc fica disponível | SELECT sintético pela chave |
| Critério de “continuidade” | acordo de negócio pós-teste |
| Hipótese Receptor | só se evidência na homologação |

**Achado preliminar (código — hipótese):** `ProcessarDownload` persiste no **sintético** e **não** enfileira no caminho contínuo do Receptor (`cteDistSVD`). Registrar go / no-go / ajuste de aceite após teste.

Referências: `6-Carga/.../SerCTeCarga.cs`, `1-Receptor/.../SerCTeReceptor.cs`.

---

## V3 — Fila compartilhada

| Confirmar | Evidência |
|-----------|-----------|
| Consumidores | Código: **Carga** + **Integrador** (`RetirarFilaIntegrador`) |
| Risco | disputa / atraso se ambos ativos |

**Achado preliminar:** 2 consumidores. Mitigação operacional: janela com Carga prioritária.

---

## V4 — Ciclo de persistência

1. Recebe chave (API 200)  
2. Consulta AN (log Carga / cStat)  
3. Persiste sintético (`documento_…_autorizacao` e/ou evento/inut)  
4. Temp removida no sucesso  
5. Disponível para continuidade (critério V2)

---

## V5 — Evidências de Resgate concluído

| Evidência | Como provar |
|-----------|-------------|
| Chave aceita | HTTP 200 + 44 dígitos |
| Processamento realizado | log Carga / temp consumida |
| Documento persistido | SELECT sintético |
| Continuidade do processamento normal | **definir após V2** |
| Sem impacto demais processos | smoke na janela |

---

## V6 — Duplicidade

| Cenário | Achado preliminar | Aceite |
|---------|-------------------|--------|
| Doc já existe | PK → log “já existente”, sem overwrite | Em validação (negócio) |
| Mesma chave reenviada | Nova temp+SEND; insert pode no-op | Em validação |

---

## Script rápido (operador)

1. Login `/resgate` → colar 1–2 chaves → **Processar**  
2. Confirmar aviso “enfileirado ≠ resgatado”  
3. **Ver fila Download** + **Atualizar status**  
4. Monitor Carga `:4260`  
5. SQL: temp / autorização pela chave  
6. Preencher resultado V2 e V6 neste roteiro  

Logs Resgate: `CT_e 2.0\Logs\resgate-*.log`
