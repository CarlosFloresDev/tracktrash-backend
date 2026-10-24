$ErrorActionPreference = 'Stop'
Set-Location (Split-Path $PSScriptRoot -Parent)
if (-not $env:TRACKTRASH_TEST_CONNECTION) {
    $taskLine = Get-Content -LiteralPath '.env' | Where-Object { $_ -match '^POSTGRES_PASSWORD=' } | Select-Object -First 1
    $taskPassword = $taskLine.Substring('POSTGRES_PASSWORD='.Length)
    $env:TRACKTRASH_TEST_CONNECTION = "Host=localhost;Port=55432;Database=postgres;Username=tracktrash;Password=$taskPassword"
}
dotnet test --nologo -v minimal
if ($LASTEXITCODE -ne 0) { throw 'Hay pruebas fallidas.' }
