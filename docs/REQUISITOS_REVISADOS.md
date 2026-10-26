# Requisitos revisados y trazabilidad de TrackTrash

Revisión del 24/10/2026 basada en los tres documentos originales y en la arquitectura del curso. El tamaño se justifica por procesos completos y entidades con responsabilidad propia. Esta especificación no declara implementado lo que sigue pendiente de cuentas o validación física.

## Procesos del producto

| Proceso | Entidades | Casos de uso y reglas de aceptación |
|---|---|---|
| Identidad y perfil | PerfilUsuario | Obtener/actualizar nombre, teléfono y ubicación de referencia; identidad validada por el proveedor; un vecino no puede autoasignarse rol de operador |
| Aliados | Distrito, AsociacionReciclaje, MaterialAceptado, PuntoAcopio | Consultar aliados y materiales, verificar asociación como administrador, seleccionar candidatos por material, zona y capacidad |
| Recojo | SolicitudRecojo, HistoricoSolicitud | Crear, asignar candidato, aceptar, iniciar viaje, reprogramar, cancelar y consultar historial; transiciones controladas, privacidad e idempotencia |
| Entrega e impacto | EntregaDetalle, MovimientoPuntos | Confirmar con código, diferenciar cantidad estimada de recibida, otorgar recompensa una sola vez en la misma transacción |
| Contenedores | Contenedor | Consultar mapa/lista por radio, calcular estado y antigüedad; desconocido no equivale a vacío |
| IoT | DispositivoIoT, LecturaNivel | Registrar dispositivo, autenticar clave individual, ingerir secuencias sin duplicados; usar mediana y no sustituir datos actuales con lecturas antiguas |
| Atención ciudadana | Incidencia | Reportar, iniciar atención y resolver con evidencia textual; trazabilidad del responsable; solo operador cierra incidencias |
| Comunicación | Notificacion | Registrar avisos por evento, listar propios, marcar leída; clave única evita duplicados; push requiere proveedor externo |
| Beneficios | Beneficio, Canje | Consultar catálogo, canjear con saldo suficiente, controlar stock concurrente y registrar puntos negativos |
| Operación | PerfilUsuario, Distrito e indicadores agregados | Panel con saturación, incidencias, recojos y cantidades confirmadas; no atribuir CO2 sin metodología |

## Reglas transversales

- Toda escritura identifica actor y fecha. Permisos comprobados en servidor; direcciones y códigos no se publican en listados generales.
- Cambios de estado concurrentes usan una versión. Confirmación y canje no pueden generar saldos duplicados o negativos.
- Entidades y repositorios/IUnitOfWork en Domain; DTOs y casos de uso individuales en Application; EF/LINQ, adapters, middleware y DI en Infrastructure; controladores en Api.
- Coordenadas validadas, búsquedas por radio acotado y paginación. Índices espaciales para los puntos del mapa.
- Los datos de demostración son sintéticos y se identifican en la app. No representan aliados ni convenios reales.
- Modo offline informa antigüedad y conserva datos propios solo dentro de la sesión/usuario correspondiente; nunca confirma una entrega o canje sin servidor.
- Animaciones respetan la preferencia de movimiento reducido. Estados accesibles mediante texto/icono además del color.

## Relación con requisitos originales

| Grupo original | Cobertura de desarrollo prevista | Dependencia externa / validación |
|---|---|---|
| RF-01–04 | Acceso, perfil, preferencias | Proyecto Supabase Auth, correo y configuración de retorno |
| RF-05–10, 16 | Mapa, contenedores, lecturas e incidencias | Hardware real y prueba de sensores; fotografías requieren almacenamiento configurado |
| RF-11–15, 17 | Módulo municipal de rutas y horarios | Datos de empresa/municipio; ETA confiable no se infiere de puntos ficticios |
| RF-18–23 | Catálogo, candidatos, recojo y confirmación | Verificación operativa de aliados y pesaje real |
| RF-24–29 | Libro de puntos, límites y canjes | Depósitos individuales y rachas requieren reglas de evidencia; cercanía GPS no prueba un depósito |
| RF-30–34 | Bandeja y eventos deduplicados | Push real exige Firebase/APNs y pruebas en dispositivo |
| RF-35–40 | Panel y reportes; instituciones/rutas por ampliar | Convenios, ámbitos institucionales y datos logísticos |

## Criterios para considerar un incremento terminado

1. Caso de uso y permisos implementados con persistencia real.
2. Interfaz de carga, vacío, error, éxito y acceso denegado.
3. Prueba de reglas relevantes y de integración con PostgreSQL cuando corresponda.
4. Contrato y limitaciones actualizados; compilación y CI aprobados.
5. Commit por cambio lógico, sin dividir artificialmente archivos para aumentar el conteo.

## Elementos que no deben declararse completados sin evidencia

- Supabase remoto y autenticación de usuarios reales sin probar el proyecto configurado.
- Autonomía de batería, resistencia ambiental, precisión o compensación térmica del sensor.
- ETA de camiones, optimización logística e impacto ambiental medido sin fuentes verificadas.
- Entrega push al teléfono, respaldos restaurados, disponibilidad mensual y pruebas de carga sin medición.
- Integración multiinstitución, facturación, pagos y comisiones mientras no exista su implementación y operación.
