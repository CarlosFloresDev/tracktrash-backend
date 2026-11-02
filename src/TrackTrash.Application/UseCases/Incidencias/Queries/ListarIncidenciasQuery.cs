using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Incidencias.Queries;

public sealed record ListarIncidenciasQuery : IRequest<IReadOnlyList<IncidenciaDto>>;

internal sealed class ListarIncidenciasQueryHandler(
    IIncidenciaRepository repo,
    IUsuarioActualService usuario) : IRequestHandler<ListarIncidenciasQuery, IReadOnlyList<IncidenciaDto>>
{
    public async Task<IReadOnlyList<IncidenciaDto>> Handle(ListarIncidenciasQuery request, CancellationToken cancellationToken) =>
        (await repo.ListarAsync(usuario.Id, usuario.EsAdministrador, cancellationToken)).Select(i => i.Dto()).ToList();
}
