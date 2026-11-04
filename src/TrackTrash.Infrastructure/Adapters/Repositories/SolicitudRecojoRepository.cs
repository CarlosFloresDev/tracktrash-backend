using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Infrastructure.Persistence.Repositories;

public sealed class SolicitudRecojoRepository : Repository<SolicitudRecojo>, ISolicitudRecojoRepository
{
    private readonly TrackTrashDbContext _db;

    public SolicitudRecojoRepository(TrackTrashDbContext db) : base(db) => _db = db;

    public Task<SolicitudRecojo?> ObtenerAsync(Guid id, CancellationToken ct) =>
        _db.Solicitudes.SingleOrDefaultAsync(x => x.Id == id, ct);

    public Task<SolicitudRecojo?> ObtenerPorClaveAsync(Guid vecinoId, Guid clave, CancellationToken ct) =>
        _db.Solicitudes.SingleOrDefaultAsync(x => x.VecinoId == vecinoId && x.ClaveSolicitud == clave, ct);

    public async Task<IReadOnlyList<SolicitudRecojo>> ListarAsync(Guid actorId, bool reciclador, int pagina, CancellationToken ct) =>
        await _db.Solicitudes.AsNoTracking()
            .Where(x => reciclador
                ? (x.Estado == EstadoSolicitud.Pendiente && x.VecinoId != actorId) || x.RecicladorId == actorId
                : x.VecinoId == actorId)
            .OrderByDescending(x => x.CreadaEn).ThenBy(x => x.Id)
            .Skip((pagina - 1) * 50).Take(50).ToListAsync(ct);
}
