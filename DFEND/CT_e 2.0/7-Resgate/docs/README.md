# Docs — Resgate CT-e

Documentação operacional do pacote `7-Resgate`.

| Doc | Uso |
|-----|-----|
| [../README.md](../README.md) | Princípio, endpoints, run local, relação com Carga |
| [ROTEIRO-HOMOLOGACAO-RESGATE.md](ROTEIRO-HOMOLOGACAO-RESGATE.md) | Checklist homologação V1–V6 |
| [DECISOES-E-HIPOTESES.md](DECISOES-E-HIPOTESES.md) | Decisões técnicas e hipóteses |
| [DOCUMENTACAO-SISTEMA.md](../../../../DOCUMENTACAO-SISTEMA.md) | Índice do monorepo |

## Princípio (lembrete)

O Resgate **não baixa** CT-e. Só informa chaves; a **Carga** (`ProcessarDownload`) executa o download.

- API: `http://localhost:5070`
- UI: `http://localhost:4220/resgate` (Orquestrador)
- Login DEV: `dev` / `dev`
