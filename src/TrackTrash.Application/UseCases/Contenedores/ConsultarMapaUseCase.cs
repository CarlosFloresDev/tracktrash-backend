using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Application.UseCases.Contenedores;

public sealed class ConsultarMapaUseCase(IContenedorRepository contenedores, ICatalogoRepository catalogo, TimeProvider reloj)
{
    public async Task<MapaDto> ExecuteAsync(double lat, double lng, double km, CancellationToken ct)
    {
        Ubicacion.Validar(lat, lng); if (!double.IsFinite(km) || km is < 0.1 or > 50) throw new Domain.Exceptions.ReglaNegocioException("Radio permitido: 0.1 a 50 km.");
        var lista = await contenedores.CercanosAsync(lat, lng, km, ct);
        var aliados = (await catalogo.AsociacionesAsync(ct)).Where(a => a.Verificada && Ubicacion.DistanciaKm(lat, lng, a.Latitud, a.Longitud) <= km).Select(a => a.Dto()).ToList();
        var puntos = (await catalogo.PuntosAsync(ct)).Where(a => Ubicacion.DistanciaKm(lat, lng, a.Latitud, a.Longitud) <= km).Select(a => new PuntoAcopioDto(a.Id, a.Nombre, a.Direccion, a.Latitud, a.Longitud, a.Demostracion)).ToList();
        return new(lista.Select(c => c.Dto(reloj.GetUtcNow())).ToList(), aliados, puntos);
    }
}
