# Documentação técnica — Orquestrador CT-e (2.0)

Dashboard da cadeia: Receptor → Arquivador → Sintetizador → Analisador → Integrador → Carga.

## Canônico

Para contrato completo, ambientes, Ligar/Desligar, go-live e UX:  
**[CT_e 3.0 Doc/Documentacao_Orquestrador_CTe.md](../../../CT_e%203.0/0-Orquestrador/Doc/Documentacao_Orquestrador_CTe.md)**

## Esta linha (2.0)

| Peça | Porta |
|------|-------|
| Orquestrador.Api | **5000** / Swagger **7100** |
| Front `cte-orquestrador` | **4220** |
| Gateway (compose) | **8080** |

- Registry por `Id`, `BaseUrl`, `FrontendUrl`, `Order`, `DependsOn`, `Enabled`
- Header `X-Cte-Internal-Api-Key` (DEV: `dev-cte-chain-key`)
- Health: `/health` · `/health/ready` · `/api/chain/health`

### Ligar / Desligar

1. Garantir API+Front ready (DEV: spawn se `EnsureBeforeCascade`)
2. `service/start` + poll até Running (ordem `Order` / `DependsOn`)
3. Desligar: ordem inversa

Clicar no estágio no dashboard **abre `FrontendUrl` em nova aba** (não sobe API/front).

### Ambientes

| Ambiente | Config | Key |
|----------|--------|-----|
| Development | `appsettings.Development.json` | `dev-cte-chain-key` |
| Homologacao / Production | env + appsettings | secret |

Ver também: [ONBOARDING_MICROSERVICO.md](ONBOARDING_MICROSERVICO.md) · [../README.md](../README.md).
