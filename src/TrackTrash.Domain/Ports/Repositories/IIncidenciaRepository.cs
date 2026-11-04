using TrackTrash.Domain.Entities;

namespace TrackTrash.Domain.Ports.Repositories;

public interface IIncidenciaRepository
{
    Task<IReadOnlyList<Incidencia>> ListarAsync(Guid usuarioId, bool operador, CancellationToken ct);
    Task<Incidencia?> ObtenerAsync(Guid id, CancellationToken ct);
    Task<bool> SaturacionAbiertaAsync(Guid contenedorId, CancellationToken ct);
    void Agregar(Incidencia incidencia);
}
