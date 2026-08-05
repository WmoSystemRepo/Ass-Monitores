# T03 — valida SqlDocumentoRepository contra SQL DEV (Windows Auth)
$ErrorActionPreference = "Stop"
Set-Location "$PSScriptRoot\.."

Write-Host "=== T03: SqlDocumentoRepository integration tests ===" -ForegroundColor Cyan
dotnet test tests/CTe.Resgate.Infrastructure.Tests/CTe.Resgate.Infrastructure.Tests.csproj --filter SqlDocumentoRepository_exists_e_insert_if_absent -v n

if ($LASTEXITCODE -ne 0) {
    Write-Host "FALHA T03" -ForegroundColor Red
    exit 1
}

Write-Host "T03 OK" -ForegroundColor Green
