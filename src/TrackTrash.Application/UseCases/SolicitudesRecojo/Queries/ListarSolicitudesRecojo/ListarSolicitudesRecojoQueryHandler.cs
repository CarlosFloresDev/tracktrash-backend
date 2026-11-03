using MediatR;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Common;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Queries.ListarSolicitudesRecojo;

internal sealed class ListarSolicitudesRecojoQueryHandler(ISolicitudRecojoRepository solicitudes, IUsuarioActualService usuario)
    : IRequestHandler<ListarSolicitudesRecojoQuery, IReadOnlyList<SolicitudDto>>
{
    public async Task<IReadOnlyList<SolicitudDto>> Handle(ListarSolicitudesRecojoQuery request, CancellationToken ct)
    {
        var lista = await solicitudes.ListarAsync(usuario.Id, usuario.EsReciclador, request.Pagina, ct);
        return lista.Select(s => SolicitudDto.Desde(s, usuario.Id)).ToList();
    }
}
