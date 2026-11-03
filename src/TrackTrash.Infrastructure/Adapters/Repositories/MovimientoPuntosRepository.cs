using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Infrastructure.Persistence.Repositories;

public sealed class MovimientoPuntosRepository(TrackTrashDbContext db) : IMovimientoPuntosRepository
{
    public void Agregar(MovimientoPuntos movimiento) => db.MovimientosPuntos.Add(movimiento);
    public async Task<int> OtorgadosEntreAsync(Guid usuario, DateTimeOffset inicio, DateTimeOffset fin, CancellationToken ct) => await db.MovimientosPuntos.Where(m => m.UsuarioId == usuario && m.Puntos > 0 && m.CreadoEn >= inicio && m.CreadoEn < fin).SumAsync(m => (int?)m.Puntos, ct) ?? 0;
    public async Task<IReadOnlyList<MovimientoPuntos>> ListarAsync(Guid usuario, CancellationToken ct) => await db.MovimientosPuntos.AsNoTracking().Where(m => m.UsuarioId == usuario).OrderByDescending(m => m.CreadoEn).Take(100).ToListAsync(ct);
    public async Task<int> ObtenerTotalAsync(Guid usuarioId, CancellationToken ct) =>
        await db.MovimientosPuntos.Where(x => x.UsuarioId == usuarioId).SumAsync(x => (int?)x.Puntos, ct) ?? 0;
}
