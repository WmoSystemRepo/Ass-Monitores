@echo off
cd /d "%~dp0"
where node >nul 2>&1
if errorlevel 1 (
  echo [ERRO] Node.js nao encontrado no PATH.
  exit /b 1
)
if not exist "node_modules\nx\bin\nx.js" (
  echo [INFO] npm install...
  call npm.cmd install
  if errorlevel 1 exit /b 1
)
if not exist "node_modules\nx\bin\nx.js" (
  echo [ERRO] node_modules\nx\bin\nx.js ausente apos npm install.
  echo [ERRO] Apague node_modules e package-lock.json, rode npm.cmd install de novo.
  exit /b 1
)
echo [INFO] Subindo Angular dos monitores abaixo (se offline)...
node ".\tools\start-chain-fronts.cjs"
echo [INFO] Orquestrador CT-e em http://localhost:4220
node ".\node_modules\nx\bin\nx.js" serve cte-orquestrador %*
