# Compila todos os *.DevHost (Debug) usados pelo "Ligar cadeia" no LocalDev.
# Paths relativos à pasta 0-Orquestrador — funciona em qualquer máquina/clone
# (CT_e, CT_e 2.0, CT_e 3.0, Users\Mendes, Users\wmoliveira, …).
#
# Uso (de qualquer cwd):
#   powershell -ExecutionPolicy Bypass -File .\tools\build-devhosts.ps1

$ErrorActionPreference = "Stop"
$orqRoot = Resolve-Path (Join-Path $PSScriptRoot "..")

$vswhere = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"
$msbuild = $null
if (Test-Path $vswhere) {
  $msbuild = & $vswhere -latest -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\MSBuild.exe" | Select-Object -First 1
}
if (-not $msbuild) {
  throw "MSBuild não encontrado. Instale Visual Studio com carga de trabalho .NET desktop."
}

$projects = @(
  "engines\receptor\tools\Receptor.DevHost\Receptor.DevHost.csproj",
  "engines\arquivador\tools\Arquivador.DevHost\Arquivador.DevHost.csproj",
  "engines\sintetizador\tools\Sintetizador.DevHost\Sintetizador.DevHost.csproj",
  "engines\analisador\tools\Analisador.DevHost\Analisador.DevHost.csproj",
  "engines\integrador\tools\Integrador.DevHost\Integrador.DevHost.csproj",
  "engines\carga\tools\Carga.DevHost\Carga.DevHost.csproj"
)

Write-Host "0-Orquestrador: $orqRoot"
Write-Host "MSBuild: $msbuild"
foreach ($rel in $projects) {
  $proj = Join-Path $orqRoot $rel
  if (-not (Test-Path $proj)) {
    throw "Projeto ausente: $rel (clone incompleto?)"
  }
  Write-Host "`n=== Building $rel ===" -ForegroundColor Cyan
  & $msbuild $proj /p:Configuration=Debug /v:m
  if ($LASTEXITCODE -ne 0) { throw "Falha ao compilar $rel" }
}

Write-Host "`nDevHosts OK. Reinicie o Orquestrador.Api e use Ligar cadeia." -ForegroundColor Green
