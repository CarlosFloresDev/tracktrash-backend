using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Catalogo.Queries;

public sealed record ConsultarAliadosQuery : IRequest<IReadOnlyList<AsociacionDto>>;

internal sealed class ConsultarAliadosQueryHandler(
    ICatalogoRepository catalogo) : IRequestHandler<ConsultarAliadosQuery, IReadOnlyList<AsociacionDto>>
{
    public async Task<IReadOnlyList<AsociacionDto>> Handle(ConsultarAliadosQuery request, CancellationToken cancellationToken) =>
        (await catalogo.AsociacionesAsync(cancellationToken)).Select(a => a.Dto()).ToList();
}
