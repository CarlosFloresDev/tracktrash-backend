# Revisión de arquitectura: implementación anterior de TrackTrash

**Alcance:** ramas `feat/ecosistema-tracktrash` (commits 2155d90…8d9ae27) y `refactor/cqrs-mediatr` (dfd4f17).
**Criterio:** Clean Architecture (4 capas) y CQRS con MediatR.
**Fecha:** 2026-11-02

La funcionalidad anterior es correcta y está bastante probada, pero tiene problemas de diseño. El equipo **no debe copiarla tal cual**: debe reimplementarla sobre `main` con la arquitectura objetivo (sección 3). El clon guía (`tracktrash-backend-guia`) corrige la estructura y la validación; los problemas que siguen abiertos en él aparecen marcados como **[pendiente en la guía]**.

---

## 1. Hallazgos

### A. Estructura CQRS

| # | Problema | Dónde | Corrección |
|---|---|---|---|
| A1 | `feat/ecosistema-tracktrash` no usa CQRS: hay clases `XxxUseCase.ExecuteAsync` que el controlador inyecta con `[FromServices]`. | `Application/UseCases/*UseCase.cs` | Un `Command` o `Query` de MediatR por cada caso de uso. |
| A2 | En `refactor/cqrs-mediatr`, el Command, el Handler y la validación están en **un solo archivo** y en una carpeta plana `Commands/`. | `UseCases/*/Commands/*.cs` | Una carpeta por acción: `Commands/<Accion>/{Command, Handler, Validator}`. |
| A3 | Los casos de uso se agrupan por área («Catalogo», «Operacion», «Entregas», «Recompensas») en lugar de por entidad. Por ejemplo, `ConfirmarEntrega` queda fuera de `SolicitudesRecojo` y `RegistrarDispositivo` dentro de `Contenedores`. | `UseCases/` | Agrupar por entidad: `Asociaciones`, `Contenedores`, `DispositivosIoT`, `Canjes`, `Beneficios`, `MovimientosPuntos`, etc. |
| A4 | Los Commands son `record` mutables (`{ get; set; }`) con `= null!`. | todos los Commands | Propiedades `init` (o un record posicional) e inmutables. |
| A5 | Los nombres no siguen un patrón uniforme (`ConsultarAliadosQuery` devuelve asociaciones). | Catálogo | `<Verbo><Entidad>{Command|Query}`; por ejemplo, `ListarAsociacionesQuery`. |

### B. Validación y pipeline

| # | Problema | Corrección |
|---|---|---|
| B1 | No hay FluentValidation ni `IPipelineBehavior`. La validación está repartida entre DataAnnotations en los DTOs y Commands, `if` dentro de los handlers (`ConsultarMapa`, `BuscarCandidatos`, `ListarSolicitudes`) y el dominio. | Un `XxxValidator : AbstractValidator<XxxCommand>` por request y un `ValidationBehavior<,>` registrado en MediatR. |
| B2 | Los errores de formato devuelven **422** (se lanza `ReglaNegocioException` desde la query) cuando deberían devolver 400. | `ValidacionException` → **400** `ValidationProblemDetails`. Las reglas de negocio siguen en **422**. |
| B3 | Las DataAnnotations (`[Required]`, `[Range]`) están en la capa Application y mezclan el contrato HTTP con el caso de uso. | Los contratos HTTP (`*Request`) van a `Api/Contracts`; las reglas, al Validator. |

### C. Dependencias entre capas

| # | Problema | Corrección |
|---|---|---|
| C1 | `ExceptionHandlingMiddleware` vive en **Infrastructure**, pero es una preocupación HTTP. | Moverlo a `Api/Middlewares`. |
| C2 | Infrastructure registra MediatR (`AddMediatR` dentro de `AddInfrastructure`) y hace referencia a un Command concreto. | `Application/DependencyInjection.AddApplication()` registra los handlers, validators y behaviors. |
| C3 | `TrackTrashDbContext` depende de `IHttpContextAccessor` para auditar, así que la persistencia queda acoplada a HTTP. **[pendiente en la guía]** | `SaveChangesInterceptor` de auditoría que use `IUsuarioActualService` (o que reciba el actor desde un behavior). |
| C4 | El read model `IndicadoresOperacion` y su interfaz están en **Domain** (`Ports/Repositories`), aunque no es un agregado. | Puerto de lectura `Application/Ports/Queries/IIndicadoresConsulta` que devuelve un DTO. |
| C5 | `Program.cs` consulta el `DbContext` directamente (health/ready) y además ejecuta migraciones y seeds. Es aceptable en la raíz de composición, pero conviene extraerlo. **[pendiente en la guía]** | `MapHealthChecks` con `AddDbContextCheck` y un comando `--migrate` separado. |

### D. Repositorios y Unit of Work

| # | Problema | Corrección |
|---|---|---|
| D1 | `ICatalogoRepository` es un «cajón de sastre» que reúne distritos, asociaciones, puntos de acopio y candidatos. `IContenedorRepository` añade dispositivos, lecturas y reportes ciudadanos. **[pendiente en la guía]** | Un repositorio por **agregado raíz** (`IAsociacionRepository`, `IContenedorRepository`, `IDispositivoIoTRepository`…). |
| D2 | Las queries cargan entidades completas y mapean en memoria. `ConsultarMapa` hace `Take(200)` de asociaciones y **después** filtra por distancia en C#, así que puede omitir aliados. **[pendiente en la guía]** | Las queries proyectan directamente a un DTO (`Select`) con filtro espacial en PostGIS, mediante puertos de lectura como en C4. |
| D3 | Cada handler llama a `GuardarCambiosAsync` y no hay transacción explícita cuando hay varias escrituras. Funciona porque hay un solo `SaveChanges`, pero depende de esa disciplina. | Mantener **un** `GuardarCambiosAsync` por Command (regla de revisión) o añadir `TransactionBehavior` solo para Commands. |
| D4 | Para forzar la concurrencia se usa `Version++` manual con hacks (`PerfilUsuario.RegistrarOperacionSaldo()` solo incrementa la versión). **[pendiente en la guía]** | Usar `xmin` de PostgreSQL como token de concurrencia (`UseXminAsConcurrencyToken`) o un método de dominio con significado real. |

### E. Dominio (entidades anémicas o sobrecargadas)

| # | Problema | Corrección |
|---|---|---|
| E1 | Hay lógica de negocio en los handlers: la zona horaria de Perú (`TimeSpan.FromHours(-5)`) en `ConfirmarEntrega`, el umbral de 80 % y «Saturación» en `ReportarNivel` y `RegistrarLectura`, el texto «20 puntos» fijo aunque existe `PoliticaRecompensas`, y el motivo de un movimiento deducido del signo de los puntos. **[pendiente en la guía]** | Llevarla al dominio: `PoliticaRecompensas.InicioDelDia(ahora)`, `Contenedor.EstaSaturado`, `MovimientoPuntos.Motivo`. |
| E2 | Hay «strings mágicos» en lugar de enums: `Contenedor.Estado()` devuelve `"Saturado"`, `Fuente = "Sensor IoT"` e `Incidencia.Tipo = "Saturación"`. **[pendiente en la guía]** | `enum EstadoContenedor`, `enum FuenteNivel` y `enum TipoIncidencia`. |
| E3 | `Ubicacion` es un helper estático que también valida textos (`Ubicacion.Texto`), lo que mezcla responsabilidades. | `Ubicacion` como *value object* (Lat/Lng) y una `Guard` o `TextoValidado` aparte. |
| E4 | Hay efectos secundarios encadenados a mano en el handler: historial, notificación, liberar la capacidad de la asociación y la racha. `ConfirmarEntregaHandler` tiene **9 dependencias**. **[pendiente en la guía]** | Eventos de dominio (`EntregaConfirmada`) publicados con `INotification` de MediatR después de guardar, con un handler por efecto. |
| E5 | La autoasignación está duplicada en `CrearSolicitudRecojo` y `AsignarSolicitud`. | Un servicio de dominio o aplicación `AsignadorDeAliados`. |

### F. Autorización y API

| # | Problema | Corrección |
|---|---|---|
| F1 | El rol se comprueba dos veces: con `[Authorize(Roles=...)]` en el controller y con `if (!usuario.EsAdministrador) throw` en el handler. Además, `ObtenerPerfil` calcula el string del rol. | Elegir una sola: el atributo en la API y un `AuthorizationBehavior` con una interfaz marcador `IRequiereRol` si se necesita en Application. |
| F2 | Los controllers devuelven `IActionResult` sin tipo, responden `200` en las creaciones (en lugar de `201`) y usan objetos anónimos (`new { id }`). | `ActionResult<TDto>`, `CreatedAtAction` y `[ProducesResponseType]`. |

### G. Pruebas y testabilidad

| # | Problema | Corrección |
|---|---|---|
| G1 | De 53 pruebas, **26 necesitan PostgreSQL** (`ApiFlowTests` parcial repartida en 5 archivos) y fallan sin `TRACKTRASH_TEST_CONNECTION`. No hay pruebas unitarias de handlers ni de validators. | Pruebas unitarias de Validators y Handlers con dobles en memoria; las de integración, marcadas con `[Trait("Categoria","Integracion")]`. |
| G2 | El test de arquitectura solo comprueba referencias entre ensamblados, no las convenciones. | Tests de convención: carpeta y namespace, que exista Handler y Validator, e inmutabilidad (ya incluidos en la guía, `CqrsTests`). |

---

## 2. Qué corrige ya el clon guía

A1–A5, B1–B3, C1, C2, C4 y G1–G2 (parcial). El resto (C3, C5, D1, D2, D4, E1–E5, F1, F2) queda **para que cada integrante lo corrija en su paquete** (ver `REPARTO_DE_TAREAS.md`).

---

## 3. Arquitectura objetivo

```
src/
  TrackTrash.Domain/            entidades, enums, value objects, excepciones de dominio, puertos de repositorio
  TrackTrash.Application/
    DependencyInjection.cs      AddApplication(): MediatR + validators + behaviors
    Behaviors/                  ValidationBehavior (y opcionalmente Transaction/Authorization)
    Exceptions/                 ValidacionException, RecursoNoEncontrado, AccesoDenegado, Conflicto
    Ports/Services/             IUsuarioActualService, INotificador..., ICodigoEntregaService
    Ports/Queries/              puertos de lectura que devuelven DTOs (IIndicadoresConsulta)
    UseCases/<Entidad>/
      Commands/<Accion>/        <Accion>Command.cs · <Accion>CommandHandler.cs · <Accion>CommandValidator.cs [· <X>Dto.cs]
      Queries/<Consulta>/       <Consulta>Query.cs · <Consulta>QueryHandler.cs · <X>Dto.cs [· <Consulta>QueryValidator.cs]
      Common/                   DTOs y mapeos compartidos por varios casos de uso de la entidad
  TrackTrash.Infrastructure/    EF Core (DbContext, IEntityTypeConfiguration, Migrations, Repositories, Queries), identidad, jobs
  TrackTrash.Api/               Controllers, Contracts (Request), Middlewares, Program.cs
tests/TrackTrash.Tests/         unitarias (sin BD) + integración [Trait Categoria=Integracion]
```

### Reglas de dependencia

- `Domain` → no depende de nada (ni de EF Core ni de MediatR).
- `Application` → solo de `Domain`, MediatR y FluentValidation. **Nunca** de EF Core ni de ASP.NET.
- `Infrastructure` → de `Application` y `Domain`. Implementa los puertos.
- `Api` → de `Application` (y de `Infrastructure` solo en `Program.cs`, como raíz de composición).
- Los controllers solo hacen: Request → Command o Query → `ISender.Send` → respuesta HTTP. No contienen lógica.

### Convenciones de nombres

| Elemento | Patrón | Ejemplo |
|---|---|---|
| Carpeta de entidad | plural | `UseCases/SolicitudesRecojo` |
| Command | `<Verbo><Entidad>Command` | `CancelarSolicitudCommand` |
| Query | `<Listar|Obtener|Consultar|Buscar><X>Query` | `ListarRutasQuery` |
| Handler | `<Request>Handler`, `internal sealed` | `CancelarSolicitudCommandHandler` |
| Validator | `<Request>Validator`, `internal sealed` | `CancelarSolicitudCommandValidator` |
| Namespace | igual a la carpeta | `TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.CancelarSolicitud` |
| Request HTTP | `<Accion>Request` en `Api/Contracts` | `CancelarSolicitudRequest` |
| DTO de salida | `<X>Dto` | `SolicitudDto` |

### Códigos HTTP

| Excepción | Código |
|---|---|
| `ValidacionException` | 400 |
| `AccesoDenegadoException` | 403 |
| `RecursoNoEncontradoException` | 404 |
| `ConflictoPersistenciaException` | 409 |
| `ReglaNegocioException` | 422 |
| Cualquier otra | 500 |
