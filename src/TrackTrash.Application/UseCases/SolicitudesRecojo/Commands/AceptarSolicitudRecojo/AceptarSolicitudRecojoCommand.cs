using MediatR;
using TrackTrash.Application.DTOs.SolicitudesRecojo;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.AceptarSolicitudRecojo;

public sealed record AceptarSolicitudRecojoCommand : IRequest<SolicitudDto>
{
    public Guid SolicitudId { get; init; }
}
