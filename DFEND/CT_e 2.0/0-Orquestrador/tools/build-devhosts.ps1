# Compila todos os *.DevHost (Debug) usados pelo "Ligar cadeia" no LocalDev.
# Uso (na raiz CT_e):
#   powershell -ExecutionPolicy Bypass -File .\0-Orquestrador\tools\build-devhosts.ps1

$ErrorActionPreference = "Stop"
$cteRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..")

$vswhere = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"
$msbuild = $null
if (Test-Path $vswhere) {
  $msbuild = & $vswhere -latest -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\MSBuild.exe" | Select-Object -First 1
}
if (-not $msbuild) {
  throw "MSBuild não encontrado. Instale Visual Studio com carga de trabalho .NET desktop."
}

$projects = @(
  "1-Receptor\tools\Receptor.DevHost\Receptor.DevHost.csproj",
  "2-Arquivador\tools\Arquivador.DevHost\Arquivador.DevHost.csproj",
  "3-Sintetizador\tools\Sintetizador.DevHost\Sintetizador.DevHost.csproj",
  "4-Analisador\tools\Analisador.DevHost\Analisador.DevHost.csproj",
  "5-Integrador\tools\Integrador.DevHost\Integrador.DevHost.csproj",
  "6-Carga\tools\Carga.DevHost\Carga.DevHost.csproj"
)

Write-Host "CT_e: $cteRoot"
Write-Host "MSBuild: $msbuild"
foreach ($rel in $projects) {
  $proj = Join-Path $cteRoot $rel
  Write-Host "`n=== Building $rel ===" -ForegroundColor Cyan
  & $msbuild $proj /p:Configuration=Debug /v:m
  if ($LASTEXITCODE -ne 0) { throw "Falha ao compilar $rel" }
}

Write-Host "`nDevHosts OK. Reinicie o Orquestrador.Api e use Ligar cadeia." -ForegroundColor Green
