# Contrato HTTP v1: TrackTrash 0.2

Fuente ejecutable del contrato: `/openapi/v1.json` en Development. La tabla de ampliación al final documenta los módulos actuales.

Todos los endpoints `/api/v1` requieren `Authorization: Bearer <token>`. JSON camelCase, fechas ISO 8601 con zona horaria y cantidades en kg estimados (hasta dos decimales). El servidor guarda fechas UTC; la app las presenta en la zona local del teléfono.

| Método y ruta | Permiso | Resultado |
|---|---|---|
| GET /api/v1/perfil | Autenticado | id y rol efectivos |
| GET /api/v1/perfil/impacto | Autenticado | puntos propios |
| GET /api/v1/solicitudes?pagina=1 | Autenticado | Hasta 50; vecino ve propias, reciclador disponibles y asignadas |
| GET /api/v1/solicitudes/{id} | Actor autorizado | Solicitud o 404 |
| POST /api/v1/solicitudes | Vecino | Solicitud creada y Location |
| POST /api/v1/solicitudes/{id}/aceptacion | Reciclador | Solicitud aceptada |
| POST /api/v1/solicitudes/{id}/confirmacion | Reciclador asignado | Entrega confirmada y puntos acreditados |

Crear:

```json
{
  "claveSolicitud": "6714608f-6fbc-4b55-9394-c078df13983f",
  "material": "PapelCarton",
  "cantidadEstimadaKg": 2.5,
  "direccion": "Av. Arequipa 123, referencia de prueba",
  "programadaPara": "2026-10-25T10:00:00-05:00"
}
```

La fecha del ejemplo debe reemplazarse por una futura dentro de 30 días. Materiales: `PapelCarton`, `Plastico`, `Vidrio`, `Metal`. Estados: `Pendiente`, `Aceptada`, `EnCamino`, `Confirmada`, `Cancelada`. Crear admite además latitud/longitud opcionales y `autoAsignar`; con true exige ubicación y un aliado elegible a menos de 20 km.

La claveSolicitud es un UUID generado por el cliente. Repetirla con el mismo cuerpo devuelve la solicitud existente; cambiar los datos requiere otra clave. Si dos operaciones compiten y una recibe 409, repetir la operación original después de actualizar permite recuperar el resultado existente.

Respuesta:

```json
{
  "id": "4da7e59c-af39-4eae-996c-23ce0ed52db4",
  "material": "PapelCarton",
  "cantidadEstimadaKg": 2.5,
  "direccion": "Av. Arequipa 123, referencia de prueba",
  "programadaPara": "2026-10-25T15:00:00+00:00",
  "estado": "Pendiente",
  "codigoConfirmacion": "123456",
  "creadaEn": "2026-10-23T20:00:00+00:00"
}
```

El código se devuelve solo al vecino propietario. La dirección se devuelve al propietario y al reciclador asignado; los demás recicladores elegibles reciben ambos campos como null. La búsqueda de candidatos aplica material, verificación, distancia y capacidad disponible. Los aliados precargados son sintéticos.

Confirmar: `{"codigo":"123456"}`. El backend verifica asignación/código; confirmar otra vez no suma puntos. La aceptación es idempotente para el mismo reciclador. La confirmación y el movimiento se guardan en una transacción PostgreSQL.

Errores: 400 datos inválidos, 401 sesión ausente/inválida, 403 operación no permitida, 404 recurso inaccesible/inexistente, 409 conflicto concurrente, 422 regla de negocio, 429 límite de intentos y 500 fallo interno. Errores controlados usan ProblemDetails con title/status/traceId; el cliente también admite 401/403/429 sin cuerpo. Límites por actor: 120 GET y 30 escrituras por minuto, independientes. En lecturas IoT sin identidad se usa IP como partición del limitador y clave individual para autorización del dispositivo.

## Ampliación disponible

Prefijo común `/api/v1`. Todos requieren sesión salvo ingesta IoT, que requiere `X-Device-Key`. Cambios de estado se realizan por POST y validan propiedad/rol en el servidor.

| Método/ruta | Permiso y contrato |
|---|---|
| GET/PUT /perfil | Perfil propio. PUT: nombre, telefono, direccion, latitud, longitud, recibirAvisos; no acepta cambios de rol/racha desde cliente. |
| GET /perfil/impacto, /perfil/movimientos | Saldo y libro de puntos propios. |
| POST /solicitudes/{id}/viaje | Reciclador asignado, pasa a EnCamino. |
| POST /solicitudes/{id}/cancelacion | Propietario/reciclador asignado, `{motivo}`; libera capacidad reservada. |
| POST /solicitudes/{id}/reprogramacion | Vecino propietario, pendiente, `{programadaPara}` futuro <=30 días. |
| POST /solicitudes/{id}/asignacion | Vecino propietario, pendiente; aliado elegible por coordenadas/material/capacidad. |
| GET /solicitudes/{id}/historial | Actores autorizados; acciones y fechas sin filtrar códigos/direcciones a terceros. |
| GET /mapa?lat=-16.3988&lng=-71.5369&radioKm=5 | Contenedores/aliados/acopios cercanos; radio mayor a 0 y hasta 50 km, topes de respuesta. |
| GET /asociaciones, /distritos | Catálogos. |
| GET /asociaciones/candidatos?material=PapelCarton&kg=2&lat=-16.3988&lng=-71.5369 | Aliados verificados/material/capacidad, hasta 20 km, distancia ascendente. |
| POST /asociaciones | Admin: distritoId,nombre,direccion,latitud,longitud,capacidadKg,materiales. |
| POST /asociaciones/{id}/verificacion | Admin; verificar aliado. |
| POST /contenedores | Admin: distritoId,nombre,latitud,longitud. |
| POST /contenedores/{id}/reporte-nivel | Sesión: `{porcentaje}` estimado; mínimo 5 min entre reportes propios. No da puntos ni sustituye sensor reciente. |
| GET/POST /incidencias | GET propias/admin todas; POST contenedorId,tipo,descripcion. |
| POST /incidencias/{id}/atencion | Admin toma responsabilidad. |
| POST /incidencias/{id}/resolucion | Admin responsable: `{resolucion}`. |
| GET /notificaciones | Bandeja propia. |
| POST /notificaciones/{id}/lectura | Propietario; idempotente. |
| GET/POST /beneficios | Lectura autenticada; alta admin: nombre,descripcion,costoPuntos,stock. |
| GET/POST /canjes | Propios; POST vecino `{beneficioId,clave}` UUID. Reintento de la misma clave no repite descuento ni consumo de stock. |
| GET /operacion/indicadores | Admin; agregados globales, kg estimados, no impacto ambiental certificado. |
| GET /rutas | Horarios semanales/próximo paso programado y suscripción propia. |
| POST/DELETE /rutas/{id}/suscripcion | POST `{minutosAntes}` entre 5 y 60; crea o actualiza la propia sin duplicar. GET /rutas devuelve minutosAntes propio o null; DELETE cancela. UI permite elegir y guardar cambios. |
| POST /rutas/{id}/paso | Vecino: reporte no verificado; límite de frecuencia 30 min. |
| POST /iot/dispositivos | Admin: `{contenedorId,alturaCm}`; retorna id y clave una sola vez. |
| POST /iot/dispositivos/{id}/lecturas | Clave individual; `{secuencia,distanciasCm,bateria,medidaEn}`. Mediana de 3–11 muestras; secuencia única por dispositivo. |

La confirmación acredita 20 puntos hasta el máximo diario de 200 positivos (UTC−5); si se alcanzó el tope, confirma la entrega sin sumar más. Rachas usan días de entregas confirmadas. Canje registra movimiento negativo y valida saldo, stock y versión en la misma transacción. Ante 409 actualizar y reintentar con la misma clave de intención, sin crear una compra nueva.

No se implementan pagos, fotos, depósitos validados, push externo ni posiciones/ETA de camión. Consultar [matriz](MATRIZ_COBERTURA.md) para el alcance por requisito.

## Preferencias de avisos internos

GET /perfil incluye `preferenciasAvisos`; PUT /perfil acepta ese objeto opcional junto a los campos ya existentes:

```json
{
  "nombre": "Vecino de prueba",
  "telefono": "",
  "direccion": "Referencia de prueba",
  "latitud": null,
  "longitud": null,
  "recibirAvisos": true,
  "preferenciasAvisos": {
    "horarios": true,
    "recojos": false,
    "incidencias": true,
    "recompensas": true
  }
}
```

`recibirAvisos=false` pausa **todos los avisos nuevos**, conservando la selección por categoría. Omitir `preferenciasAvisos` conserva las preferencias guardadas, por compatibilidad con el cliente 0.2 anterior. Campos omitidos dentro de un objeto enviado valen true; enviar el objeto completo para editarlo.

El servidor aplica estas opciones antes de añadir cada aviso a la misma transacción de negocio. No oculta ni borra historial, no cancela suscripciones y no afecta confirmaciones, puntos, canjes o incidencias. No se generan retroactivamente avisos silenciados. GET /notificaciones incorpora `tipo`: Sistema, Horarios, Recojos, Incidencias o Recompensas. La migración clasifica avisos anteriores por su clave y conserva su estado de lectura.

Cambiar minutosAntes de una suscripción existente recalcula el próximo aviso futuro; repetir el mismo valor no cambia fecha ni versión. Todavía no hay entrega push ni cálculo de llegada GPS.

Desplegar la migración y la API nuevas antes del cliente Flutter actualizado. El servidor anterior ignora las preferencias nuevas y no actualiza la anticipación de una suscripción existente. La compatibilidad comprobada permite que clientes anteriores sigan editando perfil sin reiniciar la selección por categoría.
