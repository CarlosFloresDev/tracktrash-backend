using MediatR;
using TrackTrash.Application.DTOs.SolicitudesRecojo;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Queries.ObtenerSolicitudRecojo;

public sealed record ObtenerSolicitudRecojoQuery : IRequest<SolicitudDto>
{
    public Guid SolicitudId { get; init; }
}
