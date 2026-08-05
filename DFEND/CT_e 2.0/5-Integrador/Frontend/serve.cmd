@echo off
REM Sobe o Monitor CT-e Integrador (Nx). Nao usa "ng" global.
cd /d "%~dp0"

where node >nul 2>&1
if errorlevel 1 (
  echo [ERRO] Node.js nao encontrado no PATH. Instale Node LTS e reabra o terminal.
  exit /b 1
)

if not exist "node_modules\nx\bin\nx.js" (
  echo [INFO] Instalando dependencias ^(npm install^)...
  call npm.cmd install
  if errorlevel 1 exit /b 1
)

echo [INFO] Iniciando cte-integrador em http://localhost:4250
call npx.cmd nx serve cte-integrador %*
