using MediatR;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Common;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.ConfirmarEntrega;

public sealed record ConfirmarEntregaCommand : IRequest<SolicitudDto>
{
    public required Guid Id { get; init; }
    public required string Codigo { get; init; }
}
