using MediatR;
using TrackTrash.Application.DTOs.PuntosAcopio;
using TrackTrash.Application.Exceptions;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.PuntosAcopio.Queries.ObtenerPuntoAcopio;

internal sealed class ObtenerPuntoAcopioQueryHandler(
    ICatalogoRepository repo) : IRequestHandler<ObtenerPuntoAcopioQuery, PuntoAcopioDto>
{
    public async Task<PuntoAcopioDto> Handle(ObtenerPuntoAcopioQuery request, CancellationToken cancellationToken)
    {
        var p = await repo.PuntoAsync(request.Id, cancellationToken) ?? throw new RecursoNoEncontradoException();
        return new PuntoAcopioDto(p.Id, p.Nombre, p.Direccion, p.Latitud, p.Longitud, p.Demostracion);
    }
}
