namespace TrackTrash.Domain.Ports.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> ObtenerPorIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<TEntity>> ListarAsync(CancellationToken ct);
    void Agregar(TEntity entity);
    void Actualizar(TEntity entity);
    void Eliminar(TEntity entity);
}
