# Valida a estrutura minima do 0-Orquestrador com paths relativos ao script.
# Nao usa C:\Users\... — roda em qualquer maquina/clone.
#
# Uso:
#   powershell -ExecutionPolicy Bypass -File .\tools\verify-structure.ps1

$ErrorActionPreference = "Stop"
$orqRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$failed = New-Object System.Collections.Generic.List[string]

function Require-Path([string]$rel) {
  $full = Join-Path $orqRoot $rel
  if (Test-Path $full) {
    Write-Host "  OK  $rel" -ForegroundColor DarkGreen
  } else {
    Write-Host "  FALTA $rel" -ForegroundColor Red
    [void]$failed.Add($rel)
  }
}

Write-Host "Raiz: $orqRoot"
Write-Host ""
Write-Host "Projetos API / solution:"
Require-Path "Orquestrador.Api\Orquestrador.sln"
Require-Path "Orquestrador.Api\src\Orquestrador.Api\Orquestrador.Api.csproj"
Require-Path "Orquestrador.Api\src\Orquestrador.Application\Orquestrador.Application.csproj"
Require-Path "Orquestrador.Api\src\Orquestrador.Infrastructure\Orquestrador.Infrastructure.csproj"
Require-Path "Orquestrador.Api\src\Orquestrador.Domain\Orquestrador.Domain.csproj"

Write-Host ""
Write-Host "Resgate:"
Require-Path "libs\resgate\CTe.Resgate.Domain\CTe.Resgate.Domain.csproj"
Require-Path "libs\resgate\CTe.Resgate.Application\CTe.Resgate.Application.csproj"
Require-Path "libs\resgate\CTe.Resgate.Infrastructure\CTe.Resgate.Infrastructure.csproj"

Write-Host ""
Write-Host "Engines / DevHosts:"
$map = @{
  receptor = "Receptor"
  arquivador = "Arquivador"
  sintetizador = "Sintetizador"
  analisador = "Analisador"
  integrador = "Integrador"
  carga = "Carga"
}
foreach ($svc in @("receptor","arquivador","sintetizador","analisador","integrador","carga")) {
  $name = $map[$svc]
  Require-Path "engines\$svc\tools\$name.DevHost\$name.DevHost.csproj"
}

Write-Host ""
Write-Host "Frontend:"
Require-Path "Frontend\package.json"
Require-Path "Frontend\apps\cte-orquestrador\project.json"

if ($failed.Count -gt 0) {
  Write-Host ""
  Write-Host "$($failed.Count) item(ns) faltando. Clone incompleto - nao abra a .sln ate sincronizar." -ForegroundColor Red
  Write-Host "NAO configure LocalDev:RepoRoot com path absoluto de outra maquina." -ForegroundColor Yellow
  exit 1
}

Write-Host ""
Write-Host "Estrutura OK. Proximos passos (paths relativos):" -ForegroundColor Green
Write-Host "  dotnet restore .\Orquestrador.Api\Orquestrador.sln"
Write-Host "  dotnet build   .\Orquestrador.Api\Orquestrador.sln"
Write-Host "  powershell -File .\tools\build-devhosts.ps1"
exit 0
