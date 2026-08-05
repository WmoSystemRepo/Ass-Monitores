@echo off
chcp 65001 >nul
title Abrir Orquestrador.sln
cd /d "%~dp0"

set "SLN=%~dp0Orquestrador.Api\Orquestrador.sln"

if not exist "%SLN%" (
  echo ERRO: nao achei:
  echo   %SLN%
  echo.
  echo Rode antes LIMPAR-E-BUILDAR.cmd nesta mesma pasta.
  pause
  exit /b 1
)

echo Abrindo:
echo   %SLN%
echo.

REM Preferir Visual Studio se existir; senao o padrao do Windows
set "VS="
if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Professional\Common7\IDE\devenv.exe" set "VS=%ProgramFiles%\Microsoft Visual Studio\2022\Professional\Common7\IDE\devenv.exe"
if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\Common7\IDE\devenv.exe" set "VS=%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\Common7\IDE\devenv.exe"
if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" set "VS=%ProgramFiles%\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe"
if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" (
  for /f "usebackq tokens=*" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -find Common7\IDE\devenv.exe`) do set "VS=%%i"
)

if defined VS (
  start "" "%VS%" "%SLN%"
) else (
  start "" "%SLN%"
)

exit /b 0
