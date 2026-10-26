using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Infrastructure.Persistence.Repositories;

public sealed class ContenedorRepository(TrackTrashDbContext db) : IContenedorRepository
{
    public async Task<IReadOnlyList<Contenedor>> CercanosAsync(double lat, double lng, double km, CancellationToken ct)
    {
        var p = new Point(lng, lat) { SRID = 4326 };
        return await db.Contenedores.AsNoTracking().Where(x => EF.Property<Point>(x, "Ubicacion").IsWithinDistance(p, km * 1000))
            .OrderBy(x => EF.Property<Point>(x, "Ubicacion").Distance(p)).Take(500).ToListAsync(ct);
    }
    public Task<Contenedor?> ObtenerAsync(Guid id, CancellationToken ct) => db.Contenedores.SingleOrDefaultAsync(x => x.Id == id, ct);
    public Task<DispositivoIoT?> DispositivoAsync(Guid id, CancellationToken ct) => db.Dispositivos.SingleOrDefaultAsync(x => x.Id == id, ct);
    public Task<bool> LecturaExisteAsync(Guid id, long secuencia, CancellationToken ct) => db.Lecturas.AnyAsync(x => x.DispositivoId == id && x.Secuencia == secuencia, ct);
    public void Agregar(Contenedor c) => db.Contenedores.Add(c);
    public void Agregar(DispositivoIoT d) => db.Dispositivos.Add(d);
    public void Agregar(LecturaNivel l) => db.Lecturas.Add(l);
    public void Agregar(ReporteCiudadano r) => db.Add(r);
    public Task<bool> ReporteRecienteAsync(Guid contenedor, Guid usuario, DateTimeOffset desde, CancellationToken ct) => db.Set<ReporteCiudadano>().AnyAsync(r => r.ContenedorId == contenedor && r.UsuarioId == usuario && r.CreadoEn >= desde, ct);
}
