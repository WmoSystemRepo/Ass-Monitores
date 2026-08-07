# Onboarding — microserviço na cadeia CT-e (2.0)

Documento canônico (checklist, registry schema 1.0, timeouts):

**[CT_e 3.0/0-Orquestrador/Doc/ONBOARDING_MICROSERVICO.md](../../../CT_e%203.0/0-Orquestrador/Doc/ONBOARDING_MICROSERVICO.md)**

Contrato HTTP: [../../docs/CONTRATO_MICROSERVICO_MONITOR.md](../../docs/CONTRATO_MICROSERVICO_MONITOR.md).

## Resumo rápido

1. Implementar contrato v1.3 no Monitor.Api  
2. `Dockerfile` + `Dockerfile.front`  
3. Entrada em `docker-compose.chain.yml`  
4. Entrada em `Orchestrator:Systems` (`Id`, `Order`, `DependsOn`, URLs, `Enabled`)  
5. Mesma `InternalApiKey` Orquestrador ↔ monitor  
6. Smoke: `GET /api/chain/health` + Ligar inclui o estágio  

**Não** habilitar estágio novo com `Enabled: true` sem o checklist do doc canônico.
