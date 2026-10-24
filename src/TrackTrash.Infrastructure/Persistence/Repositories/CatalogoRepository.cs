using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Infrastructure.Persistence.Repositories;
public sealed class CatalogoRepository(TrackTrashDbContext db):ICatalogoRepository
{
    public async Task<IReadOnlyList<Distrito>> DistritosAsync(CancellationToken ct)=>await db.Distritos.AsNoTracking().OrderBy(x=>x.Nombre).ToListAsync(ct);
    public async Task<IReadOnlyList<AsociacionReciclaje>> AsociacionesAsync(CancellationToken ct)=>await db.Asociaciones.AsNoTracking().Include(x=>x.Materiales).OrderBy(x=>x.Nombre).Take(200).ToListAsync(ct);
    public Task<AsociacionReciclaje?> AsociacionAsync(Guid id,CancellationToken ct)=>db.Asociaciones.Include(x=>x.Materiales).SingleOrDefaultAsync(x=>x.Id==id,ct);
    public async Task<IReadOnlyList<PuntoAcopio>> PuntosAsync(CancellationToken ct)=>await db.PuntosAcopio.AsNoTracking().Take(200).ToListAsync(ct);
    public void Agregar(AsociacionReciclaje a)=>db.Asociaciones.Add(a);
}
