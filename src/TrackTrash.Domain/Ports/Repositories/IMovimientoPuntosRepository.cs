using TrackTrash.Domain.Entities;

namespace TrackTrash.Domain.Ports.Repositories;

public interface IMovimientoPuntosRepository
{
    void Agregar(MovimientoPuntos movimiento);
    Task<int> ObtenerTotalAsync(Guid usuarioId, CancellationToken ct);
    Task<int> OtorgadosEntreAsync(Guid usuario, DateTimeOffset inicio, DateTimeOffset fin, CancellationToken ct);
    Task<IReadOnlyList<MovimientoPuntos>> ListarAsync(Guid usuario, CancellationToken ct);
}
