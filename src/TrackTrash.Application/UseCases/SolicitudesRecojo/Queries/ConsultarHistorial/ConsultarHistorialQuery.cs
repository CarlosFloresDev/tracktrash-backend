using TrackTrash.Application.DTOs.SolicitudesRecojo;
using MediatR;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Queries.ConsultarHistorial;

public sealed record ConsultarHistorialQuery : IRequest<IReadOnlyList<HistoricoDto>>
{
    public Guid SolicitudId { get; init; }
}
