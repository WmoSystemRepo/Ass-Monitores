# Documentação — Orquestrador CT-e (linha 2.0)

> **Nota (06/08/2026):** a documentação **canônica e mais completa** está em  
> **[CT_e 3.0/0-Orquestrador/Doc](../../../CT_e%203.0/0-Orquestrador/Doc/)**.  
> Esta pasta cobre a linha **2.0** (dashboard que abre fronts por `FrontendUrl` em abas).

## Diferença 2.0 × 3.0

| Tema | CT_e 2.0 | CT_e 3.0 |
|------|----------|----------|
| Fronts dos estágios | Apps Nx separados (`:4200`…`:4260`) | Monitores **in-app** em `/monitores/{id}` no `:4220` |
| Engines DevHost | Em cada pasta `1-Receptor`…`6-Carga` | Consolidados em `engines/` |
| Resgate | Pacote `7-Resgate` | `libs/resgate` + UI `/resgate` |
| Docs one-click | Limitado | `LIMPAR-E-BUILDAR.cmd` / `DEV_PORTATIL.md` |

## Conteúdo local

- [ONBOARDING_MICROSERVICO.md](ONBOARDING_MICROSERVICO.md) — espelho apontando ao 3.0
- [Documentacao_Orquestrador_CTe.md](Documentacao_Orquestrador_CTe.md) — resumo + links
- [Passo a passo execução Orquestrador.md](Passo%20a%20passo%20execução%20Orquestrador.md)

README do pacote: [../README.md](../README.md).  
Contrato compartilhado: [../../docs/CONTRATO_MICROSERVICO_MONITOR.md](../../docs/CONTRATO_MICROSERVICO_MONITOR.md).
