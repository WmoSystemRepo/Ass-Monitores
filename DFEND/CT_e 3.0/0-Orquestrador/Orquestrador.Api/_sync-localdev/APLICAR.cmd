@echo off
setlocal EnableExtensions
cd /d "%~dp0"

REM Este .cmd deve ficar DENTRO de _sync-localdev (ou do zip extraido).
REM Aplica os arquivos no Orquestrador.Api pai e recompila.

set "KIT=%~dp0"
set "API=%~dp0.."

if not exist "%API%\Orquestrador.sln" (
  echo ERRO: Orquestrador.sln nao encontrado em "%API%"
  echo Coloque a pasta _sync-localdev dentro de Orquestrador.Api e rode este .cmd de novo.
  exit /b 1
)

echo ==> Alvo: %API%
echo ==> Removendo pasta antiga Process\ ...
if exist "%API%\src\Orquestrador.Infrastructure\Process" (
  rmdir /s /q "%API%\src\Orquestrador.Infrastructure\Process"
  echo     Process\ removida.
) else (
  echo     Process\ nao existia.
)

echo ==> Criando LocalDev\ ...
mkdir "%API%\src\Orquestrador.Infrastructure\LocalDev" 2>nul
copy /Y "%KIT%Orquestrador.Infrastructure\LocalDev\*.cs" "%API%\src\Orquestrador.Infrastructure\LocalDev\" >nul
copy /Y "%KIT%Orquestrador.Infrastructure\DependencyInjection.cs" "%API%\src\Orquestrador.Infrastructure\" >nul
copy /Y "%KIT%Orquestrador.Application\Abstractions\IMonitorProcessLauncher.cs" "%API%\src\Orquestrador.Application\Abstractions\" >nul
copy /Y "%KIT%Orquestrador.Application\Options\OrchestratorOptions.cs" "%API%\src\Orquestrador.Application\Options\" >nul
copy /Y "%KIT%Orquestrador.Api\appsettings.Development.json" "%API%\src\Orquestrador.Api\" >nul
copy /Y "%KIT%reset-build.cmd" "%API%\" >nul
copy /Y "%KIT%reset-build.ps1" "%API%\" >nul

echo ==> Limpando bin/obj ...
for /d /r "%API%\src" %%D in (bin,obj) do (
  if exist "%%D" rmdir /s /q "%%D"
)
if exist "%API%\.vs" rmdir /s /q "%API%\.vs"

echo ==> Compilando ...
pushd "%API%"
dotnet build Orquestrador.sln --no-incremental
set "EC=%ERRORLEVEL%"
popd

if not "%EC%"=="0" (
  echo ==> FALHOU. Codigo %EC%
  exit /b %EC%
)

echo ==> OK. Reabra Orquestrador.sln no Visual Studio.
exit /b 0
