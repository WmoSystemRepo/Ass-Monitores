# CTe.Resgate (7-Resgate)

## Princípio

**O Resgate não implementa o mecanismo de download.**  
Só **informa** quais chaves de CT-e processar. A **Carga** (`ProcessarDownload`) executa o download e disponibiliza o documento para a **continuidade do fluxo normal do sistema**.

- Entrada: **somente chave de acesso** (44 dígitos) — **NSU fora do escopo**
- Escopo: **somente CT-e**
- Sem tabelas novas de lote no caminho feliz

## Run local

```bash
cd 7-Resgate/src/CTe.Resgate.Api
dotnet run
```

- API: http://localhost:5070  
- UI: http://localhost:4220/resgate  
- Login DEV: `dev` / `dev`  
- Logs: `C:\Users\wmoliveira\Desktop\Clones\CT_e 2.0\Logs\resgate-YYYYMMDD.log`

## Pré-requisitos (Carga)

| Item | Valor |
|------|--------|
| Serviço | DFEND_CTe_Carga |
| CodServico | 99 |
| Flags | `Executar=1`, `ExecutarAuto=1` |
| Monitor | http://localhost:4260 |

**Enfileirado ≠ resgatado.** Acompanhe fila/status na UI ou no Monitor Carga.

## Como funciona (decisão técnica)

1. Operador cola/envia 1–1000 chaves  
2. API grava na temp (`des_esquema` = chave — implementação atual da Carga, identificada na análise) + `SEND` em `fila_alvo_cte_integrador`  
3. Carga `ProcessarDownload` consome, consulta AN e persiste no sintético  

### Fila compartilhada (risco)

Consumidores no código: **Carga** e **Integrador** (`RetirarFilaIntegrador`).  
Preferir janela com Carga ativa; cuidado com concorrência/atraso se Integrador também consumir.

## Endpoints

| Método | Rota | Uso |
|--------|------|-----|
| POST | `/api/resgate/lotes` | Enfileirar chaves |
| POST | `/api/resgate/lotes/upload` | Upload CSV/TXT/XLSX |
| GET | `/api/resgate/fila-download` | Pendentes na temp + profundidade broker |
| POST | `/api/resgate/status-chaves` | Status: Pendente / Erro / Baixado / Indeterminado |

## Legado (não usar no caminho feliz)

- `SqlResgateStore` / `ResgateWorker` / `sql/001_create_resgate_tables.sql` — desenho antigo com `lote_resgate_*` (isolado, fora do DI)

## Homologação

Ver [`docs/ROTEIRO-HOMOLOGACAO-RESGATE.md`](docs/ROTEIRO-HOMOLOGACAO-RESGATE.md) (V1–V6) e [`docs/DECISOES-E-HIPOTESES.md`](docs/DECISOES-E-HIPOTESES.md).

## Config DEV

```json
{
  "ConnectionStrings": {
    "BDCTeSintetico": "Data Source=DDFESIN\\BDD_DFE_SINTETIC;Initial Catalog=bd_cte_sintetico;Integrated Security=SSPI;Connect Timeout=60;TrustServerCertificate=True"
  }
}
```
