# Limpa artefatos de build (paths absolutos de outra maquina ficam em bin/obj/.vs)
# e recompila a solucao. Rode na pasta Orquestrador.Api:
#   .\reset-build.ps1
param(
    [switch]$SkipBuild
)

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$sln = Join-Path $root 'Orquestrador.sln'

Write-Host '==> CT_e Orquestrador — reset de build' -ForegroundColor Cyan
Write-Host "    Pasta: $root"

$patterns = @('bin', 'obj', '.vs')
$removed = 0
Get-ChildItem -Path (Join-Path $root 'src') -Directory -Recurse -ErrorAction SilentlyContinue |
    Where-Object { $patterns -contains $_.Name } |
    ForEach-Object {
        Write-Host "    removendo $($_.FullName)"
        Remove-Item -LiteralPath $_.FullName -Recurse -Force -ErrorAction SilentlyContinue
        $removed++
    }

$vsRoot = Join-Path $root '.vs'
if (Test-Path $vsRoot) {
    Write-Host "    removendo $vsRoot"
    Remove-Item -LiteralPath $vsRoot -Recurse -Force -ErrorAction SilentlyContinue
    $removed++
}

Write-Host "    $removed pastas de cache removidas" -ForegroundColor DarkGray

$oldProcess = Join-Path $root 'src\Orquestrador.Infrastructure\Process'
if (Test-Path $oldProcess) {
    Write-Host '    Removendo pasta ANTIGA Process\ (substituida por LocalDev\)...' -ForegroundColor Yellow
    Remove-Item -LiteralPath $oldProcess -Recurse -Force
}

$launcher = Join-Path $root 'src\Orquestrador.Infrastructure\LocalDev\MonitorProcessLauncher.cs'
$required = @(
    'IsApiReadyAsync',
    'IsFrontendReachableAsync',
    'EnsureApiReadyAsync',
    'EnsureFrontendAsync'
)

if (-not (Test-Path $launcher)) {
    throw "MonitorProcessLauncher.cs nao encontrado: $launcher"
}

$content = Get-Content -LiteralPath $launcher -Raw
$missing = @($required | Where-Object { $content -notmatch "Task<bool>\s+$_\s*\(" })
if ($missing.Count -gt 0) {
    throw "MonitorProcessLauncher.cs incompleto. Faltam: $($missing -join ', '). Sincronize o arquivo do clone atualizado."
}

Write-Host '    MonitorProcessLauncher: OK (4 metodos da interface)' -ForegroundColor Green

if ($SkipBuild) {
    Write-Host '==> Limpeza concluida (-SkipBuild)' -ForegroundColor Cyan
    exit 0
}

Write-Host "==> dotnet build $sln" -ForegroundColor Cyan
Push-Location $root
try {
    dotnet build $sln --no-incremental
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}
finally {
    Pop-Location
}

Write-Host '==> Build concluido com sucesso.' -ForegroundColor Green
