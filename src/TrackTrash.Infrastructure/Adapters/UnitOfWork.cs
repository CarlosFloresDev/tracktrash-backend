using Microsoft.EntityFrameworkCore;
using Npgsql;
using TrackTrash.Application.Exceptions;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Infrastructure.Persistence;
using TrackTrash.Infrastructure.Persistence.Repositories;

namespace TrackTrash.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TrackTrashDbContext _db;
    private readonly Dictionary<Type, object> _genericos = [];

    public UnitOfWork(TrackTrashDbContext db)
    {
        _db = db;
        Solicitudes = new SolicitudRecojoRepository(db);
        MovimientosPuntos = new MovimientoPuntosRepository(db);
        Beneficios = new BeneficioRepository(db);
        Catalogos = new CatalogoRepository(db);
        Contenedores = new ContenedorRepository(db);
        Historial = new HistoricoRepository(db);
        Incidencias = new IncidenciaRepository(db);
        Notificaciones = new NotificacionRepository(db);
        Perfiles = new PerfilRepository(db);
        Rutas = new RutaRepository(db);
    }

    public ISolicitudRecojoRepository Solicitudes { get; }
    public IMovimientoPuntosRepository MovimientosPuntos { get; }
    public IBeneficioRepository Beneficios { get; }
    public ICatalogoRepository Catalogos { get; }
    public IContenedorRepository Contenedores { get; }
    public IHistoricoRepository Historial { get; }
    public IIncidenciaRepository Incidencias { get; }
    public INotificacionRepository Notificaciones { get; }
    public IPerfilRepository Perfiles { get; }
    public IRutaRepository Rutas { get; }

    public IRepository<TEntity> Repositorio<TEntity>() where TEntity : class
    {
        if (_genericos.TryGetValue(typeof(TEntity), out var repositorio))
            return (IRepository<TEntity>)repositorio;

        var creado = new Repository<TEntity>(_db);
        _genericos[typeof(TEntity)] = creado;
        return creado;
    }

    public async Task GuardarCambiosAsync(CancellationToken ct)
    {
        try { await _db.SaveChangesAsync(ct); }
        catch (DbUpdateConcurrencyException) { throw new ConflictoPersistenciaException(); }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        { throw new ConflictoPersistenciaException(); }
    }
}
