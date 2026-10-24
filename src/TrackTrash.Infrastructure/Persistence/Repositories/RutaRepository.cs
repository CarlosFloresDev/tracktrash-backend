using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Infrastructure.Persistence.Repositories;
public sealed class RutaRepository(TrackTrashDbContext db):IRutaRepository
{
    public async Task<IReadOnlyList<RutaRecoleccion>> ListarAsync(CancellationToken ct)=>await db.Set<RutaRecoleccion>().AsNoTracking().OrderBy(r=>r.Nombre).Take(100).ToListAsync(ct);
    public Task<RutaRecoleccion?> ObtenerAsync(Guid id,CancellationToken ct)=>db.Set<RutaRecoleccion>().SingleOrDefaultAsync(r=>r.Id==id,ct);
    public Task<SuscripcionRuta?> SuscripcionAsync(Guid ruta,Guid usuario,CancellationToken ct)=>db.Set<SuscripcionRuta>().SingleOrDefaultAsync(s=>s.RutaId==ruta&&s.UsuarioId==usuario,ct);
    public async Task<IReadOnlyList<SuscripcionRuta>> SuscripcionesAsync(Guid usuario,CancellationToken ct)=>await db.Set<SuscripcionRuta>().AsNoTracking().Where(s=>s.UsuarioId==usuario).ToListAsync(ct);
    public async Task<IReadOnlyList<SuscripcionRuta>> PendientesAsync(DateTimeOffset ahora,CancellationToken ct)=>await db.Set<SuscripcionRuta>().Where(s=>s.ProximoAviso<=ahora).Take(100).ToListAsync(ct);
    public Task<bool> ReporteRecienteAsync(Guid ruta,Guid usuario,DateTimeOffset desde,CancellationToken ct)=>db.Set<ReportePasoCamion>().AnyAsync(r=>r.RutaId==ruta&&r.UsuarioId==usuario&&r.ReportadoEn>=desde,ct);
    public void Agregar(SuscripcionRuta s)=>db.Add(s);
    public void Agregar(ReportePasoCamion r)=>db.Add(r);
    public void Quitar(SuscripcionRuta s)=>db.Remove(s);
}
