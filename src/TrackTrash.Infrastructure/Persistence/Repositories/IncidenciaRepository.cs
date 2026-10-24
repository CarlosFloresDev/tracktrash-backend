using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Infrastructure.Persistence.Repositories;

public sealed class IncidenciaRepository(TrackTrashDbContext db) : IIncidenciaRepository
{
    public async Task<IReadOnlyList<Incidencia>> ListarAsync(Guid usuario, bool operador, CancellationToken ct) => await db.Incidencias.AsNoTracking().Where(x => operador || x.ReportadaPor == usuario).OrderByDescending(x => x.CreadaEn).Take(100).ToListAsync(ct);
    public Task<Incidencia?> ObtenerAsync(Guid id, CancellationToken ct) => db.Incidencias.SingleOrDefaultAsync(x => x.Id == id, ct);
    public Task<bool> SaturacionAbiertaAsync(Guid id, CancellationToken ct) => db.Incidencias.AnyAsync(x => x.ContenedorId == id && x.Tipo == "Saturación" && x.Estado != EstadoIncidencia.Resuelta, ct);
    public void Agregar(Incidencia i) => db.Incidencias.Add(i);
}
