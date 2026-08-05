@echo off
chcp 65001 >nul
title Procurar Orquestrador e consertar
REM Coloque este .cmd em qualquer pasta (Desktop, H:\, etc.) e de duplo clique.
REM Ele procura Orquestrador.sln e roda o conserto automaticamente.

echo.
echo Procurando Orquestrador.sln em discos comuns...
echo (pode demorar 1-2 minutos)
echo.

set "FOUND="

REM Locais tipicos (sem precisar digitar path)
for %%D in (C D E F G H) do (
  if exist "%%D:\" (
    echo  - varrendo %%D:\ ...
    for /f "delims=" %%F in ('dir /s /b "%%D:\Orquestrador.sln" 2^>nul') do (
      echo %%F | findstr /i "\\0-Orquestrador\\Orquestrador.Api\\Orquestrador.sln" >nul
      if not errorlevel 1 (
        set "FOUND=%%F"
        goto :FOUND_ONE
      )
    )
  )
)

:FOUND_ONE
if not defined FOUND (
  echo.
  echo NAO ACHEI Orquestrador.sln.
  echo Verifique se o clone CT_e 3.0\0-Orquestrador existe neste PC.
  echo.
  pause
  exit /b 1
)

echo.
echo Achei:
echo   %FOUND%
echo.

REM Detecta Ass-Monitores duplicado
echo %FOUND% | findstr /i "Ass-Monitores\\Ass-Monitores" >nul
if not errorlevel 1 (
  echo ERRO: caminho com Ass-Monitores DUPLICADO.
  echo Isso quebra o Visual Studio.
  echo.
  echo Apague/mova o clone interno e use so:
  echo   ...\Ass-Monitores\DFEND\CT_e 3.0\0-Orquestrador
  echo.
  pause
  exit /b 2
)

for %%I in ("%FOUND%") do set "API_DIR=%%~dpI"
for %%I in ("%API_DIR%\..") do set "ORQ_ROOT=%%~fI"

echo Raiz Orquestrador:
echo   %ORQ_ROOT%
echo.
echo Rodando limpeza + build...
echo.

cd /d "%ORQ_ROOT%"
if exist "%ORQ_ROOT%\LIMPAR-E-BUILDAR.cmd" (
  call "%ORQ_ROOT%\LIMPAR-E-BUILDAR.cmd"
) else if exist "%ORQ_ROOT%\tools\fix-dev.ps1" (
  powershell -NoProfile -ExecutionPolicy Bypass -File "%ORQ_ROOT%\tools\fix-dev.ps1"
) else (
  echo ERRO: LIMPAR-E-BUILDAR.cmd / fix-dev.ps1 nao encontrados nesse clone.
  echo Faca git pull na pasta Ass-Monitores e tente de novo.
  pause
  exit /b 1
)

exit /b %ERRORLEVEL%
