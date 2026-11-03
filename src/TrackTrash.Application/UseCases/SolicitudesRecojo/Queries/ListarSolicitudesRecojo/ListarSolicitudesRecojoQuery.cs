using MediatR;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Common;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Queries.ListarSolicitudesRecojo;

public sealed record ListarSolicitudesRecojoQuery : IRequest<IReadOnlyList<SolicitudDto>>
{
    public int Pagina { get; init; } = 1;
}
