@echo off
chcp 65001 >nul
title Orquestrador - Limpar e Buildar
cd /d "%~dp0"

echo.
echo  Duplo-clique neste arquivo. Nao precisa copiar caminho.
echo  Pasta atual: %CD%
echo.

where powershell >nul 2>&1
if errorlevel 1 (
  echo ERRO: PowerShell nao encontrado.
  pause
  exit /b 1
)

where dotnet >nul 2>&1
if errorlevel 1 (
  echo ERRO: dotnet nao encontrado. Instale o .NET 8 SDK.
  pause
  exit /b 1
)

powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0tools\fix-dev.ps1"
exit /b %ERRORLEVEL%
