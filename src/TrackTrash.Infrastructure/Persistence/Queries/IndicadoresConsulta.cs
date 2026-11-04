using Microsoft.EntityFrameworkCore;
using TrackTrash.Application.Ports.Queries;
using TrackTrash.Application.DTOs.Indicadores;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Infrastructure.Persistence.Queries;

public sealed class IndicadoresConsulta(TrackTrashDbContext db) : IIndicadoresConsulta
{
    public async Task<IndicadoresDto> ObtenerAsync(DateTimeOffset ahora, CancellationToken ct)
    {
        var recientes = ahora.AddMinutes(-30);
        var contenedores = await db.Contenedores.CountAsync(ct);
        var saturados = await db.Contenedores.CountAsync(x => x.ActualizadoEn >= recientes && x.Porcentaje > 80, ct);
        var incidencias = await db.Incidencias.CountAsync(x => x.Estado != EstadoIncidencia.Resuelta, ct);
        var confirmados = await db.Solicitudes.CountAsync(x => x.Estado == EstadoSolicitud.Confirmada, ct);
        var kg = await db.Solicitudes.Where(x => x.Estado == EstadoSolicitud.Confirmada)
            .SumAsync(x => (decimal?)x.CantidadEstimadaKg, ct) ?? 0;
        return new IndicadoresDto(contenedores, saturados, incidencias, confirmados, kg);
    }
}
