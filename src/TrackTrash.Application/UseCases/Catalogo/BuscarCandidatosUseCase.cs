using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Application.UseCases.Catalogo;
public sealed class BuscarCandidatosUseCase(ICatalogoRepository catalogo)
{
    public async Task<IReadOnlyList<AsociacionDto>> ExecuteAsync(MaterialReciclable material,decimal kg,double lat,double lng,CancellationToken ct)
    {
        Ubicacion.Validar(lat,lng);
        if(!Enum.IsDefined(material)||kg<=0||kg>500)throw new Domain.Exceptions.ReglaNegocioException("Material o cantidad inválidos.");
        return (await catalogo.AsociacionesAsync(ct)).Where(a=>a.Verificada&&a.CapacidadKg>=kg&&a.Materiales.Any(m=>m.Material==material))
            .Select(a=>a.Dto(Ubicacion.DistanciaKm(lat,lng,a.Latitud,a.Longitud))).Where(a=>a.DistanciaKm<=20).OrderBy(a=>a.DistanciaKm).Take(10).ToList();
    }
}
