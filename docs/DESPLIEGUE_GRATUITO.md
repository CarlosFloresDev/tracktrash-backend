# Piloto remoto · 25/10/2026

## Recursos creados

- Organización nueva Supabase: TrackTrash (`vpoepddncxtccyvvwpqx`).
- Proyecto gratuito: `owfjolparskdiazhfubr`, región us-east-1. [Dashboard](https://supabase.com/dashboard/project/owfjolparskdiazhfubr).
- Siete migraciones EF aplicadas mediante SQL idempotente. PostGIS y esquema privado `tracktrash`; EF sigue siendo la autoridad del esquema.
- Tres identidades de prueba confirmadas en Auth, con roles en `app_metadata.tracktrash_role`; no se enviaron correos. Los datos del piloto son ficticios.
- Cloudflare Pages: proyecto `tracktrash`, dirección reservada `https://tracktrash.pages.dev`. Todavía sin publicación de Flutter.
- Render: sesión CLI y workspace conectados. No hay servicio creado: Render rechaza el repositorio privado como inaccesible y la validación no encuentra su rama. GitHub confirma que la rama existe.

## Acceso pendiente de Render

Revisar la [instalación de Render en GitHub](https://github.com/apps/render/installations/new) en **CarlosFloresDev** e incluir **tracktrash-backend** en Repository access. En Render, Account Settings → Account Security → Git Deployment Credentials debe estar conectada esa misma cuenta. Es una autorización entre servicios distinta del inicio de sesión CLI. [Guía oficial](https://render.com/docs/git-provider).

Después, validar `render.yaml` y crear el servicio free con la rama `feat/ecosistema-tracktrash`. No convertir el repositorio en público. No desplegar desde main: todavía contiene el primer incremento.

## Configuración del servidor

`render.yaml` describe un contenedor Docker, puerto 8080, Production y autenticación demo desactivada. `ConnectionStrings__TrackTrash` se introduce como secreto en Render, usando el rol limitado `tracktrash_api`, nunca el propietario postgres:

```text
Host=aws-0-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=tracktrash_api.owfjolparskdiazhfubr;Password=<secreto>;SSL Mode=VerifyFull;Root Certificate=/app/certs/supabase-root-2021.crt;Search Path=public,extensions;Maximum Pool Size=5;GSS Encryption Mode=Disable;Timeout=15
```

Se verificó desde una API local en Production: `/health/ready` respondió 200 contra Supabase usando ese rol y validación TLS completa; `/api/v1/perfil` respondió 200 con JWT real de Supabase. Esto no equivale a un despliegue de la API en Render ni a una prueba de Flutter remoto.

El rol runtime tiene acceso al esquema de la aplicación; `anon` y `authenticated` no tienen acceso directo a ese esquema. La API valida los JWT asimétricos del proyecto. `/health/ready` exige conexión y ausencia de migraciones pendientes. El arranque no migra ni precarga datos. Las operaciones de migración y `--seed-pilot` son explícitas; el seed exige `Pilot:Enabled` y tres UUID distintos de Auth.

Render termina TLS. Solo su marca de proceso `RENDER=true` y `RENDER_EXTERNAL_URL` HTTPS habilitan el esquema HTTPS interno; no se confía en cabeceras arbitrarias del cliente.

## Secretos y continuación

Credenciales locales ignoradas por Git: `.env.remote`, `artifacts/deployment/supabase-keys.json`, `artifacts/deployment/pilot-accounts.private.json`. No publicar esos archivos ni logs privados. El certificado público sí se versiona. `.dockerignore` excluye secretos, artifacts y metadatos de Supabase.

Con el servicio operativo: verificar readiness, JWT real, permisos y una operación persistida; compilar Flutter con su URL HTTPS real, `DEMO_MODE=false`, URL/clave publicable de Supabase y `API_TIMEOUT_SECONDS=90`. Publicar web en Pages; generar APK firmado por ABI y probar en un teléfono antes de compartir. No incluir claves service_role, SQL ni secretos en Flutter.

## Límites del piloto gratuito

[Render Free](https://render.com/docs/free) suspende el servicio inactivo; el primer acceso puede tardar y los recordatorios internos no tienen garantía de puntualidad mientras duerme. No implementar pings artificiales. Cloudflare Free sirve Flutter web; [Containers requiere Workers Paid](https://developers.cloudflare.com/containers/platform/pricing/) para alojar este backend sin reescritura.

Los APK pueden superar el [máximo de 25 MiB por archivo de Pages](https://developers.cloudflare.com/pages/platform/limits/); usar distribución separada, por ejemplo un bucket público dedicado de Storage exclusivamente para binarios. Todavía no hay APK remoto publicado ni cuenta de correo/SMTP, push, hardware, prueba de carga o respaldo restaurado verificados.

Validación local de este incremento: 53 pruebas backend aprobadas sobre PostgreSQL/PostGIS local, Release sin errores ni advertencias; 24 pruebas Flutter aprobadas y analyze sin incidencias.
