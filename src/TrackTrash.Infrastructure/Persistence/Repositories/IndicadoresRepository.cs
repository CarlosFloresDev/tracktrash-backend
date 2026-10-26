using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Infrastructure.Persistence.Repositories;

public sealed class IndicadoresRepository(TrackTrashDbContext db) : IIndicadoresRepository
{
    public async Task<IndicadoresOperacion> ObtenerAsync(DateTimeOffset ahora, CancellationToken ct)
    {
        var limite = ahora.AddMinutes(-30);
        return new(await db.Contenedores.CountAsync(ct), await db.Contenedores.CountAsync(x => x.Porcentaje > 80 && x.ActualizadoEn >= limite, ct),
            await db.Incidencias.CountAsync(x => x.Estado != EstadoIncidencia.Resuelta, ct), await db.Solicitudes.CountAsync(x => x.Estado == EstadoSolicitud.Confirmada, ct),
            await db.Solicitudes.Where(x => x.Estado == EstadoSolicitud.Confirmada).SumAsync(x => (decimal?)x.CantidadEstimadaKg, ct) ?? 0);
    }
}
