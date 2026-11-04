using MediatR;
using TrackTrash.Application.DTOs.SolicitudesRecojo;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.IniciarViaje;

public sealed record IniciarViajeCommand : IRequest<SolicitudDto>
{
    public Guid SolicitudId { get; init; }
}
