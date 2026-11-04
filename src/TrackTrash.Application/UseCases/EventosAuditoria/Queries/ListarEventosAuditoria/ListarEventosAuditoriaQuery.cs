using MediatR;
using TrackTrash.Application.DTOs.EventosAuditoria;

namespace TrackTrash.Application.UseCases.EventosAuditoria.Queries.ListarEventosAuditoria;

public sealed record ListarEventosAuditoriaQuery : IRequest<IReadOnlyList<EventoAuditoriaDto>>
{
    public string? Entidad { get; init; }
    public int Maximo { get; init; } = 50;
}
