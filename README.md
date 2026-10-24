# TrackTrash Backend

API ASP.NET Core 10 del proyecto TrackTrash. Cuatro proyectos: Domain, Application, Infrastructure y Api. App móvil independiente: [tracktrash-mobile](https://github.com/CarlosFloresDev/tracktrash-mobile).

## Primer incremento disponible

- Crear y consultar solicitudes de recojo con material, cantidad estimada, dirección y fecha.
- Aceptar solicitudes como reciclador y confirmar entregas con un código de seis dígitos.
- Acreditar 20 puntos por entrega confirmada en la misma transacción.
- Evitar duplicados por reintentos y conflictos de aceptación mediante claves únicas y control de versión.
- Proteger cada operación por identidad/rol y ocultar dirección/código a terceros.
- Persistencia PostgreSQL, repositorios con LINQ, IUnitOfWork, middleware de errores, OpenAPI y migraciones EF.

Se inicia con usuarios de demostración locales. Registro real, verificación de recicladores, asignación por zona/material, organizaciones, cancelación, fotos, mapa, sensores y push siguen pendientes. No usar esta versión como piloto público.

## Ejecutar en Windows

Requisitos: SDK .NET fijado en `global.json` y Docker Desktop con motor Linux activo.

```powershell
dotnet tool restore
powershell -NoProfile -ExecutionPolicy Bypass -File scripts/dev-setup.ps1
dotnet run --project src/TrackTrash.Api
```

El script genera una contraseña local en `.env` (ignorado por Git), guarda la conexión mediante user-secrets, inicia PostgreSQL/PostGIS en `localhost:55432` y aplica migraciones. No borra datos existentes.

API: `http://localhost:5080`. OpenAPI de desarrollo: `/openapi/v1.json`. Salud: `/health/live` y `/health/ready`.

En macOS/Linux: configurar la variable `ConnectionStrings__TrackTrash` y `.env` para Compose; ejecutar `docker compose up -d --wait`, `dotnet run --project src/TrackTrash.Api -- --migrate` y después `dotnet run --project src/TrackTrash.Api`. Definir `ASPNETCORE_ENVIRONMENT=Development` para el modo local.

## Probar

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File scripts/test.ps1
```

Las pruebas de integración crean y eliminan bases temporales `tracktrash_test_<guid>`. La conexión de pruebas necesita permiso de crear bases y debe apuntar exclusivamente a un PostgreSQL de desarrollo/CI. No usar Supabase de producción para las pruebas.

También se puede definir `TRACKTRASH_TEST_CONNECTION` y ejecutar `dotnet test`. CI proporciona una instancia efímera independiente. Hay pruebas de dominio, permisos HTTP, privacidad, reintentos, doble aceptación, rollback de confirmación/puntos y referencias entre capas.

## Usuarios locales

Solo en `Development` y con `Authentication:Demo=true`, usar `Authorization: Bearer demo-vecino` o `Bearer demo-reciclador`. Hay identidades secundarias `demo-vecino-2` y `demo-reciclador-2` para comprobar aislamiento. Son credenciales públicas de prueba, sin datos reales. El servidor impide activar este esquema fuera de Development.

La app Flutter usa esos perfiles con botones identificados como demostración. Para teléfono físico, exponer deliberadamente el servidor en la red de desarrollo con `dotnet run --project src/TrackTrash.Api --urls http://0.0.0.0:5080`; utilizar la IP LAN en Flutter y habilitar únicamente el acceso local necesario.

## Supabase

La conexión PostgreSQL y validación de JWT asimétricos están preparadas; no se ha conectado ningún proyecto remoto. Configurar secretos y variables desde el host, nunca desde Flutter:

```text
ConnectionStrings__TrackTrash=<conexión PostgreSQL de tu proyecto>
Authentication__Demo=false
Authentication__Authority=https://<referencia>.supabase.co/auth/v1
```

La API valida issuer, audience `authenticated`, vigencia y firma usando JWKS de Supabase. Requiere claves asimétricas publicadas por Auth; no incluye compatibilidad con el secreto compartido JWT legado. El rol reciclador se obtiene de `app_metadata.tracktrash_role`, gestionado por el servidor; no de `user_metadata`. Un usuario autenticado sin ese rol se trata como vecino. Falta validar esta integración contra el proyecto real y completar el acceso Flutter.

Las tablas de negocio viven en el esquema `tracktrash`; mantenerlo fuera de los esquemas expuestos por Data API y utilizar una cuenta SQL con privilegios mínimos. La autorización de la API no se transfiere automáticamente a políticas RLS. Auth y Storage son servicios distintos de la conexión EF; el adapter de fotos se desarrollará en otro incremento.

Aplicar migraciones explícitamente con un rol autorizado y acceso controlado. Revisar el SQL antes de producción:

```powershell
dotnet ef migrations script --idempotent --context TrackTrashDbContext --project src/TrackTrash.Infrastructure --startup-project src/TrackTrash.Api --output artifacts/migrations.sql
```

## Organización

| Proyecto | Contenido |
|---|---|
| Domain | Entidades, reglas, contratos de repositorios e IUnitOfWork |
| Application | DTOs, interfaces de servicios y casos de uso concretos con ExecuteAsync |
| Infrastructure | DbContext, repositorios EF/LINQ, UnitOfWork, identidad, DI y middleware |
| Api | Controllers, Program.cs y configuración del host |

[Arquitectura conceptual](docs/ARQUITECTURA_TRACKTRASH.md), [plan](docs/PLAN_TRACKTRASH.md), [contrato HTTP](docs/API.md) y [estado/backlog](docs/ESTADO.md).

Para scaffolding se ofrece `scripts/scaffold.ps1`: genera una copia de referencia en `artifacts/scaffolding` sin modificar Domain ni la persistencia existente. La autoridad actual del esquema son las migraciones EF. No mezclar cambios manuales de tablas con migraciones sin registrarlos.

## Trabajo con Git

`main` mantiene la base integrada; cada incremento se trabaja en `feat/<tarea>` y se presenta mediante pull request. Los commits se crean por cambios verificables. GitHub Actions compila y ejecuta pruebas contra PostgreSQL real.
