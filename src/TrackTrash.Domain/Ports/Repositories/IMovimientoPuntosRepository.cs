using TrackTrash.Domain.Entities;

namespace TrackTrash.Domain.Ports.Repositories;

public interface IMovimientoPuntosRepository
{
    void Agregar(MovimientoPuntos movimiento);
    Task<int> ObtenerTotalAsync(Guid usuarioId, CancellationToken ct);
}
