# Sobe o Orquestrador CT-e (Nx). Nao depende de "ng" no PATH.
Set-Location $PSScriptRoot

if (-not (Get-Command node -ErrorAction SilentlyContinue)) {
  Write-Error "Node.js nao encontrado no PATH. Instale Node 20/22/24 e reabra o terminal."
  exit 1
}

if (-not (Test-Path "node_modules\nx\bin\nx.js")) {
  Write-Host "[INFO] Instalando dependencias (npm.cmd install)..."
  npm.cmd install
  if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}

if (-not (Test-Path "node_modules\nx\bin\nx.js")) {
  Write-Error "node_modules\nx\bin\nx.js ausente apos install. Apague node_modules e rode npm.cmd install."
  exit 1
}

Write-Host "[INFO] Iniciando cte-orquestrador em http://localhost:4220"
npm.cmd start @args
