# Dev portátil — Orquestrador CT-e

> Como abrir e buildar o Orquestrador em **qualquer máquina**, sem depender de `C:\Users\...`.  
> Atualizado: 05/08/2026

## 1. Objetivo

Evitar erros recorrentes ao trocar de PC (Mendes ↔ wmoliveira) ou de pasta (`CT_e` / `CT_e 3.0` / `Ass-Monitores`):

| Sintoma típico | Causa comum |
|----------------|-------------|
| `<<<<<<< HEAD` em `project.nuget.cache` | `bin`/`obj` versionados + merge |
| Duplicar `Assembly*Attribute` | Cache `obj` conflictado / antigo |
| `GeneratedMSBuildEditorConfig` / MAX_PATH | Path muito longo (`Ass-Monitores\Ass-Monitores\...`) |
| “Não é possível encontrar as informações do projeto” | VS abrindo clone errado / Lixeira / restore falhou |
| Script achou `$Recycle.Bin\...` | Clone deletado; busca pegou Lixeira |

## 2. One-click (recomendado)

Na pasta `0-Orquestrador` (Explorer → duplo clique):

| Arquivo | O que faz |
|---------|-----------|
| **`LIMPAR-E-BUILDAR.cmd`** | Remove `bin`/`obj`/`.vs`/`_artifacts`, valida estrutura, `dotnet restore` + `dotnet build` |
| **`ABRIR-SOLUTION.cmd`** | Abre `Orquestrador.Api\Orquestrador.sln` no Visual Studio |
| **`PROCURAR-E-CONSERTAR.cmd`** | Procura clone **válido** nos discos; ignora Lixeira/Temp/AppData; exige `LIMPAR-E-BUILDAR.cmd` |

Fluxo:

1. Feche o Visual Studio.  
2. `LIMPAR-E-BUILDAR.cmd` → espere **OK**.  
3. `ABRIR-SOLUTION.cmd`.  

Texto curto: [../COMO-USAR.txt](../COMO-USAR.txt).

### Se não achar a pasta

1. Copie `PROCURAR-E-CONSERTAR.cmd` para o Desktop (há cópia também na raiz `Ass-Monitores`).  
2. Duplo clique.  
3. Se disser que **não achou** ou só havia Lixeira: restaure o projeto da Lixeira **ou** `git pull` / clone de novo do `Ass-Monitores`.

## 3. Paths portáteis (como o código resolve)

- **Raiz de trabalho** = pasta `0-Orquestrador` (contém `engines` + `Orquestrador.Api`).  
  O nome do pai (`CT_e`, `CT_e 2.0`, `CT_e 3.0`) não importa.
- **PackageFolder** dos monitores: `engines\receptor`, `engines\arquivador`, … (relativo à raiz acima).
- **`CtePathResolver` / `RepoRootResolver`**: sobem a partir do ContentRoot/BaseDirectory do **processo atual**. Paths absolutos de outro usuário/clone (`C:\Users\outro\...`) são **ignorados ou remapeados** pelo sufixo estável (`engines\…`, `0-Orquestrador\…`) — a árvore do processo sempre vence.
- **`LocalDev:RepoRoot`** e **`Monitors:*:RootPath`**: deixe **vazios**. Não grave `C:\Users\outro\...`.
- Mensagens de erro do DevHost usam path **relativo** (`engines\receptor\tools\...`), nunca o prefixo da máquina.

## 4. Artefatos de build

| Item | Onde | Git |
|------|------|-----|
| Saída MSBuild | `0-Orquestrador\_artifacts\` | **ignorado** (`.gitignore`) |
| `bin` / `obj` antigos sob `src` / `libs` | limpos pelo script | **não versionar** |

`Directory.Build.props` (raiz e `Orquestrador.Api`) redireciona `BaseIntermediateOutputPath` / `BaseOutputPath` para `_artifacts`, reduzindo o comprimento do path (MAX_PATH ~260).

## 5. Path certo vs errado

**Certo (exemplo de sufixo):**

```text
...\Ass-Monitores\DFEND\CT_e 3.0\0-Orquestrador
```

**Errado:**

```text
...\Ass-Monitores\Ass-Monitores\DFEND\CT_e 3.0\0-Orquestrador   ← pasta duplicada
C:\$Recycle.Bin\...\0-Orquestrador                               ← Lixeira
```

O Visual Studio mostra o path absoluto da máquina atual (`wmoliveira`, `Mendes`, …). Isso é normal **se** o clone for o válido; o problema é abrir o clone errado.

## 6. Comandos manuais (PowerShell)

Não cole comandos PowerShell no **Prompt de Comando (CMD)**.

```powershell
cd <pasta>\0-Orquestrador
powershell -NoProfile -ExecutionPolicy Bypass -File .\tools\verify-structure.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File .\tools\fix-dev.ps1
dotnet restore .\Orquestrador.Api\Orquestrador.sln
dotnet build   .\Orquestrador.Api\Orquestrador.sln
```

No CMD equivalente (só se preferir):

```bat
cd /d <pasta>\0-Orquestrador
LIMPAR-E-BUILDAR.cmd
```

## 7. Checklist quando o VS quebra

1. Fechar o VS.  
2. Rodar `LIMPAR-E-BUILDAR.cmd`.  
3. Abrir **somente** via `ABRIR-SOLUTION.cmd` (não reabrir atalho antigo).  
4. Confirmar que **não** há `Ass-Monitores\Ass-Monitores` nem `$Recycle.Bin` no path.  
5. `git pull` no `Ass-Monitores` para pegar scripts/gitignore atualizados.  
6. Nunca commitar `_artifacts`, `bin` ou `obj`.

## 8. Relação com o resto da doc

- Operação Ligar/Desligar / ambientes: [Documentacao_Orquestrador_CTe.md](Documentacao_Orquestrador_CTe.md)  
- Passo a passo F5 + front: [Passo a passo execução Orquestrador.md](Passo%20a%20passo%20execução%20Orquestrador.md)  
- Plugar microserviço: [ONBOARDING_MICROSERVICO.md](ONBOARDING_MICROSERVICO.md)
