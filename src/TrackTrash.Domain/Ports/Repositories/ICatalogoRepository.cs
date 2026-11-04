using TrackTrash.Domain.Entities;

namespace TrackTrash.Domain.Ports.Repositories;

public interface ICatalogoRepository
{
    Task<IReadOnlyList<Distrito>> DistritosAsync(CancellationToken ct);
    Task<IReadOnlyList<AsociacionReciclaje>> AsociacionesAsync(CancellationToken ct);
    Task<AsociacionReciclaje?> AsociacionAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<PuntoAcopio>> PuntosAsync(CancellationToken ct);
    Task<IReadOnlyList<AsociacionReciclaje>> CandidatosAsync(MaterialReciclable material, decimal kg,
        double lat, double lng, CancellationToken ct);
    Task<PuntoAcopio?> PuntoAsync(Guid id, CancellationToken ct);
    void Agregar(AsociacionReciclaje asociacion);
    void Agregar(PuntoAcopio punto);
}
