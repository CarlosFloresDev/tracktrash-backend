# TrackTrash Backend

API ASP.NET Core 10 para reciclaje y atención ciudadana en Arequipa. [Cliente Flutter independiente](https://github.com/CarlosFloresDev/tracktrash-mobile). Incremento 0.2: **20 entidades persistidas y 40 casos de uso**, con reglas y persistencia real. El conteo describe el alcance, no certifica calidad ni cumplimiento total.

## Funciones disponibles

- Perfil, aliados verificados, materiales, distritos, puntos de acopio y búsquedas PostGIS por radio.
- Recojos: crear, asignar por cercanía/material/capacidad, aceptar, iniciar viaje, cancelar, reprogramar, consultar historial y confirmar con código privado.
- Capacidad reservada al aceptar y liberada al cancelar/confirmar; control de versión para operaciones simultáneas.
- Mapa de contenedores con origen/antigüedad, estimaciones ciudadanas e incidencias con atención y resolución.
- Protocolo IoT autenticado por dispositivo: mediana, batería, contacto, secuencias únicas y rechazo de lecturas inválidas. Simulador HTTP incluido; hardware pendiente.
- Horarios semanales, reportes no verificados de paso y suscripciones con recordatorios persistidos dentro de la app.
- Libro de puntos: 20 por entrega, máximo 200 positivos al día de Perú; rachas actual/máxima y canjes transaccionales con costo/stock/idempotencia.
- Bandeja propia con lectura y eventos deduplicados, panel operativo y auditoría de cambios sin copiar datos privados.
- Preferencias por categoría (horarios, recojos, incidencias y canjes), pausa global y antelación de suscripciones editable. Silenciar avisos conserva el historial y no impide operaciones.

Piloto técnico disponible en [Flutter web](https://tracktrash.pages.dev) y [salud de la API](https://tracktrash-api.onrender.com/health/ready), con datos ficticios y autenticación Supabase real. La rama actual es `feat/ecosistema-tracktrash`; main conserva el primer incremento. [Matriz de los 40 RF y 26 RNF](docs/MATRIZ_COBERTURA.md): identifica cada requisito implementado, parcial o pendiente, incluyendo depósitos, push, instituciones, fotos, GPS/ETA y validación física.

## Ejecutar en Windows

Requisitos: SDK fijado en `global.json` y Docker Desktop con motor Linux activo.

```powershell
dotnet tool restore
powershell -NoProfile -ExecutionPolicy Bypass -File scripts/dev-setup.ps1
dotnet run --project src/TrackTrash.Api
```

El script genera contraseña en `.env` ignorado, guarda conexión en user-secrets, inicia PostgreSQL/PostGIS en localhost:55432 y aplica migraciones. No borra datos existentes. Datos demo sintéticos solo en Development con autenticación demo.

API http://localhost:5080; contrato `/openapi/v1.json` en Development; salud `/health/live` y `/health/ready`. En Linux/macOS configurar `ConnectionStrings__TrackTrash`, `.env` y `ASPNETCORE_ENVIRONMENT=Development`; ejecutar `docker compose up -d --wait`, `dotnet run --project src/TrackTrash.Api -- --migrate`, luego el host sin `--migrate`.

## Identidades y permisos

Development admite `Authorization: Bearer demo-vecino`, `demo-reciclador` y `demo-admin`; identidades secundarias `demo-vecino-2`/`demo-reciclador-2` permiten probar aislamiento. Tokens públicos de prueba, sin datos reales; el host impide habilitarlos fuera de Development.

Vecino consulta datos propios; reciclador ve solicitudes disponibles y asignadas, con dirección habilitada al aceptar; solo el propietario recibe el código. Operador atiende incidencias y administra catálogos. Los roles reales deben provenir de `app_metadata.tracktrash_role`, gestionado por servidor. El cliente no concede permisos.

Para teléfono físico, exponer deliberadamente la API en la red de desarrollo: `dotnet run --project src/TrackTrash.Api --urls http://0.0.0.0:5080`. Compilar Flutter con IP LAN; el APK disponible apunta a Android Emulator.

## Pruebas y calidad

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File scripts/test.ps1
dotnet build --configuration Release
```

47 pruebas backend: reglas, arquitectura, permisos/privacidad HTTP, concurrencia/rollback, PostGIS, IoT, incidencias, saldo/stock, topes diarios, rachas, auditoría, recordatorios y preferencias. Incluye preservar datos al migrar y actualizar suscripciones sin duplicarlas. Integración contra PostgreSQL real, no proveedor en memoria.

Las pruebas crean/borran bases `tracktrash_test_<guid>` en PostgreSQL local/CI. Nunca usar la conexión de un proyecto remoto o producción. Alternativa: `TRACKTRASH_TEST_CONNECTION` y `dotnet test`. GitHub Actions ejecuta restore bloqueado, build y pruebas con PostGIS efímero.

## Supabase real: integración preparada, pendiente de validación remota

```text
ConnectionStrings__TrackTrash=<conexión SQL del servidor, nunca del cliente>
Authentication__Demo=false
Authentication__Authority=https://<referencia>.supabase.co/auth/v1
Jobs__Enabled=true
Cors__Origins__0=https://<frontend>
```

La API valida firma, issuer, audience `authenticated` y expiración usando JWKS. Requiere claves asimétricas de Auth; no acepta el secreto compartido JWT legado. Flutter ya incorpora registro/login, renovación y almacenamiento seguro; faltan probar el proyecto real y completar retornos de recuperación de contraseña.

Tablas de negocio en esquema `tracktrash`, fuera de los esquemas expuestos por Data API. Conexión SQL de privilegios mínimos, sin claves `service_role` en Flutter. La autorización C# no crea automáticamente políticas RLS para otros accesos. Storage, push y Realtime son integraciones independientes todavía pendientes.

Habilitar PostGIS con rol autorizado y comprobar el esquema donde se instala. En Supabase puede ser `extensions`; la conexión/migración debe resolver tipos y funciones espaciales, por ejemplo mediante Search Path apropiado. Revisar el SQL y probarlo en staging antes de tocar datos reales:

```powershell
dotnet ef migrations script --idempotent --context TrackTrashDbContext --project src/TrackTrash.Infrastructure --startup-project src/TrackTrash.Api --output artifacts/migrations.sql
```

Guías oficiales consultadas: [claves JWT Supabase](https://supabase.com/docs/guides/auth/signing-keys), [extensiones](https://supabase.com/docs/guides/database/extensions), [PostGIS](https://supabase.com/docs/guides/database/extensions/postgis).

## Operación, sensores y despliegue

`Jobs:Enabled` activa el worker cada 30 segundos. Persiste próximo recordatorio y evento único; avisos solo internos, sin entrega FCM/APNs. Rutas demo son horarios sintéticos, no posición ni ETA de un camión.

Registrar dispositivo como admin (`POST /api/v1/iot/dispositivos`); guardar la clave retornada una sola vez. Se almacena únicamente su hash. El simulador toma la clave de `TRACKTRASH_DEVICE_KEY`:

```powershell
python tools/simulate_sensor.py --device <uuid> --sequence 1 --distance 25 --battery 80
```

No es firmware ESP32 ni valida autonomía/temperatura/carcasa. No publicar claves del dispositivo en capturas o commits.

Dockerfile multietapa, usuario no root, puerto 8080, sin migración automática al arrancar:

```powershell
docker build -t tracktrash-api:0.2 .
```

Imagen construida localmente y desplegada en Render Free con HTTPS, secretos del host y Supabase. El piloto todavía requiere observabilidad operativa, prueba de carga y respaldos restaurados; no acredita disponibilidad de producción. [Referencia de contenedores .NET](https://learn.microsoft.com/dotnet/core/docker/introduction).

## Arquitectura y trabajo

Piloto remoto: [recursos creados, configuración, evidencia y pendientes](docs/DESPLIEGUE_GRATUITO.md). Supabase tiene las siete migraciones; API y web publicadas en planes gratuitos. Render puede dormir por inactividad.

| Proyecto | Responsabilidad |
|---|---|
| Domain | Entidades, reglas, repositorios e IUnitOfWork; sin EF/ASP.NET |
| Application | DTOs, servicios abstractos y casos de uso concretos con ExecuteAsync |
| Infrastructure | EF/LINQ, PostgreSQL/PostGIS, adapters, identidad, middleware, worker y DI |
| Api | Controllers y composición del host |

[Contrato HTTP](docs/API.md), [requisitos revisados](docs/REQUISITOS_REVISADOS.md), [cobertura/backlog](docs/MATRIZ_COBERTURA.md), [arquitectura](docs/ARQUITECTURA_TRACKTRASH.md). Los planes iniciales se conservan como antecedente; la matriz describe el estado actual.

`scripts/scaffold.ps1` genera referencia en artifacts sin sobrescribir Domain; migraciones EF son la autoridad del esquema. `main` contiene la base integrada; incrementos en `feat/<tarea>` con commits por unidades coherentes y PR con CI. No se necesitan microservicios para esta escala.
