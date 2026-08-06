@echo off
chcp 65001 >nul
title Procurar Orquestrador e consertar
REM Copia na raiz Ass-Monitores — mesmo conteudo do 0-Orquestrador.

echo.
echo Procurando Orquestrador.sln valido...
echo (ignora Lixeira / Temp / AppData)
echo.

set "FOUND="
set "ORQ_ROOT="

call :TRY "%~dp0DFEND\CT_e 3.0\0-Orquestrador"
if defined FOUND goto :RUN
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
echo Precisa ter LIMPAR-E-BUILDAR.cmd + Orquestrador.sln.
echo Se so esta na Lixeira: restaure a pasta ou faca git pull do Ass-Monitores.
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
  pause
  exit /b 2
)

echo %ORQ_ROOT% | findstr /i "Recycle.Bin" >nul
if not errorlevel 1 (
  echo ERRO: isto esta na Lixeira. Restaure o projeto primeiro.
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
