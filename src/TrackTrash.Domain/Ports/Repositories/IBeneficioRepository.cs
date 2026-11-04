using TrackTrash.Domain.Entities;

namespace TrackTrash.Domain.Ports.Repositories;

public interface IBeneficioRepository
{
    Task<IReadOnlyList<Beneficio>> ListarAsync(CancellationToken ct);
    Task<Beneficio?> ObtenerAsync(Guid id, CancellationToken ct);
    Task<Canje?> PorClaveAsync(Guid usuario, Guid clave, CancellationToken ct);
    Task<IReadOnlyList<Canje>> CanjesAsync(Guid usuario, CancellationToken ct);
    void Agregar(Beneficio beneficio);
    void Agregar(Canje canje);
}
