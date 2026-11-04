using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Infrastructure.Persistence.Repositories;

public sealed class CatalogoRepository(TrackTrashDbContext db) : ICatalogoRepository
{
    public async Task<IReadOnlyList<Distrito>> DistritosAsync(CancellationToken ct) => await db.Distritos.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync(ct);
    public async Task<IReadOnlyList<AsociacionReciclaje>> AsociacionesAsync(CancellationToken ct) => await db.Asociaciones.AsNoTracking().Include(x => x.Materiales).OrderBy(x => x.Nombre).Take(200).ToListAsync(ct);
    public Task<AsociacionReciclaje?> AsociacionAsync(Guid id, CancellationToken ct) => db.Asociaciones.Include(x => x.Materiales).SingleOrDefaultAsync(x => x.Id == id, ct);
    public async Task<IReadOnlyList<PuntoAcopio>> PuntosAsync(CancellationToken ct) => await db.PuntosAcopio.AsNoTracking().OrderBy(p => p.Nombre).ThenBy(p => p.Id).Take(200).ToListAsync(ct);
    public Task<PuntoAcopio?> PuntoAsync(Guid id, CancellationToken ct) => db.PuntosAcopio.SingleOrDefaultAsync(p => p.Id == id, ct);
    public void Agregar(AsociacionReciclaje a) => db.Asociaciones.Add(a);
    public void Agregar(PuntoAcopio p) => db.PuntosAcopio.Add(p);
    public async Task<IReadOnlyList<AsociacionReciclaje>> CandidatosAsync(MaterialReciclable material, decimal kg, double lat, double lng, CancellationToken ct)
    {
        const double margen = 0.25; // Prefiltro aproximado; la distancia exacta se calcula después.
        var candidatas = await db.Asociaciones.AsNoTracking().Include(x => x.Materiales)
            .Where(x => x.Verificada && x.CapacidadKg - x.ReservadoKg >= kg && x.Materiales.Any(m => m.Material == material)
                && x.Latitud >= lat - margen && x.Latitud <= lat + margen
                && x.Longitud >= lng - margen && x.Longitud <= lng + margen)
            .Take(200).ToListAsync(ct);
        return candidatas.Where(x => Ubicacion.DistanciaKm(lat, lng, x.Latitud, x.Longitud) <= 20)
            .OrderBy(x => Ubicacion.DistanciaKm(lat, lng, x.Latitud, x.Longitud)).Take(10).ToList();
    }
}
