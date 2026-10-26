# Guía para explicar TrackTrash

[Abrir la infografía y la explicación completa de Android, web y API](https://github.com/CarlosFloresDev/tracktrash-mobile/blob/feat/ecosistema-tracktrash/docs/GUIA_FLUJO_CODIGO.md).

El mismo cliente Flutter se compila para Android/web. Supabase Auth autentica al usuario y entrega JWT; la API valida ese token y ejecuta las reglas antes de persistir los datos de negocio en PostgreSQL/PostGIS.

| Para encontrar | Revisar |
|---|---|
| Arranque, CORS, autenticación y salud | [Program.cs](../src/TrackTrash.Api/Program.cs) |
| Endpoints de recojos | [SolicitudesRecojoController.cs](../src/TrackTrash.Api/Controllers/SolicitudesRecojoController.cs) |
| Crear un recojo | [CrearSolicitudRecojoUseCase.cs](../src/TrackTrash.Application/UseCases/SolicitudesRecojo/CrearSolicitudRecojoUseCase.cs) |
| Confirmar y coordinar puntos/historial/aviso | [ConfirmarEntregaUseCase.cs](../src/TrackTrash.Application/UseCases/Entregas/ConfirmarEntregaUseCase.cs) |
| Estados y reglas de la solicitud | [SolicitudRecojo.cs](../src/TrackTrash.Domain/Entities/SolicitudRecojo.cs) |
| Política de puntos | [PoliticaRecompensas.cs](../src/TrackTrash.Domain/Entities/PoliticaRecompensas.cs) |
| JWT, roles e inyección | [DependencyInjection.cs](../src/TrackTrash.Infrastructure/Configuration/DependencyInjection.cs) |
| EF, tablas y auditoría | [TrackTrashDbContext.cs](../src/TrackTrash.Infrastructure/Persistence/TrackTrashDbContext.cs) |
| Consultas LINQ | [Repositories](../src/TrackTrash.Infrastructure/Persistence/Repositories) |
| Confirmación de la unidad de trabajo | [UnitOfWork.cs](../src/TrackTrash.Infrastructure/Persistence/UnitOfWork.cs) |

El recorrido en ejecución es controller → caso de uso → reglas de dominio y contratos → implementaciones de persistencia. Las referencias entre proyectos siguen Clean Architecture: Domain permanece independiente de EF/ASP.NET; los adaptadores concretos están en Infrastructure. [Arquitectura detallada](ARQUITECTURA_IMPLEMENTADA.md).

La rama actual es `feat/ecosistema-tracktrash`; los PR #2 siguen abiertos. El diagrama representa el piloto con datos ficticios y avisos internos. Push, GPS de camiones, hardware y otras brechas de la [matriz](MATRIZ_COBERTURA.md) siguen pendientes.
