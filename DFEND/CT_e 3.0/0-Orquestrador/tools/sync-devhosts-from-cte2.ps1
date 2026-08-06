# Sincroniza DevHosts (+ bin\Debug) do CT_e 2.0 para o Orquestrador 3.0.
# Uso (na pasta 0-Orquestrador):
#   powershell -NoProfile -ExecutionPolicy Bypass -File .\tools\sync-devhosts-from-cte2.ps1
#
# Destinos:
#   1) legado-CT_e-2.0\<N-Servico>\   — espelho claro da origem
#   2) engines\<servico>\tools\*.DevHost\ — onde a API procura o .exe

$ErrorActionPreference = "Stop"
$orqRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$dfend = Resolve-Path (Join-Path $orqRoot "..\..")
$cte2 = Join-Path $dfend "CT_e 2.0"

if (-not (Test-Path -LiteralPath $cte2)) {
  Write-Host "ERRO: nao achei CT_e 2.0 em: $cte2" -ForegroundColor Red
  exit 1
}

$map = @(
  @{ Src = "1-Receptor";      Eng = "receptor";      Host = "Receptor.DevHost" },
  @{ Src = "2-Arquivador";    Eng = "arquivador";    Host = "Arquivador.DevHost" },
  @{ Src = "3-Sintetizador";  Eng = "sintetizador";  Host = "Sintetizador.DevHost" },
  @{ Src = "4-Analisador";    Eng = "analisador";    Host = "Analisador.DevHost" },
  @{ Src = "5-Integrador";    Eng = "integrador";    Host = "Integrador.DevHost" },
  @{ Src = "6-Carga";         Eng = "carga";         Host = "Carga.DevHost" }
)

$legadoRoot = Join-Path $orqRoot "legado-CT_e-2.0"
New-Item -ItemType Directory -Force -Path $legadoRoot | Out-Null

$readme = @"
# legado-CT_e-2.0

Espelho dos DevHosts (e deps) copiados de ``CT_e 2.0\<N-Servico>\``.

| Pasta aqui | Origem CT_e 2.0 | Engine 3.0 (API) |
|------------|-----------------|------------------|
| 1-Receptor | CT_e 2.0\1-Receptor\tools\Receptor.DevHost | engines\receptor\tools\Receptor.DevHost |
| 2-Arquivador | CT_e 2.0\2-Arquivador\tools\... | engines\arquivador\tools\... |
| 3-Sintetizador | ... | engines\sintetizador\tools\... |
| 4-Analisador | ... | engines\analisador\tools\... |
| 5-Integrador | ... | engines\integrador\tools\... |
| 6-Carga | ... | engines\carga\tools\... |

A API LocalDev usa **somente** ``engines\<servico>\tools\*.DevHost\bin\Debug\*.exe``.
Esta pasta ``legado-CT_e-2.0`` documenta a origem; o sync tambem atualiza ``engines\``.
"@
Set-Content -LiteralPath (Join-Path $legadoRoot "README.md") -Value $readme -Encoding UTF8

function Copy-Tree([string]$from, [string]$to) {
  if (-not (Test-Path -LiteralPath $from)) {
    Write-Host "  SKIP origem ausente: $from" -ForegroundColor Yellow
    return $false
  }
  New-Item -ItemType Directory -Force -Path $to | Out-Null
  # /E dirs, /XO older, /NFL /NDL quieter, /R:1 /W:1
  & robocopy $from $to /E /XO /NFL /NDL /NJH /NJS /R:1 /W:1 | Out-Null
  $code = $LASTEXITCODE
  if ($code -ge 8) {
    Write-Host "  robocopy falhou ($code): $from -> $to" -ForegroundColor Red
    return $false
  }
  return $true
}

Write-Host "CT_e 2.0 : $cte2"
Write-Host "Orquestrador: $orqRoot"
Write-Host ""

$ok = 0
foreach ($m in $map) {
  $srcHost = Join-Path $cte2 "$($m.Src)\tools\$($m.Host)"
  $dstLegado = Join-Path $legadoRoot "$($m.Src)\tools\$($m.Host)"
  $dstEngine = Join-Path $orqRoot "engines\$($m.Eng)\tools\$($m.Host)"

  Write-Host "=== $($m.Src) -> engines\$($m.Eng) + legado ===" -ForegroundColor Cyan
  $a = Copy-Tree $srcHost $dstLegado
  $b = Copy-Tree $srcHost $dstEngine

  # Tambem copia windowsservices se existir (referencia do DevHost)
  $wsName = Get-ChildItem -LiteralPath (Join-Path $cte2 $m.Src) -Directory -ErrorAction SilentlyContinue |
    Where-Object { $_.Name -like 'dfend-cte-*-windowsservices' } |
    Select-Object -First 1
  if ($wsName) {
    $srcWs = $wsName.FullName
    $dstWsLegado = Join-Path $legadoRoot "$($m.Src)\$($wsName.Name)"
    $dstWsEngine = Join-Path $orqRoot "engines\$($m.Eng)\$($wsName.Name)"
    Copy-Tree $srcWs $dstWsLegado | Out-Null
    Copy-Tree $srcWs $dstWsEngine | Out-Null
    Write-Host "  windowsservices: $($wsName.Name)"
  }

  $exe = Join-Path $dstEngine "bin\Debug\$($m.Host).exe"
  if (Test-Path -LiteralPath $exe) {
    Write-Host "  OK exe: $exe" -ForegroundColor Green
    $ok++
  } else {
    Write-Host "  AVISO: exe ainda ausente em engines (rode build-devhosts.ps1): $exe" -ForegroundColor Yellow
  }
}

Write-Host ""
Write-Host "DevHosts com exe em engines: $ok / $($map.Count)" -ForegroundColor $(if ($ok -eq $map.Count) { "Green" } else { "Yellow" })
Write-Host "Proximo: powershell -File .\tools\build-devhosts.ps1   (se algum exe faltou)"
Write-Host "Reinicie Orquestrador.Api e Ligar cadeia."
