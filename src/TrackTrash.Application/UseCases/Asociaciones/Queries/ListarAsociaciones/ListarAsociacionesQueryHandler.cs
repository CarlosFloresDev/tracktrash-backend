using MediatR;
using TrackTrash.Application.DTOs.Asociaciones;
using TrackTrash.Application.DTOs.Canjes;
using TrackTrash.Application.DTOs.Contenedores;
using TrackTrash.Application.DTOs.Incidencias;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Asociaciones.Queries.ListarAsociaciones;

internal sealed class ListarAsociacionesQueryHandler(
    ICatalogoRepository catalogo) : IRequestHandler<ListarAsociacionesQuery, IReadOnlyList<AsociacionDto>>
{
    public async Task<IReadOnlyList<AsociacionDto>> Handle(ListarAsociacionesQuery request, CancellationToken cancellationToken) =>
        (await catalogo.AsociacionesAsync(cancellationToken)).Select(a => a.ToDto()).ToList();
}
