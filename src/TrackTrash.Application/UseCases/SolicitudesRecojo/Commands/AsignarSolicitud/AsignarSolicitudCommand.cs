using MediatR;
using TrackTrash.Application.DTOs.SolicitudesRecojo;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.AsignarSolicitud;

public sealed record AsignarSolicitudCommand : IRequest<SolicitudDto>
{
    public Guid SolicitudId { get; init; }
}
