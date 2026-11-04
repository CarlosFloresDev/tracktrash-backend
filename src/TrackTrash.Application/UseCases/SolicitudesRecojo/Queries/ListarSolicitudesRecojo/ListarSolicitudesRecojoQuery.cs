using MediatR;
using TrackTrash.Application.DTOs.SolicitudesRecojo;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Queries.ListarSolicitudesRecojo;

public sealed record ListarSolicitudesRecojoQuery : IRequest<IReadOnlyList<SolicitudDto>>
{
    public int Pagina { get; init; }
}
