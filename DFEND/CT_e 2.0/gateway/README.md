# Gateway CT-e (nginx)

Ponto de entrada único da cadeia Docker na porta **8080**.

## Papel

Proxy reverso que unifica:

| Prefixo | Destino |
|---------|---------|
| `/` | Orquestrador front |
| `/api/orquestrador/` | Orquestrador API `:5000` |
| `/api/receptor/` | Receptor API `:5010` |
| `/api/arquivador/` | Arquivador API `:5020` |
| `/hubs/receptor/` | SignalR Receptor |
| `/hubs/arquivador/` | SignalR Arquivador |
| `/receptor/` | Front Receptor |
| `/arquivador/` | Front Arquivador |
| `/health` | `200 ok` |

Arquivos: `Dockerfile` + `nginx.conf`.

## Como sobe

Via compose da cadeia (não rodar sozinho em DEV local típico):

```powershell
cd "DFEND\CT_e 2.0"
$env:CTE_INTERNAL_API_KEY = "dev-cte-chain-key"
docker compose -f docker-compose.chain.yml up --build gateway
# ou a cadeia inteira:
docker compose -f docker-compose.chain.yml up --build
```

Acesso: `http://localhost:8080`

## Observações

- Em DEV com F5 (sem Docker), os fronts falam direto nas portas `4200`/`4210`/`4220`… — o gateway **não** é necessário.
- O `nginx.conf` atual cobre Orquestrador + Receptor + Arquivador de forma explícita; demais estágios entram pelo Orquestrador ou portas mapeadas no compose.
- Health do container: `GET /health`.

Ver também: [../README.md](../README.md) · [DOCUMENTACAO-SISTEMA.md](../../../DOCUMENTACAO-SISTEMA.md).
