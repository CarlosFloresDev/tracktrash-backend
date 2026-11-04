using MediatR;
using TrackTrash.Application.DTOs.Asociaciones;
using TrackTrash.Application.DTOs.Canjes;
using TrackTrash.Application.DTOs.Contenedores;
using TrackTrash.Application.DTOs.Incidencias;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Asociaciones.Queries.BuscarCandidatos;

internal sealed class BuscarCandidatosQueryHandler(
    ICatalogoRepository catalogo) : IRequestHandler<BuscarCandidatosQuery, IReadOnlyList<AsociacionDto>>
{
    public async Task<IReadOnlyList<AsociacionDto>> Handle(BuscarCandidatosQuery request, CancellationToken cancellationToken)
    {
        return (await catalogo.CandidatosAsync(request.Material, request.Kg, request.Latitud, request.Longitud, cancellationToken)).Select(a => a.ToDto(Ubicacion.DistanciaKm(request.Latitud, request.Longitud, a.Latitud, a.Longitud))).ToList();
    }
}
