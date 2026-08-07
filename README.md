# Ass-Monitores

Monorepo **DFEND** com as linhas **CT_e 2.0** (cadeia multi-app) e **CT_e 3.0** (Orquestrador unificado).

> Documentação completa do sistema: **[DOCUMENTACAO-SISTEMA.md](DOCUMENTACAO-SISTEMA.md)**  
> (arquitetura, portas, contrato v1.3, índice por módulo, auditoria).

## CT_e 3.0 — Orquestrador (uso diário)

Pasta: `DFEND\CT_e 3.0\0-Orquestrador`

1. Duplo clique em **`LIMPAR-E-BUILDAR.cmd`**
2. Duplo clique em **`ABRIR-SOLUTION.cmd`**

Se não achar a pasta: **`PROCURAR-E-CONSERTAR.cmd`** (nesta raiz ou no Desktop).

Docs:

- [0-Orquestrador/COMO-USAR.txt](DFEND/CT_e%203.0/0-Orquestrador/COMO-USAR.txt)
- [0-Orquestrador/README.md](DFEND/CT_e%203.0/0-Orquestrador/README.md)
- [Doc/Documentacao_Orquestrador_CTe.md](DFEND/CT_e%203.0/0-Orquestrador/Doc/Documentacao_Orquestrador_CTe.md) (§8.5 modo Apresentação)
- [Doc/DEV_PORTATIL.md](DFEND/CT_e%203.0/0-Orquestrador/Doc/DEV_PORTATIL.md)
- [Frontend/README.md](DFEND/CT_e%203.0/0-Orquestrador/Frontend/README.md)

**Não** use clone com `Ass-Monitores\Ass-Monitores` nem projeto só na Lixeira.

## CT_e 2.0 — Cadeia desacoplada

Pasta: `DFEND\CT_e 2.0\`

- Visão geral: [CT_e 2.0/README.md](DFEND/CT_e%202.0/README.md)
- Compose: `docker-compose.chain.yml` (APIs + Fronts + gateway `:8080`)
- Estágios: `0-Orquestrador` … `7-Resgate` + `gateway`

## Cadeia fiscal (visão rápida)

```text
Receptor → Arquivador → Sintetizador → Analisador → Integrador → Carga
     └────────────── Orquestrador (Ligar / Desligar / dashboard) ──────────────┘
                                                              Resgate → Carga
```

| Porta | Serviço |
|-------|---------|
| 4220 / 5000 | Orquestrador UI / API |
| 4200–4260 | Fronts dos monitores |
| 5010–5050 · 5080 | APIs dos monitores (Carga = **5080**) |
| 5070 | Resgate API |
| 8080 | Gateway Docker |

## Pré-requisitos

- .NET 8 SDK + Visual Studio 2022
- Node.js LTS (20/22/24) — no PowerShell use `npm.cmd`
- SQL Server DEV alcançável (recepção / sintético conforme o estágio)
- Docker (opcional, para a cadeia completa)
