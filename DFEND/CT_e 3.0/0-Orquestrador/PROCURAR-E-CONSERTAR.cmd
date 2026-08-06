@echo off
chcp 65001 >nul
title Procurar Orquestrador e consertar
REM Duplo clique. Procura Orquestrador.sln valido (com LIMPAR-E-BUILDAR.cmd).
REM Ignora Lixeira, Temp, AppData e pastas Ass-Monitores duplicadas.

echo.
echo Procurando Orquestrador.sln valido...
echo (ignora Lixeira / Temp / AppData)
echo.

set "FOUND="
set "ORQ_ROOT="

REM 1) Locais tipicos primeiro (rapido)
call :TRY "C:\Users\%USERNAME%\Desktop\Clones\Assefaz\Ass-Monitores\DFEND\CT_e 3.0\0-Orquestrador"
if defined FOUND goto :RUN
call :TRY "C:\Users\%USERNAME%\Desktop\Clones\DFEND\Ass-Monitores\DFEND\CT_e 3.0\0-Orquestrador"
if defined FOUND goto :RUN
call :TRY "C:\Users\%USERNAME%\Desktop\Clones\DFEND\CT_e 3.0\0-Orquestrador"
if defined FOUND goto :RUN
call :TRY "D:\Clones\Ass-Monitores\DFEND\CT_e 3.0\0-Orquestrador"
if defined FOUND goto :RUN
call :TRY "H:\Ass-Monitores\DFEND\CT_e 3.0\0-Orquestrador"
if defined FOUND goto :RUN
call :TRY "H:\Clones\Ass-Monitores\DFEND\CT_e 3.0\0-Orquestrador"
if defined FOUND goto :RUN

REM 2) Busca ampla, filtrando lixo
for %%D in (C D E F G H) do (
  if exist "%%D:\" (
    echo  - varrendo %%D:\ ...
    for /f "delims=" %%F in ('dir /s /b "%%D:\Orquestrador.sln" 2^>nul') do (
      call :CANDIDATE "%%F"
      if defined FOUND goto :RUN
    )
  )
)

echo.
echo NAO ACHEI um clone valido do Orquestrador.
echo.
echo O que precisa existir:
echo   ...\0-Orquestrador\Orquestrador.Api\Orquestrador.sln
echo   ...\0-Orquestrador\LIMPAR-E-BUILDAR.cmd
echo.
echo Se o projeto so esta na Lixeira, restaure a pasta 0-Orquestrador
echo ou faca git clone / git pull do Ass-Monitores de novo.
echo.
pause
exit /b 1

:RUN
echo.
echo Achei clone VALIDO:
echo   %FOUND%
echo.
echo Raiz:
echo   %ORQ_ROOT%
echo.

echo %ORQ_ROOT% | findstr /i "Ass-Monitores\\Ass-Monitores" >nul
if not errorlevel 1 (
  echo ERRO: Ass-Monitores DUPLICADO no path.
  echo Use um unico Ass-Monitores\DFEND\CT_e 3.0\0-Orquestrador
  echo.
  pause
  exit /b 2
)

echo %ORQ_ROOT% | findstr /i "Recycle.Bin" >nul
if not errorlevel 1 (
  echo ERRO: isto esta na Lixeira. Restaure o projeto primeiro.
  echo.
  pause
  exit /b 3
)

echo Rodando limpeza + build...
echo.
cd /d "%ORQ_ROOT%"
call "%ORQ_ROOT%\LIMPAR-E-BUILDAR.cmd"
exit /b %ERRORLEVEL%

:TRY
if defined FOUND goto :EOF
if exist "%~1\LIMPAR-E-BUILDAR.cmd" if exist "%~1\Orquestrador.Api\Orquestrador.sln" (
  set "ORQ_ROOT=%~1"
  set "FOUND=%~1\Orquestrador.Api\Orquestrador.sln"
  echo  + tipico OK: %~1
)
goto :EOF

:CANDIDATE
if defined FOUND goto :EOF
set "P=%~1"

echo %P% | findstr /i "Recycle.Bin \\Temp\\ \\AppData\\ \\node_modules\\ \\.git\\" >nul
if not errorlevel 1 goto :EOF

echo %P% | findstr /i "Ass-Monitores\\Ass-Monitores" >nul
if not errorlevel 1 goto :EOF

echo %P% | findstr /i "\\0-Orquestrador\\Orquestrador.Api\\Orquestrador.sln$" >nul
if errorlevel 1 goto :EOF

for %%I in ("%P%") do set "API_DIR=%%~dpI"
for %%I in ("%API_DIR%\..") do set "ROOT_TRY=%%~fI"

if not exist "%ROOT_TRY%\LIMPAR-E-BUILDAR.cmd" goto :EOF
if not exist "%ROOT_TRY%\tools\fix-dev.ps1" goto :EOF

set "ORQ_ROOT=%ROOT_TRY%"
set "FOUND=%P%"
echo  + valido: %P%
goto :EOF
