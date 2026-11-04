using TrackTrash.Domain.Entities;

namespace TrackTrash.Domain.Ports.Repositories;

public interface IPerfilRepository
{
    Task<PerfilUsuario?> ObtenerAsync(Guid id, CancellationToken ct);
    void Agregar(PerfilUsuario perfil);
}
