using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Infrastructure.Persistence.Repositories;

public sealed class MovimientoPuntosRepository(TrackTrashDbContext db) : IMovimientoPuntosRepository
{
    public void Agregar(MovimientoPuntos movimiento) => db.MovimientosPuntos.Add(movimiento);
    public async Task<int> ObtenerTotalAsync(Guid usuarioId, CancellationToken ct) =>
        await db.MovimientosPuntos.Where(x => x.UsuarioId == usuarioId).SumAsync(x => (int?)x.Puntos, ct) ?? 0;
}
