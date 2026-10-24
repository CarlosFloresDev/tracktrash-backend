$ErrorActionPreference = 'Stop'
Set-Location (Split-Path $PSScriptRoot -Parent)
if (-not (Test-Path -LiteralPath '.env')) {
    $taskBytes = New-Object byte[] 24
    $taskRandom = [Security.Cryptography.RandomNumberGenerator]::Create()
    $taskRandom.GetBytes($taskBytes)
    $taskRandom.Dispose()
    $taskPassword = [BitConverter]::ToString($taskBytes).Replace('-', '')
    Set-Content -LiteralPath '.env' -Value "POSTGRES_PASSWORD=$taskPassword" -Encoding ascii
}
$taskLine = Get-Content -LiteralPath '.env' | Where-Object { $_ -match '^POSTGRES_PASSWORD=' } | Select-Object -First 1
if (-not $taskLine) { throw 'Falta POSTGRES_PASSWORD en .env.' }
$taskPassword = $taskLine.Substring('POSTGRES_PASSWORD='.Length)
$taskConnection = "Host=localhost;Port=55432;Database=tracktrash;Username=tracktrash;Password=$taskPassword"
dotnet user-secrets set 'ConnectionStrings:TrackTrash' $taskConnection --project src/TrackTrash.Api | Out-Null
if ($LASTEXITCODE -ne 0) { throw 'No se pudo guardar la conexión local.' }
docker compose up -d --wait
if ($LASTEXITCODE -ne 0) { throw 'No se pudo iniciar PostgreSQL.' }
dotnet run --project src/TrackTrash.Api -- --migrate
if ($LASTEXITCODE -ne 0) { throw 'No se pudieron aplicar migraciones.' }
Write-Host 'Base de datos lista. Inicia la API con dotnet run --project src/TrackTrash.Api'
