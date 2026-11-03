using MediatR;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Common;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.AceptarSolicitudRecojo;

public sealed record AceptarSolicitudRecojoCommand : IRequest<SolicitudDto>
{
    public required Guid Id { get; init; }
}
