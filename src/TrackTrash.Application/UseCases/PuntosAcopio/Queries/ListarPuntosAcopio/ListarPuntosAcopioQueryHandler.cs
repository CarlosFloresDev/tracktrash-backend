using MediatR;
using TrackTrash.Application.DTOs.PuntosAcopio;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.PuntosAcopio.Queries.ListarPuntosAcopio;

internal sealed class ListarPuntosAcopioQueryHandler(
    ICatalogoRepository repo) : IRequestHandler<ListarPuntosAcopioQuery, IReadOnlyList<PuntoAcopioDto>>
{
    public async Task<IReadOnlyList<PuntoAcopioDto>> Handle(ListarPuntosAcopioQuery request, CancellationToken cancellationToken) =>
        (await repo.PuntosAsync(cancellationToken))
            .Select(p => new PuntoAcopioDto(p.Id, p.Nombre, p.Direccion, p.Latitud, p.Longitud, p.Demostracion)).ToList();
}
