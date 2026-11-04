using MediatR;
using TrackTrash.Application.DTOs.Asociaciones;
using TrackTrash.Application.DTOs.Canjes;
using TrackTrash.Application.DTOs.Contenedores;
using TrackTrash.Application.DTOs.Incidencias;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Contenedores.Queries.ConsultarMapa;

internal sealed class ConsultarMapaQueryHandler(
    IContenedorRepository contenedores,
    ICatalogoRepository catalogo,
    TimeProvider reloj) : IRequestHandler<ConsultarMapaQuery, MapaDto>
{
    public async Task<MapaDto> Handle(ConsultarMapaQuery request, CancellationToken cancellationToken)
    {
        var lista = await contenedores.CercanosAsync(request.Latitud, request.Longitud, request.RadioKm, cancellationToken);
        var aliados = (await catalogo.AsociacionesAsync(cancellationToken)).Where(a => a.Verificada && Ubicacion.DistanciaKm(request.Latitud, request.Longitud, a.Latitud, a.Longitud) <= request.RadioKm).Select(a => a.ToDto()).ToList();
        var puntos = (await catalogo.PuntosAsync(cancellationToken)).Where(a => Ubicacion.DistanciaKm(request.Latitud, request.Longitud, a.Latitud, a.Longitud) <= request.RadioKm).Select(a => new PuntoAcopioDto(a.Id, a.Nombre, a.Direccion, a.Latitud, a.Longitud, a.Demostracion)).ToList();
        return new(lista.Select(c => c.ToDto(reloj.GetUtcNow())).ToList(), aliados, puntos);
    }
}
