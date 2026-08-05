# Monitor Carga CT-e

Pacote portátil em `6-Carga/` para observar o Windows Service **DFEND_CTe_Carga** em tempo real.

Negócio: **download pontual por chave** (não fluxo contínuo) via WS `cteConsultaDFe`.

## Componentes

| Peça | Porta | Pasta |
|------|-------|-------|
| Front (Nx `cte-carga`) | **4260** | `Frontend/` |
| BFF (`Carga.Api`) | **5060** / Swagger **7166** | `Carga.Api/` |
| Host POC | — | `tools/Carga.DevHost/` |
| Windows Service (congelado pós-RESUMO) | — | `dfend-cte-carga-windowsservices/` |

- `serviceId`: `dfend-cte-monitor-carga`
- `CodServico`: **99**
- Accent: **`#0f766e`** (teal)
- SQL DEV: recepção `DDFEREC\BDD_DFE_RECEPCAO` + sintético `DDFESIN\BDD_DFE_SINTETIC`

## Subir em DEV

1. BFF: abrir `Carga.Api/Monitor.sln` → profile `http` (`:5060`)
2. Front: `cd Frontend` → `npm install` → `npm start` (`:4260`)
3. Ligar Carga pela UI (sobe `Carga.DevHost` + `Executar=1` no cod **99**)

Documentação: `Doc/Documentacao_Monitor_Carga_Fiscal_CTe.md`  
RESUMO do original: `Doc/RESUMO_ALTERACOES_DESENVOLVIMENTO.md`
