using TrackTrash.Domain.Entities;

namespace TrackTrash.Domain.Ports.Repositories;

public interface ISolicitudRecojoRepository
{
    Task<SolicitudRecojo?> ObtenerAsync(Guid id, CancellationToken ct);
    Task<SolicitudRecojo?> ObtenerPorClaveAsync(Guid vecinoId, Guid clave, CancellationToken ct);
    Task<IReadOnlyList<SolicitudRecojo>> ListarAsync(Guid actorId, bool reciclador, int pagina, CancellationToken ct);
    void Agregar(SolicitudRecojo solicitud);
}
