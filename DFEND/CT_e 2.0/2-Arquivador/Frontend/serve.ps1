# Sobe o Monitor CT-e Arquivador (Nx). Nao depende de "ng" no PATH.
Set-Location $PSScriptRoot

if (-not (Get-Command node -ErrorAction SilentlyContinue)) {
  Write-Error "Node.js nao encontrado no PATH. Instale Node LTS e reabra o terminal."
  exit 1
}

if (-not (Test-Path "node_modules\nx\bin\nx.js")) {
  Write-Host "[INFO] Instalando dependencias (npm install)..."
  npm.cmd install
  if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}

Write-Host "[INFO] Iniciando cte-arquivador em http://localhost:4210"
npm.cmd start @args
