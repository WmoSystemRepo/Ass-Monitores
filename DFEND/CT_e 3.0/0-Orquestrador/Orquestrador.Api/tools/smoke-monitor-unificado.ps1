# Smoke — Monitor Unificado (W6 pós-limpeza de dependência)
# Uso: pwsh ./smoke-monitor-unificado.ps1
# Pré-requisito: Orquestrador.Api escutando em http://localhost:5000

$ErrorActionPreference = "Stop"
$base = "http://localhost:5000"

function Assert-Ok([string]$path) {
  $url = "$base$path"
  Write-Host "GET $url ..."
  $r = Invoke-WebRequest -Uri $url -UseBasicParsing -TimeoutSec 15
  if ($r.StatusCode -ge 400) { throw "Falha $url → $($r.StatusCode)" }
  Write-Host "  OK $($r.StatusCode)"
}

Assert-Ok "/api/health/live"
Assert-Ok "/api/health/ready"
Assert-Ok "/health/live"
Assert-Ok "/api/monitores/receptor/info"
Assert-Ok "/api/monitores/carga/info"
Assert-Ok "/api/monitores/arquivador/service/status"

Write-Host ""
Write-Host "Smoke Monitor Unificado: PASSOU" -ForegroundColor Green
