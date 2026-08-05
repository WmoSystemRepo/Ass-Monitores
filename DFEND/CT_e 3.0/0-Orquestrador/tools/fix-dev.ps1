param(
  [switch]$NoPause
)

# Conserta o Orquestrador sem depender de path de usuario.
# Limpa bin/obj/.vs/_artifacts, restore e build.
# Chamado por LIMPAR-E-BUILDAR.cmd (duplo clique).

$ErrorActionPreference = "Stop"

function Wait-Exit([int]$code) {
  if (-not $NoPause) {
    Write-Host "Pressione Enter para fechar..."
    [void][Console]::ReadLine()
  }
  exit $code
}

$orqRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$sln = Join-Path $orqRoot "Orquestrador.Api\Orquestrador.sln"

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host " Orquestrador - limpar e buildar"
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Raiz: $orqRoot"
Write-Host ""

if ($orqRoot -match '(?i)Ass-Monitores[\\/]+Ass-Monitores') {
  Write-Host "AVISO: path com Ass-Monitores DUPLICADO." -ForegroundColor Red
  Write-Host "Clone errado. Use um unico Ass-Monitores no caminho." -ForegroundColor Yellow
  Write-Host "Exemplo certo: ...\Ass-Monitores\DFEND\CT_e 3.0\0-Orquestrador" -ForegroundColor Yellow
  Write-Host ""
  Wait-Exit 2
}

if (-not (Test-Path $sln)) {
  Write-Host "ERRO: nao achei Orquestrador.Api\Orquestrador.sln em:" -ForegroundColor Red
  Write-Host "  $orqRoot"
  Wait-Exit 1
}

Write-Host "[1/4] Limpando caches (bin/obj/.vs/_artifacts)..." -ForegroundColor Cyan
$removed = 0
Get-ChildItem -LiteralPath $orqRoot -Recurse -Directory -Force -ErrorAction SilentlyContinue |
  Where-Object { $_.Name -in @('bin', 'obj', '.vs') } |
  ForEach-Object {
    try {
      Remove-Item -LiteralPath $_.FullName -Recurse -Force -ErrorAction Stop
      $removed++
    } catch {
      Write-Host "  (aviso) nao removeu: $($_.FullName)" -ForegroundColor Yellow
    }
  }
$art = Join-Path $orqRoot "_artifacts"
if (Test-Path -LiteralPath $art) {
  try {
    Remove-Item -LiteralPath $art -Recurse -Force -ErrorAction Stop
    $removed++
  } catch {
    Write-Host "  (aviso) _artifacts em uso - feche o Visual Studio e rode de novo." -ForegroundColor Yellow
  }
}
Write-Host "  Pastas removidas: $removed"

Write-Host "[2/4] Validando estrutura..." -ForegroundColor Cyan
& (Join-Path $PSScriptRoot "verify-structure.ps1")
if ($LASTEXITCODE -ne 0) {
  Write-Host "Estrutura incompleta. Nao continue no VS ate sincronizar o clone." -ForegroundColor Red
  Wait-Exit $LASTEXITCODE
}

Write-Host "[3/4] dotnet restore..." -ForegroundColor Cyan
dotnet restore $sln --nologo
if ($LASTEXITCODE -ne 0) {
  Write-Host "RESTORE FALHOU." -ForegroundColor Red
  Wait-Exit $LASTEXITCODE
}

Write-Host "[4/4] dotnet build..." -ForegroundColor Cyan
dotnet build $sln --nologo
if ($LASTEXITCODE -ne 0) {
  Write-Host "BUILD FALHOU." -ForegroundColor Red
  Wait-Exit $LASTEXITCODE
}

Write-Host ""
Write-Host "OK - restore e build concluidos." -ForegroundColor Green
Write-Host "Agora abra a solution com ABRIR-SOLUTION.cmd (duplo clique)." -ForegroundColor Green
Write-Host ""
Wait-Exit 0
