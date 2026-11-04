using MediatR;
using TrackTrash.Application.DTOs.SolicitudesRecojo;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.CancelarSolicitud;

public sealed record CancelarSolicitudCommand : IRequest<SolicitudDto>
{
    public Guid SolicitudId { get; init; }

    public string Motivo { get; init; } = null!;
}
