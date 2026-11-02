using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Catalogo.Queries;

public sealed record ListarDistritosQuery : IRequest<IReadOnlyList<DistritoDto>>;

internal sealed class ListarDistritosQueryHandler(
    ICatalogoRepository repo) : IRequestHandler<ListarDistritosQuery, IReadOnlyList<DistritoDto>>
{
    public async Task<IReadOnlyList<DistritoDto>> Handle(ListarDistritosQuery request, CancellationToken cancellationToken) =>
        (await repo.DistritosAsync(cancellationToken)).Select(d => new DistritoDto(d.Id, d.Nombre)).ToList();
}
