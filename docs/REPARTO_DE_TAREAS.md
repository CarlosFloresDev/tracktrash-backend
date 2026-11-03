# Reparto de tareas: reimplementación de TrackTrash sobre `main`

**Base:** `main` en `0d88e3a`. Ya contiene el andamiaje de 4 capas, el primer flujo de recojos (casos de uso clásicos: crear, aceptar, listar y obtener solicitud, confirmar entrega e impacto) y **las 24 entidades de dominio, una por archivo** (incluidas EventoAuditoria, PoliticaRecompensas, ReporteCiudadano, ReportePasoCamion, RutaRecoleccion, SuscripcionRuta, TipoAviso y Ubicacion).
**No hay que crear entidades nuevas.** Cada paquete implementa los casos de uso, la infraestructura (mapeo EF, repositorios, migración), los endpoints y las pruebas. Solo se ajustan las entidades cuando lo pide la revisión (`docs/REVISION_ARQUITECTURA.md`).

**Referencia:** clon guía `D:\descargas\Tecsup\tecsup 2026-2\DAEA\TRACKTRASH\tracktrash-backend-guia\` (ver `GUIA.md`). Úsalo para entender el comportamiento esperado, **no para copiar y pegar**.

## Orden y dependencias

```
P0 Carlos: base CQRS ──► P1 Integrante 1: mapa e IoT ──► P2 Integrante 2: recojos y recompensas
                     └──► P3 Integrante 3: comunidad y avisos   (en paralelo con P1)
```

- **P0** se fusiona primero (1–2 días). Sin `AddApplication`, `ValidationBehavior` ni el middleware en la API, nadie puede seguir el patrón.
- **P1** y **P3** van en paralelo después de P0. **P2** necesita el `IAsociacionRepository` y la búsqueda de candidatos de P1 para la autoasignación y las reservas. Mientras tanto, puede avanzar con las recompensas y los estados.
- **Migraciones:** una sola migración por PR, creada **después** de hacer rebase sobre `main` actualizado. El orden de fusión es P0 → P1 → P3 → P2. Si `TrackTrashDbContextModelSnapshot.cs` tiene conflicto, se borra la migración propia, se hace rebase y se regenera.

## Reglas comunes (todos los paquetes)

- Estructura `Application/UseCases/<Entidad>/Commands/<Accion>/{Command, Handler, Validator}` y `Queries/<Consulta>/{Query, Handler, Dto}`.
- Commands y Queries inmutables (`init`). Handlers y Validators `internal sealed`.
- Contratos HTTP en `Api/Contracts`. Los controllers solo llaman a `ISender.Send`.
- Mapeo EF con un `IEntityTypeConfiguration<T>` por entidad en `Infrastructure/Persistence/Configurations/`. No se añade más código al `OnModelCreating` gigante.
- Repositorios **por agregado** (corrección D1). Las queries de lectura proyectan a DTO (corrección D2).
- Pruebas unitarias de validators y handlers sin BD (con dobles en memoria, como `CqrsTests` en la guía) y al menos una prueba de integración por endpoint principal, marcada con `[Trait("Categoria","Integracion")]`.
- `dotnet build` sin warnings y `dotnet test --filter "Categoria!=Integracion"` en verde antes de abrir el PR.

---

## P0: Carlos (coordinación, base CQRS y revisión)

**Rama:** `feat/coordinador-base-cqrs` (la crea Carlos cuando empiece).

- **Entidades:** ninguna nueva.
- **Commands y Queries:** migrar a CQRS el primer flujo que ya está en main. Commands: `CrearSolicitudRecojo`, `AceptarSolicitudRecojo` y `ConfirmarEntrega`. Queries: `ListarSolicitudesRecojo`, `ObtenerSolicitudRecojo` y `ObtenerImpacto`. Eliminar las clases `*UseCase`.
- **Base técnica:**
  - `Application/DependencyInjection.AddApplication()`;
  - `Behaviors/ValidationBehavior`;
  - `Exceptions/ValidacionException`;
  - `Api/Middlewares/ExceptionHandlingMiddleware` (sacarlo de Infrastructure; 400/403/404/409/422);
  - `Api/Contracts`;
  - paquetes FluentValidation 12.0.0.
- **Endpoints:** los de `/api/v1/solicitudes` existentes, sin cambiar el contrato.
- **Infraestructura:** separar el mapeo actual en `Configurations/`. Sin migración nueva.
- **Pruebas:**
  - `CqrsTests` (convenciones: carpeta, Handler, Validator, init);
  - marcar `ApiFlowTests` como Integración;
  - documentar `scripts/test.ps1`.
- **Correcciones:** A1, A2, A4, B1–B3, C1, C2, G1, G2.
- **Despliegue:** al final, revisar Dockerfile, `render.yaml` y `compose.yaml`.
- **Revisión:** revisa y fusiona los PR de P1–P3 con la checklist del PR.
- **Criterios de aceptación:** el primer flujo funciona igual (mismos tests de integración en verde con BD); un request inválido devuelve 400 con `errors`; los tests de convención pasan.
- **Guía:**
  - `src/TrackTrash.Application/{DependencyInjection.cs,Behaviors,Exceptions}`;
  - `src/TrackTrash.Api/{Middlewares,Contracts}`;
  - `UseCases/SolicitudesRecojo`;
  - `tests/TrackTrash.Tests/CqrsTests.cs`.

---

## P1: Integrante 1 (mapa espacial, catálogo e IoT)

**Rama:** `feat/integrante-1-mapa-iot`

- **Entidades (ya existen):** Distrito, AsociacionReciclaje, MaterialAceptado, PuntoAcopio, Contenedor, DispositivoIoT, LecturaNivel, ReporteCiudadano y Ubicacion.
- **Commands:**
  - `Asociaciones/CrearAsociacion` y `VerificarAsociacion`;
  - `Contenedores/CrearContenedor` y `ReportarNivel`;
  - `DispositivosIoT/RegistrarDispositivo` y `RegistrarLectura` (idempotente por secuencia, clave hasheada; si el nivel supera el 80 %, se crea una incidencia de saturación a través del puerto que expone P3, o se deja un TODO coordinado).
- **Queries:**
  - `Distritos/ListarDistritos`;
  - `Asociaciones/ListarAsociaciones` y `BuscarCandidatos` (verificadas, capacidad libre, material y ≤ 20 km, ordenadas por distancia);
  - `Contenedores/ConsultarMapa` (contenedores, aliados y puntos dentro del radio).
- **Endpoints:**

  | Método | Ruta | Rol |
  |---|---|---|
  | GET | `/api/v1/mapa?lat&lng&radioKm` | autenticado |
  | GET | `/api/v1/distritos` | autenticado |
  | GET | `/api/v1/asociaciones` | autenticado |
  | GET | `/api/v1/asociaciones/candidatos` | autenticado |
  | POST | `/api/v1/asociaciones` | admin |
  | POST | `/api/v1/asociaciones/{id}/verificacion` | admin |
  | POST | `/api/v1/contenedores` | admin |
  | POST | `/api/v1/contenedores/{id}/reporte-nivel` | autenticado |
  | POST | `/api/v1/iot/dispositivos` | admin |
  | POST | `/api/v1/iot/dispositivos/{id}/lecturas` | anónimo, cabecera `X-Device-Key` |

- **Infraestructura:**
  - PostGIS (`UseNetTopologySuite`, columnas `geography(Point,4326)` e índices GiST);
  - repositorios `IAsociacionRepository`, `IContenedorRepository`, `IDispositivoIoTRepository` y `IDistritoRepository`;
  - puerto de lectura `IMapaConsulta` que filtra en BD (corrige el `Take(200)` y el filtrado en memoria);
  - migración `MapaEIoT` y seed de demostración de distritos de Arequipa;
  - `tools/simulate_sensor.py`.
- **Pruebas:** validators (coordenadas, radio, distancias 3–11); dominio de `LecturaNivel` (mediana → porcentaje); `RegistrarLectura` duplicada → `Duplicada=true`; integración del mapa con PostGIS.
- **Correcciones:** D1, D2, E2 (`EstadoContenedor` y `FuenteNivel` como enums), E3 (`Ubicacion` como value object), F2.
- **Criterios de aceptación:** `radioKm=1000` → 400; un aliado no verificado no aparece; una lectura repetida no duplica; los candidatos salen ordenados por distancia; el seed carga al menos 3 contenedores y 2 asociaciones.
- **Guía:**
  - `UseCases/{Asociaciones,Contenedores,DispositivosIoT,Distritos}`;
  - `Infrastructure/Persistence/{EcosistemaModel.cs,Repositories/CatalogoRepository.cs,Repositories/ContenedorRepository.cs,DemoSeed.cs}`;
  - `Migrations/*EcosistemaCircular*`;
  - `Api/Controllers/{MapaController,IoTController}.cs`;
  - `tools/simulate_sensor.py`.

---

## P2: Integrante 2 (ciclo completo del recojo y recompensas)

**Rama:** `feat/integrante-2-recojos-recompensas`

- **Entidades (ya existen):** SolicitudRecojo (nuevos estados), HistoricoSolicitud, MovimientoPuntos, PoliticaRecompensas, Beneficio, Canje y PerfilUsuario (racha).
- **Commands:**
  - `SolicitudesRecojo/AsignarSolicitud`, `IniciarViaje`, `CancelarSolicitud` (libera la reserva) y `ReprogramarSolicitud`;
  - ampliar `CrearSolicitudRecojo` con ubicación y `AutoAsignar`;
  - ampliar `AceptarSolicitudRecojo` para reservar capacidad en la asociación;
  - ampliar `ConfirmarEntrega` con tope diario de 200 puntos, racha y liberación de capacidad;
  - `Beneficios/CrearBeneficio`;
  - `Canjes/CanjearBeneficio` (idempotente por clave y transaccional con el saldo).
- **Queries:** `SolicitudesRecojo/ConsultarHistorial`, `Beneficios/ListarBeneficios`, `Canjes/ListarCanjes` y `MovimientosPuntos/ListarMovimientos`.
- **Endpoints:**

  | Método | Ruta | Rol |
  |---|---|---|
  | POST | `/api/v1/solicitudes/{id}/asignacion` | vecino |
  | POST | `/api/v1/solicitudes/{id}/viaje` | reciclador |
  | POST | `/api/v1/solicitudes/{id}/cancelacion` | — |
  | POST | `/api/v1/solicitudes/{id}/reprogramacion` | vecino |
  | GET | `/api/v1/solicitudes/{id}/historial` | — |
  | GET | `/api/v1/beneficios` | — |
  | POST | `/api/v1/beneficios` | admin |
  | GET | `/api/v1/canjes` | — |
  | POST | `/api/v1/canjes` | vecino |
  | GET | `/api/v1/perfil/movimientos` | — |

- **Infraestructura:** repositorios `IHistoricoRepository` e `IBeneficioRepository` (con `ICanjeRepository` por separado); concurrencia con `xmin`; índices únicos (clave de canje y solicitud por movimiento); migración `CicloRecojoYRecompensas`.
- **Pruebas:**
  - dominio: transiciones de estado inválidas → 422, y la racha que cruza la medianoche en Perú;
  - handlers con dobles en memoria: tope diario y reutilización de la clave de canje con otro beneficio;
  - integración: dos canjes concurrentes → solo uno exitoso (409/422 el otro).
- **Correcciones:** D3, D4, E1 (zona horaria y puntos en `PoliticaRecompensas`; motivo en `MovimientoPuntos`), E4 (eventos de dominio `EntregaConfirmada` y `SolicitudCancelada` para el historial y los avisos), E5 (`AsignadorDeAliados` único), F1.
- **Dependencias:** P0 fusionado. `BuscarCandidatos` e `IAsociacionRepository` de P1 son necesarios para la asignación y las reservas. Las notificaciones usan el `INotificadorUsuarioService` de P3; hasta que exista, se usa un stub.
- **Criterios de aceptación:**
  - flujo completo Pendiente → Aceptada → EnCamino → Confirmada con historial;
  - cancelar una solicitud aceptada libera los kg reservados;
  - el 11.º recojo del día confirma la entrega sin otorgar puntos;
  - un canje sin saldo → 422;
  - la misma clave de canje devuelve el mismo canje.
- **Guía:**
  - `UseCases/{SolicitudesRecojo,Beneficios,Canjes,MovimientosPuntos}`;
  - `Domain/Entities/{SolicitudRecojo,PerfilUsuario,PoliticaRecompensas,Beneficio}.cs`;
  - `Migrations/*CicloRecojoYCapacidad*` y `*RachasYReportesCiudadanos*`;
  - `tests/TrackTrash.Tests/BusinessRulesTests.cs`.

---

## P3: Integrante 3 (comunidad, rutas, avisos y auditoría)

**Rama:** `feat/integrante-3-comunidad-avisos`

- **Entidades (ya existen):** Incidencia, EstadoIncidencia, Notificacion, TipoAviso, PerfilUsuario (preferencias), RutaRecoleccion, SuscripcionRuta, ReportePasoCamion y EventoAuditoria.
- **Commands:**
  - `Incidencias/CrearIncidencia`, `AtenderIncidencia` y `ResolverIncidencia`;
  - `Notificaciones/LeerNotificacion`;
  - `Perfiles/ActualizarPerfil` (con preferencias por tipo de aviso);
  - `Rutas/SuscribirRuta` (si ya existe, cambia la antelación), `CancelarSuscripcion`, `ReportarPaso` (como mucho una vez cada 30 minutos) y `ProcesarRecordatorios` (lo ejecuta un worker).
- **Queries:** `Incidencias/ListarIncidencias`, `Notificaciones/ListarNotificaciones`, `Perfiles/ObtenerPerfil`, `Rutas/ListarRutas` e `Indicadores/ObtenerIndicadores` (admin).
- **Endpoints:**

  | Método | Ruta | Rol |
  |---|---|---|
  | GET | `/api/v1/incidencias` | — |
  | POST | `/api/v1/incidencias` | — |
  | POST | `/api/v1/incidencias/{id}/atencion` | admin |
  | POST | `/api/v1/incidencias/{id}/resolucion` | admin |
  | GET | `/api/v1/notificaciones` | — |
  | POST | `/api/v1/notificaciones/{id}/lectura` | — |
  | GET | `/api/v1/perfil` | — |
  | PUT | `/api/v1/perfil` | — |
  | GET | `/api/v1/rutas` | — |
  | POST | `/api/v1/rutas/{id}/suscripcion` | — |
  | DELETE | `/api/v1/rutas/{id}/suscripcion` | — |
  | POST | `/api/v1/rutas/{id}/paso` | — |
  | GET | `/api/v1/operacion/indicadores` | admin |

- **Infraestructura:**
  - repositorios `IIncidenciaRepository`, `INotificacionRepository`, `IPerfilRepository` e `IRutaRepository`;
  - adaptador `NotificadorUsuarioService`, que respeta las preferencias y se añade a la misma unidad de trabajo;
  - `RecordatoriosWorker` (`BackgroundService` controlado por `Jobs:Enabled`);
  - puerto `IIndicadoresConsulta`;
  - auditoría mediante un **`SaveChangesInterceptor`** (sin `IHttpContextAccessor` dentro del DbContext);
  - migración `ComunidadYAvisos` y seed de rutas de demostración.
- **Pruebas:**
  - validators: antelación de 5–60 minutos → 400 y textos de incidencia;
  - dominio: `PerfilUsuario.PermiteAviso`, `SuscripcionRuta.Programar` e `Incidencia.Resolver` por un responsable distinto → 422;
  - handler `ProcesarRecordatorios` con `TimeProvider` falso;
  - integración: las preferencias desactivadas no generan aviso, pero la operación sí se guarda.
- **Correcciones:** C3 (interceptor de auditoría), C4, E2 (`TipoIncidencia` como enum), F1 (el rol se calcula en un solo sitio), F2.
- **Criterios de aceptación:**
  - un vecino solo ve sus incidencias y el admin las ve todas;
  - reportar el paso dos veces en menos de 30 minutos → 422;
  - una suscripción repetida actualiza la antelación sin duplicar;
  - un recordatorio vencido genera un aviso por cada paso;
  - cada escritura deja un `EventoAuditoria` con su actor.
- **Guía:**
  - `UseCases/{Incidencias,Notificaciones,Perfiles,Rutas,Indicadores}`;
  - `Application/Ports/Queries`;
  - `Infrastructure/{BackgroundJobs,Adapters/Notifications,Persistence/Queries}`;
  - `Infrastructure/Persistence/TrackTrashDbContext.cs` (auditoría, el modelo a corregir);
  - `Migrations/*HorariosYRecordatorios*`, `*AuditoriaOperaciones*` y `*PreferenciasAvisos*`;
  - `tests/TrackTrash.Tests/PreferenciasAvisosTests.cs`.

---

## Resumen de carga

| Paquete | Commands | Queries | Endpoints | Migración |
|---|---|---|---|---|
| P0 Carlos | 3 (migrados) | 3 (migradas) | existentes | — |
| P1 Integrante 1 | 6 | 4 | 10 | MapaEIoT (PostGIS) |
| P2 Integrante 2 | 8 (3 ampliados) | 4 | 10 | CicloRecojoYRecompensas |
| P3 Integrante 3 | 8 | 5 | 13 | ComunidadYAvisos |

P3 tiene más endpoints, pero son CRUD sencillos. P2 concentra la lógica transaccional y P1 la parte espacial (PostGIS).
