using MediatR;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Common;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Queries.ObtenerSolicitudRecojo;

public sealed record ObtenerSolicitudRecojoQuery : IRequest<SolicitudDto>
{
    public required Guid Id { get; init; }
}
