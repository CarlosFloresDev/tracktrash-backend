# Cómo contribuir a TrackTrash

## Ramas

- `main` está protegida: **nadie hace push directo**. Todo entra por Pull Request revisado por Carlos.
- Una rama por paquete: `feat/integrante-<n>-<tema>`. Las ramas cortas de apoyo usan `fix/…`, `chore/…` y `docs/…`.
- Mantén tu rama al día con `git fetch origin` y `git rebase origin/main`. No hagas merge de `main` dentro de tu rama.
- Nunca uses `push --force` sobre `main`. En tu propia rama, usa solo `--force-with-lease` y únicamente después de un rebase.

## Commits

- Usa Conventional Commits en español, por ejemplo `feat(rutas): suscribir vecino a una ruta`. Otros tipos: `fix`, `test`, `docs`, `refactor` y `chore`.
- Cada commit debe compilar.

## Pull Requests

- Un PR por paquete o subtema, de tamaño revisable (idealmente menos de 600 líneas, sin contar migraciones).
- Antes de abrirlo:
  - `dotnet build` sin warnings;
  - `dotnet test --filter "Categoria!=Integracion"` en verde;
  - las pruebas de integración en verde si tienes PostgreSQL (`scripts/test.ps1`).
- **Una sola migración por PR**, generada después de hacer rebase sobre `main`. Si el snapshot entra en conflicto, borra tu migración, haz rebase y vuelve a generarla.
- Enlaza el issue con `Closes #N` y completa la checklist de la plantilla.

## Arquitectura

Sigue `docs/REVISION_ARQUITECTURA.md`, sección 3:

- `Application/UseCases/<Entidad>/Commands/<Accion>/{Command, Handler, Validator}`;
- `Application/UseCases/<Entidad>/Queries/<Consulta>/{Query, Handler, Dto}`;
- la API solo traduce HTTP a Commands y Queries.

El reparto del trabajo está en `docs/REPARTO_DE_TAREAS.md`.
