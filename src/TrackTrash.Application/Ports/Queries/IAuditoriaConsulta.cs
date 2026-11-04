using TrackTrash.Application.DTOs.EventosAuditoria;

namespace TrackTrash.Application.Ports.Queries;

public interface IAuditoriaConsulta
{
    Task<IReadOnlyList<EventoAuditoriaDto>> ListarAsync(string? entidad, int maximo, CancellationToken ct);
}
