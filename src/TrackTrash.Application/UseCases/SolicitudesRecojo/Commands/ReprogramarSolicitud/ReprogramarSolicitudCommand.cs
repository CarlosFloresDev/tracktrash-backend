using MediatR;
using TrackTrash.Application.DTOs.SolicitudesRecojo;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.ReprogramarSolicitud;

public sealed record ReprogramarSolicitudCommand : IRequest<SolicitudDto>
{
    public Guid SolicitudId { get; init; }

    public DateTimeOffset ProgramadaPara { get; init; }
}
