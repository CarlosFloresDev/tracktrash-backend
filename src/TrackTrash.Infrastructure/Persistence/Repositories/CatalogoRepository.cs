using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Infrastructure.Persistence.Repositories;

public sealed class CatalogoRepository(TrackTrashDbContext db) : ICatalogoRepository
{
    public async Task<IReadOnlyList<Distrito>> DistritosAsync(CancellationToken ct) => await db.Distritos.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync(ct);
    public async Task<IReadOnlyList<AsociacionReciclaje>> AsociacionesAsync(CancellationToken ct) => await db.Asociaciones.AsNoTracking().Include(x => x.Materiales).OrderBy(x => x.Nombre).Take(200).ToListAsync(ct);
    public Task<AsociacionReciclaje?> AsociacionAsync(Guid id, CancellationToken ct) => db.Asociaciones.Include(x => x.Materiales).SingleOrDefaultAsync(x => x.Id == id, ct);
    public async Task<IReadOnlyList<PuntoAcopio>> PuntosAsync(CancellationToken ct) => await db.PuntosAcopio.AsNoTracking().Take(200).ToListAsync(ct);
    public void Agregar(AsociacionReciclaje a) => db.Asociaciones.Add(a);
    public async Task<IReadOnlyList<AsociacionReciclaje>> CandidatosAsync(MaterialReciclable material, decimal kg, double lat, double lng, CancellationToken ct)
    {
        var punto = new Point(lng, lat) { SRID = 4326 };
        return await db.Asociaciones.AsNoTracking().Include(x => x.Materiales)
            .Where(x => x.Verificada && x.CapacidadKg - x.ReservadoKg >= kg && x.Materiales.Any(m => m.Material == material)
                && EF.Property<Point>(x, "Ubicacion").IsWithinDistance(punto, 20000))
            .OrderBy(x => EF.Property<Point>(x, "Ubicacion").Distance(punto)).Take(10).ToListAsync(ct);
    }
}
