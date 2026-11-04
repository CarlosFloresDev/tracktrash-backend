using MediatR;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.DTOs.Asociaciones;
using TrackTrash.Application.DTOs.Canjes;
using TrackTrash.Application.DTOs.Contenedores;
using TrackTrash.Application.DTOs.Incidencias;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Incidencias.Queries.ListarIncidencias;

internal sealed class ListarIncidenciasQueryHandler(
    IIncidenciaRepository repo,
    IUsuarioActualService usuario) : IRequestHandler<ListarIncidenciasQuery, IReadOnlyList<IncidenciaDto>>
{
    public async Task<IReadOnlyList<IncidenciaDto>> Handle(ListarIncidenciasQuery request, CancellationToken cancellationToken) =>
        (await repo.ListarAsync(usuario.Id, usuario.EsAdministrador, cancellationToken)).Select(i => i.ToDto()).ToList();
}
