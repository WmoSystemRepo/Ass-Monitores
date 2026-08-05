# T02 — valida SqlResgateStore contra SQL DEV (Windows Auth)
$ErrorActionPreference = "Stop"
Set-Location "$PSScriptRoot\.."

Write-Host "=== T02: SqlResgateStore integration tests ===" -ForegroundColor Cyan
dotnet test tests/CTe.Resgate.Infrastructure.Tests/CTe.Resgate.Infrastructure.Tests.csproj --filter SqlResgateStore_crud_e_claim_pendente -v n

if ($LASTEXITCODE -ne 0) {
    Write-Host "FALHA T02" -ForegroundColor Red
    exit 1
}

Write-Host "T02 OK" -ForegroundColor Green
