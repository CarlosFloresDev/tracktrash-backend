# Estado de TrackTrash · 24/10/2026 · incremento 0.2

Repositorios públicos separados bajo CarlosFloresDev (visibilidad autorizada el 25/10/2026):

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

Pendientes principales: correo/SMTP, recuperación completa de contraseña, Storage/fotos, push/outbox, depósitos validados, detalle multimaterial, instituciones/municipalidades, camiones/GPS/ETA y priorización, retención de lecturas, respaldos restaurados, carga, pruebas físicas y revisión de privacidad. No hay garantía de cero fallos ni cumplimiento total sin esas validaciones.

Los datos precargados de aliados, horarios, ubicaciones y beneficios son sintéticos e identificados como demostración. El simulador HTTP no es firmware ESP32 y no demuestra autonomía o resistencia ambiental. iOS no se compiló en Windows; APK aún requiere instalación y prueba en dispositivo.

## Retomar

Continuación de avisos: preferencias por categoría y pausa global aplicadas en servidor; suscripciones con anticipación editable y actualizable sin duplicados. Migración `20261025033533_PreferenciasAvisos` aplicada en la BD local, conservando avisos y lecturas anteriores. El worker sigue usando avisos internos, sin proveedor push.

QA adicional: preferencias guardadas desde Flutter y suscripción a 35 minutos, verificadas contra API/PostgreSQL y restauradas al terminar. Script mobile/tools/smoke_preferences_web.py; capturas nuevas de perfil/antelación. La sesión Flutter encontrada en 5300 se conservó; la compilación revisada se sirve en http://localhost:5301 con API en 5080 y ese origen habilitado solo en el proceso de desarrollo.

Leer ambos README/AGENTS.md, consultar git status y continuar en las ramas actuales. Backend: Docker Desktop, `docker compose up -d --wait`, `dotnet run --project src/TrackTrash.Api`. App: `flutter run -d edge --web-port=5300 --dart-define=DEMO_MODE=true --dart-define=API_BASE_URL=http://localhost:5080`.

Skills aplicadas: aspnet-core, flutter-apply-architecture-best-practices, flutter-add-widget-test e imagegen. En este incremento se conectaron cuentas de Supabase, Cloudflare y Render; los DOCX/PDF originales siguen privados. Las instrucciones de continuación están en docs/CONTINUAR.md de la carpeta general.


## Piloto remoto · 25/10/2026

Organización nueva Supabase TrackTrash y proyecto `owfjolparskdiazhfubr` creados. Siete migraciones aplicadas, TLS VerifyFull con CA pública y rol SQL limitado verificados. Tres cuentas sintéticas confirmadas para el piloto; secretos locales ignorados por Git.

Web publicada: https://tracktrash.pages.dev. API Render Free: https://tracktrash-api.onrender.com/health/ready. El acceso al repositorio quedó resuelto al hacerlo público con permiso del propietario. Rama desplegada `feat/ecosistema-tracktrash`; main conserva el primer incremento y los PR #2 siguen abiertos. Detalles en `docs/DESPLIEGUE_GRATUITO.md` del backend.

APK Release 0.2.1+3 por ABI publicados en Storage público de descargas; firmas verificadas. Enlaces, SHA-256 y capturas del recorrido web en el repositorio Flutter, `docs/DISTRIBUCION_PILOTO.md`. La clave de firma y cuentas del piloto se conservan como archivos locales ignorados, sin publicarlas en repositorios.

Validación remota: JWT real para los tres roles; anónimo y token demo 401, vecino accediendo a indicadores admin 403, admin 200. Recojo sintético creado, consultado y cancelado; CORS y Location HTTPS correctos. Flutter publicado probado en Edge headless: login real, perfil, mapa y cierre, sin excepciones de navegador ni controles demo. Backend: 53 pruebas locales aprobadas, Release sin advertencias e imagen Docker 0.2.1 construida. Flutter: analyze y 24 pruebas aprobadas; firma propia generada. Instalación física y hardware siguen pendientes. Render Free duerme y no garantiza puntualidad de recordatorios internos.
