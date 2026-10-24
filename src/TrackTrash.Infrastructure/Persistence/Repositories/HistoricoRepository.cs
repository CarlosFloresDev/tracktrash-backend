using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Infrastructure.Persistence.Repositories;
public sealed class HistoricoRepository(TrackTrashDbContext db):IHistoricoRepository
{
    public void Agregar(HistoricoSolicitud e)=>db.Historial.Add(e);
    public async Task<IReadOnlyList<HistoricoSolicitud>> ListarAsync(Guid id,CancellationToken ct)=>await db.Historial.AsNoTracking().Where(x=>x.SolicitudId==id).OrderBy(x=>x.CreadoEn).ToListAsync(ct);
}
