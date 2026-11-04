using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Domain.Ports.Persistence;

public interface IUnitOfWork
{
    ISolicitudRecojoRepository Solicitudes { get; }
    IMovimientoPuntosRepository MovimientosPuntos { get; }
    IBeneficioRepository Beneficios { get; }
    ICatalogoRepository Catalogos { get; }
    IContenedorRepository Contenedores { get; }
    IHistoricoRepository Historial { get; }
    IIncidenciaRepository Incidencias { get; }
    INotificacionRepository Notificaciones { get; }
    IPerfilRepository Perfiles { get; }
    IRutaRepository Rutas { get; }

    IRepository<TEntity> Repositorio<TEntity>() where TEntity : class;
    Task GuardarCambiosAsync(CancellationToken ct);
}
