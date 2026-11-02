using System.ComponentModel.DataAnnotations;
using MediatR;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Queries;

public sealed record ListarSolicitudesRecojoQuery : IRequest<IReadOnlyList<SolicitudDto>>
{
    [Range(1, 10000)]
    public int Pagina { get; set; }
}

internal sealed class ListarSolicitudesRecojoQueryHandler(
    ISolicitudRecojoRepository solicitudes,
    IUsuarioActualService usuario) : IRequestHandler<ListarSolicitudesRecojoQuery, IReadOnlyList<SolicitudDto>>
{
    public async Task<IReadOnlyList<SolicitudDto>> Handle(ListarSolicitudesRecojoQuery request, CancellationToken cancellationToken)
    {
        if (request.Pagina < 1 || request.Pagina > 10000) throw new Domain.Exceptions.ReglaNegocioException("La página debe estar entre 1 y 10000.");
        var lista = await solicitudes.ListarAsync(usuario.Id, usuario.EsReciclador, request.Pagina, cancellationToken);
        return lista.Select(s => SolicitudDto.Desde(s, usuario.Id)).ToList();
    }
}
