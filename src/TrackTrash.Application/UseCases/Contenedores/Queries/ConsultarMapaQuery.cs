using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Contenedores.Queries;

public sealed record ConsultarMapaQuery : IRequest<MapaDto>
{
    public double Latitud { get; set; }

    public double Longitud { get; set; }

    public double RadioKm { get; set; }
}

internal sealed class ConsultarMapaQueryHandler(
    IContenedorRepository contenedores,
    ICatalogoRepository catalogo,
    TimeProvider reloj) : IRequestHandler<ConsultarMapaQuery, MapaDto>
{
    public async Task<MapaDto> Handle(ConsultarMapaQuery request, CancellationToken cancellationToken)
    {
        Ubicacion.Validar(request.Latitud, request.Longitud); if (!double.IsFinite(request.RadioKm) || request.RadioKm is < 0.1 or > 50) throw new Domain.Exceptions.ReglaNegocioException("Radio permitido: 0.1 a 50 request.RadioKm.");
        var lista = await contenedores.CercanosAsync(request.Latitud, request.Longitud, request.RadioKm, cancellationToken);
        var aliados = (await catalogo.AsociacionesAsync(cancellationToken)).Where(a => a.Verificada && Ubicacion.DistanciaKm(request.Latitud, request.Longitud, a.Latitud, a.Longitud) <= request.RadioKm).Select(a => a.Dto()).ToList();
        var puntos = (await catalogo.PuntosAsync(cancellationToken)).Where(a => Ubicacion.DistanciaKm(request.Latitud, request.Longitud, a.Latitud, a.Longitud) <= request.RadioKm).Select(a => new PuntoAcopioDto(a.Id, a.Nombre, a.Direccion, a.Latitud, a.Longitud, a.Demostracion)).ToList();
        return new(lista.Select(c => c.Dto(reloj.GetUtcNow())).ToList(), aliados, puntos);
    }
}
