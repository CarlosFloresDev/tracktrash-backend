using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Infrastructure.Persistence;

namespace TrackTrash.Infrastructure.Persistence.Repositories;

public class Repository<TEntity>(TrackTrashDbContext db) : IRepository<TEntity>
    where TEntity : class
{
    public Task<TEntity?> ObtenerPorIdAsync(Guid id, CancellationToken ct) =>
        db.Set<TEntity>().FindAsync([id], ct).AsTask();

    public async Task<IReadOnlyList<TEntity>> ListarAsync(CancellationToken ct) =>
        await db.Set<TEntity>().AsNoTracking().ToListAsync(ct);

    public void Agregar(TEntity entity) => db.Set<TEntity>().Add(entity);

    public void Actualizar(TEntity entity) => db.Set<TEntity>().Update(entity);

    public void Eliminar(TEntity entity) => db.Set<TEntity>().Remove(entity);
}
