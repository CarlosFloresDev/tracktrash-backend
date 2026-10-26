# Estado de TrackTrash · 24/10/2026 · incremento 0.2

Repositorios privados separados bajo CarlosFloresDev:

- [Backend](https://github.com/CarlosFloresDev/tracktrash-backend), C# / ASP.NET Core 10: Domain, Application, Infrastructure y Api.
- [Mobile](https://github.com/CarlosFloresDev/tracktrash-mobile), Flutter para Android/iOS/web con navegación adaptable.

El primer incremento está integrado en main (PR #1 de cada repo). Ampliación actual en `feat/ecosistema-tracktrash`, con múltiples commits por requisitos, dominio, módulos, pruebas, diseño, autenticación, iconos, correcciones y documentación. PRs para revisión: [backend #2](https://github.com/CarlosFloresDev/tracktrash-backend/pull/2) y [mobile #2](https://github.com/CarlosFloresDev/tracktrash-mobile/pull/2).

## Alcance actual

20 entidades persistidas y 40 casos de uso concretos. Perfil, aliados/materiales/capacidad, asignación espacial, ciclo de recojo con historial/cancelación/reprogramación, mapa PostGIS, contenedores/reportes ciudadanos, ingesta IoT autenticada, incidencias, horarios y suscripciones, avisos internos, puntos/topes/rachas, catálogo/canjes y panel operativo. Auditoría transaccional y control de concurrencia/idempotencia.

Flutter: ilustración original Tacho Vivo, iconos, animaciones con movimiento reducido, cinco áreas de navegación, estados de carga/error/vacío, mapa/lista y caché pública opcional. SDK Supabase integrado para acceso y renovación; almacenamiento seguro de sesión y perfiles demo en memoria.

## Evidencia

- Backend: 53 pruebas aprobadas contra PostgreSQL/PostGIS, compilación Release sin advertencias; Dockerfile sin root.
- Flutter: 24 pruebas aprobadas y análisis sin incidencias; builds web y APK debug. QA de navegador con flujo real y capturas en mobile/docs/screenshots; instrucciones en su README.
- Cada repositorio ejecuta GitHub Actions. Consultar los checks del PR para el último commit, sin confundir validación local con producción.
- APK local: mobile/build/app/outputs/flutter-apk/app-debug.apk, para Android Emulator (`10.0.2.2:5080`). No se versiona. Recompilar con IP LAN para teléfono físico.

## Lo que falta

La [matriz de los 40 RF y 26 RNF](MATRIZ_COBERTURA.md) detalla cada estado; [arquitectura real](ARQUITECTURA_IMPLEMENTADA.md). Los documentos originales y los planes iniciales se conservan como antecedente, no como prueba de implementación.

Pendientes principales: despliegue de API/Flutter, correo/SMTP y validaci?n remota completa, recuperación completa de contraseña, Storage/fotos, push/outbox, depósitos validados, detalle multimaterial, instituciones/municipalidades, camiones/GPS/ETA y priorización, retención de lecturas, respaldos restaurados, carga, pruebas físicas y revisión de privacidad. No hay garantía de cero fallos ni cumplimiento total sin esas validaciones.

Los datos precargados de aliados, horarios, ubicaciones y beneficios son sintéticos e identificados como demostración. El simulador HTTP no es firmware ESP32 y no demuestra autonomía o resistencia ambiental. iOS no se compiló en Windows; APK aún requiere instalación y prueba en dispositivo.

## Retomar

Continuación de avisos: preferencias por categoría y pausa global aplicadas en servidor; suscripciones con anticipación editable y actualizable sin duplicados. Migración `20261025033533_PreferenciasAvisos` aplicada en la BD local, conservando avisos y lecturas anteriores. El worker sigue usando avisos internos, sin proveedor push.

QA adicional: preferencias guardadas desde Flutter y suscripción a 35 minutos, verificadas contra API/PostgreSQL y restauradas al terminar. Script mobile/tools/smoke_preferences_web.py; capturas nuevas de perfil/antelación. La sesión Flutter encontrada en 5300 se conservó; la compilación revisada se sirve en http://localhost:5301 con API en 5080 y ese origen habilitado solo en el proceso de desarrollo.

Leer ambos README/AGENTS.md, consultar git status y continuar en las ramas actuales. Backend: Docker Desktop, `docker compose up -d --wait`, `dotnet run --project src/TrackTrash.Api`. App: `flutter run -d edge --web-port=5300 --dart-define=DEMO_MODE=true --dart-define=API_BASE_URL=http://localhost:5080`.

Skills aplicadas: aspnet-core, flutter-apply-architecture-best-practices, flutter-add-widget-test e imagegen. En este incremento se conectaron cuentas de Supabase, Cloudflare y Render; los DOCX/PDF originales siguen privados. Las instrucciones de continuación están en docs/CONTINUAR.md de la carpeta general.


## Piloto remoto ? 25/10/2026

Organizaci?n nueva Supabase TrackTrash y proyecto `owfjolparskdiazhfubr` creados. Siete migraciones aplicadas, TLS VerifyFull con CA p?blica y rol SQL limitado verificados; login real Supabase y perfil HTTP 200 probados desde API local en Production. Tres cuentas sint?ticas de prueba, secretos locales ignorados. Esto no completa la validaci?n remota de Flutter.

Cloudflare Pages `tracktrash` reservado, todav?a sin publicar. Render CLI autenticado, pero Render rechaza el repositorio privado y no encuentra su rama aunque GitHub la confirma. Falta revisar la instalaci?n GitHub de Render para CarlosFloresDev/tracktrash-backend. Todav?a sin servicio/API p?blica ni APK remoto. Documentaci?n detallada en `tracktrash-backend/docs/DESPLIEGUE_GRATUITO.md` (desde el backend, `docs/DESPLIEGUE_GRATUITO.md`).

Backend: 53 pruebas locales aprobadas, Release sin advertencias e imagen Docker 0.2.1 construida. Flutter: analyze y 24 pruebas aprobadas; firma propia generada y configuraci?n release preparada, sin distribuir una compilaci?n con servidor inexistente. Free duerme en Render; no garantiza puntualidad de recordatorios internos. Hardware, push y otras brechas de la matriz siguen pendientes.
