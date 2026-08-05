# Passo a passo — execução do sistema Monitor Integrador

Guia **DEV** alinhado a `Documentacao_Monitor_Integrador_Fiscal_CTe.md` e contrato **v1.3**.

| Item | Valor |
|------|--------|
| Pacote | `5-Integrador/` |
| CodServico | **7** |
| API / UI / Swagger | `:5050` / `:4250` / `:7156` |
| Status | Monitor **implementado** |

```text
5-Integrador/
├── dfend-cte-integrador-windowsservices/
├── Frontend/
├── Integrador.Api/
├── tools/Integrador.DevHost/
└── Doc/
```

---

## 0. Pronto vs falta

| Item | Status |
|------|--------|
| Doc técnica + análise de regras | ✅ |
| Contrato + Doc Monitor | ✅ |
| RESUMO DEV (Windows Auth) | ✅ aplicado |
| BFF / Front / DevHost | ✅ |

---

## 1. Subir o Monitor (DEV)

1. SQL: `Executar` + flags `IntegrarNetezza` / `IntegrarDocVinculado` / `IntegrarFICS` (cod **7**).  
2. API: `dotnet run --project Integrador.Api/src/Monitor.Api` · Swagger `:7156` · `/api/monitor/info` → `domain: "integrador"`.  
3. Front: em `Frontend/` → `npm install` · `npm start` → `:4250`.  
4. Ligar Integrador CT-e no painel → sobe `Integrador.DevHost` + `Executar=1`.  
5. Snapshot deve expor flags e contagem da fila.

## 2. Aceite rápido

- [ ] info/ready OK  
- [ ] Front `:4250`  
- [ ] Ligar altera `Executar` (cod 7)  
- [ ] Cards Netezza / DocVinc / FICS visíveis  

## 3. Windows Service sem Monitor

Use `Passo a passo execução do Windows Service Integrador.md`.
