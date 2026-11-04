using Microsoft.EntityFrameworkCore;
using TrackTrash.Application.DTOs.EventosAuditoria;
using TrackTrash.Application.Ports.Queries;

namespace TrackTrash.Infrastructure.Persistence.Queries;

public sealed class AuditoriaConsulta(TrackTrashDbContext db) : IAuditoriaConsulta
{
    public async Task<IReadOnlyList<EventoAuditoriaDto>> ListarAsync(string? entidad, int maximo, CancellationToken ct) =>
        await db.EventosAuditoria.AsNoTracking()
            .Where(e => entidad == null || e.Entidad == entidad)
            .OrderByDescending(e => e.CreadoEn).ThenBy(e => e.Id).Take(maximo)
            .Select(e => new EventoAuditoriaDto(e.Id, e.Actor, e.Entidad, e.Referencia, e.Accion, e.Campos, e.CreadoEn))
            .ToListAsync(ct);
}
