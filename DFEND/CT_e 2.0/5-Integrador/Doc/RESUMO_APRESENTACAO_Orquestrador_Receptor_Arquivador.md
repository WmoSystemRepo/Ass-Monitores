# Resumo rápido — Orquestrador · Receptor · Arquivador

> Versão curta. Detalhamento completo de menus, cards e dados:  
> [`RESUMO_ALTERACOES_DESENVOLVIMENTO.md`](./RESUMO_ALTERACOES_DESENVOLVIMENTO.md)

---

## Pontos para focar

1. Papel de cada um (controle · entrada SEFAZ · fan-out)
2. Fluxo: SEFAZ → Receptor → fila → Arquivador → Sint/Anal/Int
3. Plano de controle (HTTP) vs plano de dados (Service Broker)
4. Menus e cards de cada Monitor
5. Cascata Ligar/Desligar + gate `Executar = 1`

---

## Em uma linha cada

| Sistema | Frase |
|---------|-------|
| **Orquestrador** | Painel: liga/desliga a cadeia e abre cada Monitor |
| **Receptor** | Busca CT-e na SEFAZ, grava temp, enfileira Arquivador |
| **Arquivador** | Consome fila, fan-out para Sint/Anal/Int, exclui temp |
