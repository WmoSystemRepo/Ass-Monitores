# DFEND CT-e Receptor — pacote portátil (Monitor · DEV)

Raiz canônica deste domínio na cadeia `CT_e`:

```text
1-Receptor/
├── dfend-cte-receptor-windowsservices/   # Windows Service ORIGINAL (código intocado)
├── Frontend/                             # Nx Angular — app cte-receptor :4200
├── Receptor.Api/                         # BFF ASP.NET Core (SQL + SignalR) :5010
│   └── Monitor.sln                       # abrir no VS 2022
├── tools/
│   └── Receptor.DevHost/                 # Host POC — Ligar sem InstallUtil
├── Doc/                                  # documentação técnica e operacional
└── Dockerfile · docker-compose.yml       # microserviço
```

Paths relativos a `1-Receptor/` (exceto cadeia: `0-Orquestrador/`, `2-Arquivador/`, `docker-compose.chain.yml`).

- **Documento mestre:** `Doc/Documentacao_Monitor_Receptor_Fiscal_CTe.md`
- **Contrato microserviço:** `Doc/CONTRATO_MICROSERVICO_MONITOR.md` (**v1.3**)
- Plano / ata: `CT_e/.cursor/plans/monitor_realtime_receptor_fded97f7.plan.md`
- Runbook: `Doc/Passo a passo execução do sistema Monitor Receptor.md`

## Pré-requisitos

- .NET 8 SDK + Visual Studio 2022
- Node.js LTS (npm) — no PowerShell use `npm.cmd` / `npx.cmd` se `ExecutionPolicy` bloquear
- SQL Server DEV alcançável (`bd_cte_recepcao`)
- **Não** é obrigatório ter o Windows Service instalado: DEV usa `PreferLocalProcess` + `Receptor.DevHost`

## Como rodar

### 1. Compilar o Host POC (uma vez / após clone)

```powershell
cd tools\Receptor.DevHost
# Build Debug → tools\Receptor.DevHost\bin\Debug\Receptor.DevHost.exe
```

### 2. API (Visual Studio 2022)

1. Abrir `Receptor.Api/Monitor.sln`
2. Startup: **Monitor.Api** · perfil **https**
3. **F5** → Swagger em `https://localhost:7116/swagger`
4. Front usa a API em **http://localhost:5010**

Alternativa:

```powershell
cd Receptor.Api
dotnet run --project src/Monitor.Api --launch-profile https
```

Connection string: `src/Monitor.Api/appsettings.Development.json` ou User Secrets.  
API key DEV: `Monitor:InternalApiKey` = `dev-cte-chain-key` (Orquestrador usa a mesma).

### 3. Front (Nx)

**Não use** `ng s` — este projeto é Nx; o CLI Angular global não precisa estar instalado.

```powershell
cd Frontend
npm.cmd install
npm.cmd start
# atalho Windows: .\serve.cmd
```

UI: `http://localhost:4200` (Ctrl+F5 após mudanças de UI).

### 4. Ligar o Receptor CT-e

No **Monitor** da UI: **Ligar Receptor CT-e** → sobe `Receptor.DevHost` + `Executar=1` e grava `monitor-live.log` (Debug online).  
**Desligar** encerra o host + `Executar=0`.

> F5 só no exe original **não** alimenta o Debug online do monitor. Use Ligar pelo Monitor.

## O que a UI entrega hoje

| Menu | Rota | Conteúdo |
|------|------|----------|
| Monitor | `/` | Anatomia (SEFAZ→tmp→Broker→Arquivador), AGORA, cards tabelas, Ligar/Desligar |
| Threads (hint: Linhas de trabalho) | `/threads` | Pool T1–T5 |
| Histórico | `/logs` | Timeline · Todos/Sucesso/Erros/Avisos |
| Tabelas | `/tabelas` | Hub + detalhe sessão (5 keys) |
| Configurações | `/config` | Somente leitura · configurações de origem (SQL) |

Rota auxiliar (link no fluxo, fora do menu): `/mais-informacoes`.

## Microserviço

- `GET /api/monitor/info` · `/health` · `/health/live` · `/health/ready`
- `/api/monitor/*` com `X-Cte-Internal-Api-Key`
- Headers `X-Monitor-Service` / `X-Monitor-Version`
- Orquestrador: `0-Orquestrador/Doc/Documentacao_Orquestrador_CTe.md`

## Regras

- Dados reais em **DEV** — sem mock; Homolog/Prod = scaffolding BFF + secrets/SCM
- Código do Windows Service **intocado**
- Escrita SQL só `UPDATE Executar` nos POSTs de serviço
- Snapshot DEV a cada **1 s**; logs ~1 s via SignalR

## Transferência

1. Copiar a pasta `1-Receptor/` completa.
2. Ajustar connection string no BFF (User Secrets preferível).
3. Compilar `tools/Receptor.DevHost` (Debug).
4. `npm.cmd install` em `Frontend/` se necessário.
5. Subir API + Front; Ligar pelo Monitor.
