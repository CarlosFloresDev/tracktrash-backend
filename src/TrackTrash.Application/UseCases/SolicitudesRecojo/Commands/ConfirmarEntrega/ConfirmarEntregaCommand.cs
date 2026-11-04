using MediatR;
using TrackTrash.Application.DTOs.SolicitudesRecojo;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.ConfirmarEntrega;

public sealed record ConfirmarEntregaCommand : IRequest<SolicitudDto>
{
    public Guid SolicitudId { get; init; }

    public string Codigo { get; init; } = null!;
}
