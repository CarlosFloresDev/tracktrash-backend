$ErrorActionPreference = 'Stop'
$taskRoot = Split-Path $PSScriptRoot -Parent
Set-Location $taskRoot
$env:ASPNETCORE_ENVIRONMENT = 'Development'
$taskOutput = Join-Path $taskRoot ('artifacts/scaffolding/' + (Get-Date -Format 'yyyyMMdd-HHmmss'))
dotnet ef dbcontext scaffold 'Name=ConnectionStrings:TrackTrash' Npgsql.EntityFrameworkCore.PostgreSQL --project src/TrackTrash.Infrastructure --startup-project src/TrackTrash.Api --schema tracktrash --context ScaffoldedTrackTrashDbContext --output-dir $taskOutput --context-dir $taskOutput --no-onconfiguring
if ($LASTEXITCODE -ne 0) { throw 'No se pudo generar el modelo de referencia.' }
Write-Host "Scaffolding generado en $taskOutput. No sustituye las entidades de Domain."
