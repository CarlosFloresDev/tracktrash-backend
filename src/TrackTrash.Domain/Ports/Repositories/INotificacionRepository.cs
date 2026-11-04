using TrackTrash.Domain.Entities;

namespace TrackTrash.Domain.Ports.Repositories;

public interface INotificacionRepository
{
    Task<IReadOnlyList<Notificacion>> ListarAsync(Guid usuarioId, CancellationToken ct);
    Task<Notificacion?> ObtenerAsync(Guid id, CancellationToken ct);
    void Agregar(Notificacion notificacion);
}
