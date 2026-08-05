@echo off
REM Gera/atualiza o zip para levar ao PC wmoliveira.
setlocal
cd /d "%~dp0"
powershell -NoProfile -ExecutionPolicy Bypass -Command ^
  "Compress-Archive -Path '.\_sync-localdev\*' -DestinationPath '.\sync-localdev-wmoliveira.zip' -Force; Write-Host 'OK:' (Resolve-Path '.\sync-localdev-wmoliveira.zip')"
