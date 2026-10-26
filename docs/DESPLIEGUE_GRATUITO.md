# Piloto remoto · 25/10/2026

## Recursos creados

- Organización nueva Supabase: TrackTrash (`vpoepddncxtccyvvwpqx`).
- Proyecto gratuito: `owfjolparskdiazhfubr`, región us-east-1. [Dashboard](https://supabase.com/dashboard/project/owfjolparskdiazhfubr).
- Siete migraciones EF aplicadas mediante SQL idempotente. PostGIS y esquema privado `tracktrash`; EF sigue siendo la autoridad del esquema.
- Tres identidades de prueba confirmadas en Auth, con roles en `app_metadata.tracktrash_role`; no se enviaron correos. Los datos del piloto son ficticios.
- Web Flutter publicada en [tracktrash.pages.dev](https://tracktrash.pages.dev), con Supabase real y modo demo desactivado.
- API publicada en [tracktrash-api.onrender.com](https://tracktrash-api.onrender.com/health/ready), servicio Render Free `srv-dbfbleu7bikc73ctg55g`, región Virginia. Primer despliegue `dep-dbfblfe7bikc73ctg6pg`, commit `96faae9`.

## Repositorios y actualizaciones

Ambos repositorios se hicieron públicos con autorización del propietario para compartirlos con compañeros. Antes se revisaron los historiales con Gitleaks 8.30.1: 27 commits backend y 21 Flutter, sin detecciones. Las credenciales remotas, firmas Android y archivos originales del curso permanecen fuera de Git.

Render pudo acceder después del cambio de visibilidad; `render.yaml` validado. La rama desplegada es `feat/ecosistema-tracktrash`; main todavía contiene el primer incremento. Los PR #2 siguen abiertos. Autodespliegue desactivado: ejecutar despliegue explícito después de revisar CI y migraciones. No modificar el plan gratuito.

## Configuración del servidor

`render.yaml` describe un contenedor Docker, puerto 8080, Production y autenticación demo desactivada. `ConnectionStrings__TrackTrash` se introduce como secreto en Render, usando el rol limitado `tracktrash_api`, nunca el propietario postgres:

```text
Host=aws-0-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=tracktrash_api.owfjolparskdiazhfubr;Password=<secreto>;SSL Mode=VerifyFull;Root Certificate=/app/certs/supabase-root-2021.crt;Search Path=public,extensions;Maximum Pool Size=5;GSS Encryption Mode=Disable;Timeout=15
```

Se verificó desde API local y después desde Render: readiness 200 con rol SQL limitado y TLS completo, perfil 200 con JWT real para vecino/reciclador/admin, anónimo y token demo 401, vecino accediendo a indicadores admin 403 y admin 200. Preflight CORS 204 para Pages. Recojo sintético creado (201), consultado y cancelado; Location HTTPS correcto. El recorrido en Edge headless contra Pages verificó login real, perfil, mapa y cierre de sesión, sin excepciones de navegador ni controles demo.

El rol runtime tiene acceso al esquema de la aplicación; `anon` y `authenticated` no tienen acceso directo a ese esquema. La API valida los JWT asimétricos del proyecto. `/health/ready` exige conexión y ausencia de migraciones pendientes. El arranque no migra ni precarga datos. Las operaciones de migración y `--seed-pilot` son explícitas; el seed exige `Pilot:Enabled` y tres UUID distintos de Auth.

Render termina TLS. Solo su marca de proceso `RENDER=true` y `RENDER_EXTERNAL_URL` HTTPS habilitan el esquema HTTPS interno; no se confía en cabeceras arbitrarias del cliente.

## Secretos y continuación

Credenciales locales ignoradas por Git: `.env.remote`, `artifacts/deployment/supabase-keys.json`, `artifacts/deployment/pilot-accounts.private.json`. No publicar esos archivos ni logs privados. El certificado público sí se versiona. `.dockerignore` excluye secretos, artifacts y metadatos de Supabase.

Flutter usa `config/pilot.public.json` de su repositorio: API HTTPS real, `DEMO_MODE=false`, URL/clave publicable de Supabase y `API_TIMEOUT_SECONDS=90`. La clave publicable identifica el proyecto; no concede acceso administrativo. No incluir claves service_role, SQL ni secretos en Flutter. Se creó un bucket público `downloads`, restringido a APK de hasta 50 MiB, sin permisos de escritura para clientes; no es Storage de fotos ciudadanas.

Se publicaron tres APK Release firmados 0.2.1+3 (arm64-v8a, armeabi-v7a y x86_64) en ese bucket; firmas verificadas y enlaces públicos comprobados. [Descargas y hashes en el repositorio Flutter](https://github.com/CarlosFloresDev/tracktrash-mobile/blob/feat/ecosistema-tracktrash/docs/DISTRIBUCION_PILOTO.md). Todavía falta instalación física. En Flutter, Gitleaks identifica la clave `sb_publishable_` como clave genérica: `.gitleaks.toml` permite exclusivamente ese tipo público en `config/pilot.public.json`, manteniendo las demás comprobaciones activas.

## Límites del piloto gratuito

[Render Free](https://render.com/docs/free) suspende el servicio inactivo; el primer acceso puede tardar y los recordatorios internos no tienen garantía de puntualidad mientras duerme. No implementar pings artificiales. Cloudflare Free sirve Flutter web; [Containers requiere Workers Paid](https://developers.cloudflare.com/containers/platform/pricing/) para alojar este backend sin reescritura.

Los APK pueden superar el [máximo de 25 MiB por archivo de Pages](https://developers.cloudflare.com/pages/platform/limits/); se distribuyen aparte mediante Storage. Correo/SMTP, recuperación completa, push, hardware, instalación física, prueba de carga y respaldo restaurado siguen pendientes. Las cuentas confirmadas del piloto permiten probar sin correos; no se certifica el registro público completo.

Validación local de este incremento: 53 pruebas backend aprobadas sobre PostgreSQL/PostGIS local, Release sin errores ni advertencias; 24 pruebas Flutter aprobadas y analyze sin incidencias.
