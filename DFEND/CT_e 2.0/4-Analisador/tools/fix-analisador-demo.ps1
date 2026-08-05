# Unblock Analisador front (:4240) -> API (:5040) for DEV/demo.
# Run from ANY folder on the CT_e 2.0 machine.
$ErrorActionPreference = 'Stop'
$root = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
if (-not (Test-Path (Join-Path $root '4-Analisador'))) {
  $root = (Get-Location).Path
  if (-not (Test-Path (Join-Path $root '4-Analisador'))) {
    $root = Split-Path (Get-Location).Path -Parent
  }
}

$apiDir = Join-Path $root '4-Analisador\Analisador.Api\src\Monitor.Api'
$program = Join-Path $apiDir 'Program.cs'
$devJson = Join-Path $apiDir 'appsettings.Development.json'

Write-Host "Repo: $root"
if (-not (Test-Path $program)) { throw "Program.cs não encontrado: $program" }

# 1) CORS 4240
$cs = Get-Content $program -Raw
$cs2 = $cs.Replace('http://localhost:4230', 'http://localhost:4240').Replace('http://127.0.0.1:4230', 'http://127.0.0.1:4240')
if ($cs2 -ne $cs) {
  Set-Content -Path $program -Value $cs2 -Encoding UTF8
  Write-Host "CORS: 4230 -> 4240"
} else {
  Write-Host "CORS: já em 4240 (ou texto diferente)"
}

# 2) DEV sem API key no browser (middleware libera /api/monitor/*)
$json = Get-Content $devJson -Raw
$json2 = $json -replace '"InternalApiKey"\s*:\s*"[^"]*"', '"InternalApiKey": ""'
Set-Content -Path $devJson -Value $json2 -Encoding UTF8
Write-Host "InternalApiKey DEV: vazio (front Angular não precisa do header)"

# 3) Liberar porta 5040
$conns = Get-NetTCPConnection -LocalPort 5040 -ErrorAction SilentlyContinue |
  Where-Object { $_.State -eq 'Listen' }
foreach ($c in $conns) {
  Write-Host "Encerrando PID $($c.OwningProcess) na 5040"
  Stop-Process -Id $c.OwningProcess -Force -ErrorAction SilentlyContinue
}

# 4) Rebuild + run API
Set-Location $apiDir
dotnet build
Write-Host ""
Write-Host "Subindo API em http://localhost:5040 ..."
Write-Host "Depois abra http://localhost:4240 e Ctrl+F5"
dotnet run --launch-profile http
