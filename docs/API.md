# Contrato HTTP v1: solicitudes de recojo

Implementación inicial. Fuente ejecutable del contrato: `/openapi/v1.json` en Development.

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

La fecha del ejemplo debe reemplazarse por una futura dentro de 30 días. Materiales: `PapelCarton`, `Plastico`, `Vidrio`, `Metal`. Estados: `Pendiente`, `Aceptada`, `Confirmada`.

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

El código se devuelve solo al vecino propietario. La dirección se devuelve al propietario y al reciclador asignado; los demás recicladores elegibles reciben ambos campos como null. Aún no hay búsqueda por zona ni verificación de aliados, por eso solo se habilita la demostración con datos sintéticos.

Confirmar: `{"codigo":"123456"}`. El backend verifica asignación/código; confirmar otra vez no suma puntos. La aceptación es idempotente para el mismo reciclador. La confirmación y el movimiento se guardan en una transacción PostgreSQL.

Errores: 400 datos inválidos, 401 sesión ausente/inválida, 403 operación no permitida, 404 solicitud inaccesible/inexistente, 409 conflicto concurrente, 422 regla de negocio, 429 límite de intentos y 500 fallo interno. Errores controlados usan ProblemDetails con title/status/traceId; el cliente también admite 401/403/429 sin cuerpo. El límite inicial es 30 operaciones por minuto por actor.

No hay notificaciones push, pagos, canjes, borrado ni cancelación en este contrato inicial.
