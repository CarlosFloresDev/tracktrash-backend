# Repositorios y estado de TrackTrash

El desarrollo se separó en dos repositorios Git independientes dentro de esta carpeta de trabajo:

- [tracktrash-backend](https://github.com/CarlosFloresDev/tracktrash-backend): C# / ASP.NET Core 10, Domain, Application, Infrastructure y Api; PostgreSQL, migraciones y pruebas.
- [tracktrash-mobile](https://github.com/CarlosFloresDev/tracktrash-mobile): Flutter, navegación, interfaz, integración HTTP y pruebas.

Ambos son privados, bajo CarlosFloresDev. Los documentos originales de clase permanecen en la carpeta general; no se publicaron los archivos DOCX/PDF. La documentación técnica de backend y el contrato de API se versionan en su repositorio. Cada repositorio incluye su propio README de ejecución y CI.

La rama de trabajo inicial es `feat/primer-flujo-recojos`, con commits separados por base, implementación y validación/documentación. `main` contiene la base inicial hasta integrar el pull request.

Primer incremento: crear solicitud de recojo como vecino, aceptar como reciclador, confirmar con código y acreditar puntos una sola vez. Usa PostgreSQL local real y perfiles de prueba identificados como demostración. Supabase remoto, usuarios reales, mapa, IoT, fotos y push requieren los siguientes incrementos.

Validación local: 15 pruebas C# aprobadas (incluye PostgreSQL y lectura de JWKS), 10 pruebas Flutter aprobadas, análisis Flutter sin incidencias y backend Release sin advertencias. Compilaciones web y APK debug generadas; pendiente prueba del APK en dispositivo físico e iOS. Cada repositorio tiene CI independiente.

Pull requests del incremento: [backend #1](https://github.com/CarlosFloresDev/tracktrash-backend/pull/1) y [mobile #1](https://github.com/CarlosFloresDev/tracktrash-mobile/pull/1). Permanecen abiertos para revisión.

Skills incorporadas para este desarrollo: aspnet-core, flutter-apply-architecture-best-practices y flutter-add-widget-test. Estarán disponibles para descubrimiento automático en el siguiente turno; en este desarrollo se leyeron sus instrucciones locales.
