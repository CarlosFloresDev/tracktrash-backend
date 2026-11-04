using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Infrastructure.Persistence.Repositories;

public sealed class MovimientoPuntosRepository : Repository<MovimientoPuntos>, IMovimientoPuntosRepository
{
    private readonly TrackTrashDbContext _db;

    public MovimientoPuntosRepository(TrackTrashDbContext db) : base(db) => _db = db;

    public async Task<int> OtorgadosEntreAsync(Guid usuario, DateTimeOffset inicio, DateTimeOffset fin, CancellationToken ct) => await _db.MovimientosPuntos.Where(m => m.UsuarioId == usuario && m.Puntos > 0 && m.CreadoEn >= inicio && m.CreadoEn < fin).SumAsync(m => (int?)m.Puntos, ct) ?? 0;
    public async Task<IReadOnlyList<MovimientoPuntos>> ListarAsync(Guid usuario, CancellationToken ct) => await _db.MovimientosPuntos.AsNoTracking().Where(m => m.UsuarioId == usuario).OrderByDescending(m => m.CreadoEn).Take(100).ToListAsync(ct);
    public async Task<int> ObtenerTotalAsync(Guid usuarioId, CancellationToken ct) =>
        await _db.MovimientosPuntos.Where(x => x.UsuarioId == usuarioId).SumAsync(x => (int?)x.Puntos, ct) ?? 0;
}
