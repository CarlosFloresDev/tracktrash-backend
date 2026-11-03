using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Infrastructure.Persistence.Repositories;

public sealed class NotificacionRepository(TrackTrashDbContext db) : INotificacionRepository
{
    public async Task<IReadOnlyList<Notificacion>> ListarAsync(Guid usuario, CancellationToken ct) => await db.Notificaciones.AsNoTracking().Where(x => x.UsuarioId == usuario).OrderByDescending(x => x.CreadaEn).Take(100).ToListAsync(ct);
    public Task<Notificacion?> ObtenerAsync(Guid id, CancellationToken ct) => db.Notificaciones.SingleOrDefaultAsync(x => x.Id == id, ct);
    public void Agregar(Notificacion n) => db.Notificaciones.Add(n);
}
