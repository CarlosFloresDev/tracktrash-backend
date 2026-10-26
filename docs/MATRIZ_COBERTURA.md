# Cobertura verificable de TrackTrash · 24/10/2026

Fuente: RF-01–40 y RNF-01–26 del informe original, más la estructura del curso. La numeración conserva la del documento. **Implementado** significa disponible en el entorno local y probado al nivel indicado; no certifica un despliegue público. **Parcial** identifica explícitamente lo que falta. No se considera terminado el producto completo.

## Requisitos funcionales

| ID | Estado | Evidencia actual y alcance pendiente |
|---|---|---|
| RF-01 | Parcial | Login real Supabase probado en Flutter publicado; API Render valida JWT/JWKS. Registro, renovación prolongada, correo y recuperación completa con retorno/enlace profundo aún requieren validación. |
| RF-02 | Parcial | Vecino, reciclador y administrador con permisos en API; faltan roles municipal/institucional y ámbitos de organización. |
| RF-03 | Implementado | GET/PUT perfil, nombre, teléfono, referencia y coordenadas opcionales. |
| RF-04 | Parcial | Pausa global y selección independiente por categoría de avisos internos (horarios, recojos, incidencias y canjes), persistidas y aplicadas por servidor. Historial previo conservado; tipos/proveedor push aún pendientes. |
| RF-05 | Parcial | Registro API, distrito y ubicación espacial de contenedores; falta catálogo de tipos y su administración completa en UI. |
| RF-06 | Parcial | Registro de dispositivo y protocolo HTTP de lecturas con clave individual, pruebas PostgreSQL y simulador; falta ESP32 físico. |
| RF-07 | Implementado | Mediana de muestras, porcentaje y tres estados; además estado desconocido para lecturas antiguas. Pruebas de dominio/integración. |
| RF-08 | Parcial | Reporte ciudadano de nivel, límite de frecuencia y origen visible; fotografía opcional y Storage pendientes. |
| RF-09 | Parcial | Batería/contacto persistidos y antigüedad visible; faltan alarma de desconexión y supervisión de baterías. |
| RF-10 | Implementado | Reportar y consultar incidencias propias; operador atiende y resuelve con responsable e historial de cambios. |
| RF-11 | Parcial | Horarios semanales y zona/distrito; faltan empresas, camiones y geometría de rutas. |
| RF-12 | Parcial | Reportes ciudadanos de paso persistentes, identificados como no verificados; falta integración GPS de empresas. |
| RF-13 | Pendiente | ETA requiere posiciones y un modelo validado; el próximo horario programado no se presenta como predicción. |
| RF-14 | Parcial | Suscripción a ruta, antelación editable en API y Flutter (5–60 min, 20 predeterminado), reprogramación al cambiarla e idempotencia del reintento; falta punto geográfico suscrito. |
| RF-15 | Parcial | Reporte de paso con límite de frecuencia; todavía no alimenta un predictor ETA. |
| RF-16 | Implementado | Mapa/lista consultando API cada 30 s en primer plano, filtros, fuente, antigüedad y caché pública explícita. |
| RF-17 | Pendiente | No hay posiciones verificadas de camiones para mostrarlas. |
| RF-18 | Implementado | Cuatro materiales, guía básica y selección en formulario de recojo. |
| RF-19 | Implementado | Asignación opcional automática/manual al aliado verificado elegible más cercano, radio 20 km, material y capacidad disponible; consulta PostGIS. |
| RF-20 | Parcial | API de alta/verificación y catálogo con materiales/capacidad; falta autogestión completa por asociación y operación real de verificación. |
| RF-21 | Implementado | Listar, aceptar e iniciar viaje; capacidad reservada y control de versión. |
| RF-22 | Implementado | Código privado del vecino; solo reciclador asignado confirma. Confirmación y puntos atómicos e idempotentes. |
| RF-23 | Parcial | Un material y cantidad estimada por solicitud confirmada; falta detalle de múltiples materiales/pesaje recibido. |
| RF-24 | Pendiente | No hay ventanas de depósito ni comprobación combinada GPS/tiempo/tapa/sensor. Un reporte ciudadano no genera puntos. |
| RF-25 | Parcial | Tope de 200 puntos positivos diarios por entregas (día de Perú), control concurrente; falta limitar ventanas de depósito. |
| RF-26 | Parcial | 20 puntos por entrega confirmada, una sola vez; no hay depósitos validados. |
| RF-27 | Implementado | Racha actual y máxima de días con entregas confirmadas, zona UTC−5, sin incrementos por dos entregas el mismo día. |
| RF-28 | Implementado local | Catálogo, costo, stock, canje con código y control concurrente de saldo. Beneficios DEMO; faltan convenios y entrega operativa real. |
| RF-29 | Implementado | Libro de movimientos con origen de entrega/canje, positivos y negativos; consulta propia en app. |
| RF-30 | Parcial | Avisos dentro de la app por eventos de recojo/incidencia/canje y recordatorios; faltan proveedor push y condiciones GPS/deposición del requisito completo. |
| RF-31 | Pendiente | No se envía una inferencia de «basura no sacada» sin evidencias de depósito. |
| RF-32 | Parcial | Eventos internos deduplicados por clave única; entrega push aún no implementada. |
| RF-33 | Implementado | Bandeja propia, historial y marcado de lectura. |
| RF-34 | Parcial | Trabajo periódico con recordatorios persistidos y eventos deduplicados; falta outbox de proveedor push, resultado de entrega y política de reintentos externos. |
| RF-35 | Parcial | Flutter web adaptable, mapa/alertas y panel de operador; falta rol municipal y aislamiento por municipalidad. |
| RF-36 | Parcial | Abrir, tomar atención y resolver; responsable asignado al operador que toma el caso. Falta reasignar a otro personal municipal. |
| RF-37 | Parcial | Indicadores globales y cantidades claramente estimadas; faltan filtros por distrito, tiempo medio de atención y participación. |
| RF-38 | Implementado | Mismo cliente Flutter web para aceptar y confirmar como reciclador. |
| RF-39 | Pendiente | Instituciones, pertenencias, ámbitos, indicadores propios y exportaciones. |
| RF-40 | Pendiente | Empresas y priorización/optimización de rutas. |

## Requisitos no funcionales

| ID | Estado | Evidencia o validación necesaria |
|---|---|---|
| RNF-01 | Por medir | Geography/GiST y radio acotado disponibles. No se ha acreditado p95 <2 s con 1.000 contenedores; el endpoint limita a 500 contenedores por consulta. |
| RNF-02 | Pendiente | No existe disparador de cercanía de camión ni push medido <60 s. |
| RNF-03 | Por medir | Falta prueba de carga de 100 usuarios y presupuesto de recursos. |
| RNF-04 | Objetivo | No hay un mes de servicio operado; health checks no demuestran 99% mensual. |
| RNF-05 | Parcial | Reportes ciudadanos y estado sin datos recientes; probar fallos reales de hardware en piloto. |
| RNF-06 | Pendiente | La API deduplica secuencias reenviadas, pero no hay firmware con cola persistente local. |
| RNF-07 | Pendiente | Configurar respaldos/retención del host y verificar una restauración. |
| RNF-08 | Parcial | JWT asimétrico real verificado en Render y acceso Flutter publicado; token demo rechazado. Contraseñas en Auth, sin persistencia en texto en EF/Flutter. Correo/recuperación y auditoría adicional pendientes. |
| RNF-09 | Parcial | Claves IoT individuales con hash y comparación constante; producción exige HTTPS del host. Desarrollo usa HTTP local explícito. |
| RNF-10 | Parcial | Roles y propiedad de recursos comprobados en API; falta aislamiento municipal/institucional. |
| RNF-11 | Parcial | Permiso GPS solicitado, coordenadas opcionales, dirección privada y caché solo pública; faltan política de privacidad, consentimiento completo y revisión legal para operación. |
| RNF-12 | Parcial | Límites diarios, frecuencia, idempotencia y concurrencia; falta detección avanzada de ubicaciones/patrones de fraude. |
| RNF-13 | Parcial | Auditoría transaccional de actor/entidad/campos modificados sin copiar valores privados; falta política general de borrado lógico/retención. |
| RNF-14 | Implementado local | Interfaz en español y accesos directos; errores del proveedor Auth pueden conservar su texto original. |
| RNF-15 | Parcial | APK debug compilado; proyecto iOS configurado, sin compilar en Windows. Falta validar Android 8+, permisos y dispositivos físicos. |
| RNF-16 | Parcial | Último mapa público con advertencia de antigüedad. No hay mapa base offline, cola de escrituras ni caché privada de recojos. |
| RNF-17 | Por validar | Diseño adaptable, estados de carga/error/vacío, texto además de color y movimiento reducido; faltan sesiones de usabilidad y auditoría de accesibilidad. |
| RNF-18 | Implementado | ASP.NET Core/EF Core 10, PostgreSQL 17/PostGIS 3.5, repositorios LINQ y UnitOfWork. |
| RNF-19 | Implementado | Columnas calculadas geography(Point,4326), índices GiST y búsquedas espaciales reales. |
| RNF-20 | Parcial | API Docker sin root desplegada en Render Free, Supabase con siete migraciones/TLS/rol limitado y Flutter web en Pages. Falta restauración, carga, observabilidad operativa y continuidad; el plan gratuito duerme. |
| RNF-21 | Parcial | 53 pruebas backend y 24 Flutter; puntos, concurrencia, permisos, recordatorios, preferencias, migración de historial, hosting, sensores y canjes. Smoke remoto API y web aprobado; pruebas físicas y de requisitos pendientes no se sustituyen con ello. |
| RNF-22 | Pendiente | Lecturas persistidas sin particionado/purga; definir retención antes de crecimiento de piloto. |
| RNF-23 | Parcial | Modelo de distritos y consultas por datos; altas sin UI completa. Instituciones aún no modeladas. |
| RNF-24 | Pendiente físico | Autonomía de batería no medida. |
| RNF-25 | Pendiente físico | Carcasa y resistencia ambiental no ensayadas. |
| RNF-26 | Pendiente físico | La API recibe distancias; compensación térmica corresponde al sensor/firmware y falta implementarla/validarla. |

## Siguiente orden de trabajo

1. Conectar Supabase real en un entorno de pruebas: Auth, correo, claves asimétricas, permisos del esquema y migraciones revisadas; verificar registro/renovación/cierre/recuperación.
2. Cerrar requisitos académicos prioritarios con el docente: depósitos y fraude, detalle multimaterial, roles institucionales, camiones/ETA. Evitar agregar entidades sin proceso que las use.
3. Añadir Storage privado con validación de archivos y acceso temporal para fotos; proveedor push y outbox con reintentos observables.
4. Cargar aliados/rutas/ubicaciones verificados, probar un dispositivo, medir carga y restauración, completar privacidad y probar usabilidad antes de piloto.

Las funcionalidades pendientes no pueden darse por terminadas por disponer de un paquete, una clase vacía o un dato simulado.
