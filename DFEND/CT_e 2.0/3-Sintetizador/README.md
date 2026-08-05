# Monitor Sintetizador CT-e

Pacote portátil em `3-Sintetizador/` para observar o Windows Service **DFEND_CTe_Sintetizador** em tempo real.

## Componentes

| Peça | Porta | Pasta |
|------|-------|-------|
| Front (Nx `cte-sintetizador`) | **4230** | `Frontend/` |
| BFF (`Sintetizador.Api`) | **5030** / Swagger **7136** | `Sintetizador.Api/` |
| Host POC | — | `tools/Sintetizador.DevHost/` |
| Windows Service (congelado pós-RESUMO) | — | `dfend-cte-sintetizador-windowsservices/` |

- `serviceId`: `dfend-cte-monitor-sintetizador`
- `CodServico`: **8**
- Accent: **`#7c3aed`**
- SQL DEV: `DDFESIN\BDD_DFE_SINTETIC` / `bd_cte_sintetico`

## Subir em DEV

1. BFF: abrir `Sintetizador.Api/Monitor.sln` → profile `http` (`:5030`)
2. Front: `cd Frontend` → `npm install` → `npm start` (`:4230`)
3. Ligar Sintetizador pela UI (sobe `Sintetizador.DevHost` + `Executar=1`)

Documentação: `Doc/Documentacao_Monitor_Sintetizador_Fiscal_CTe.md`  
RESUMO do original: `Doc/RESUMO_ALTERACOES_DESENVOLVIMENTO.md`
