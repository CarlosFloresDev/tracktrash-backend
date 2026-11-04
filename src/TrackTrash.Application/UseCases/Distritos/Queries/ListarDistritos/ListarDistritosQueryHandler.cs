using TrackTrash.Application.DTOs.Distritos;
using MediatR;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Distritos.Queries.ListarDistritos;

internal sealed class ListarDistritosQueryHandler(
    ICatalogoRepository repo) : IRequestHandler<ListarDistritosQuery, IReadOnlyList<DistritoDto>>
{
    public async Task<IReadOnlyList<DistritoDto>> Handle(ListarDistritosQuery request, CancellationToken cancellationToken) =>
        (await repo.DistritosAsync(cancellationToken)).Select(d => new DistritoDto(d.Id, d.Nombre)).ToList();
}
