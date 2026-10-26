using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Catalogo;

public sealed class CrearAsociacionUseCase(ICatalogoRepository catalogo, IUsuarioActualService usuario, IUnitOfWork uow)
{
    public async Task<AsociacionDto> ExecuteAsync(CrearAsociacionRequest r, CancellationToken ct)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException();
        if (!(await catalogo.DistritosAsync(ct)).Any(d => d.Id == r.DistritoId)) throw new RecursoNoEncontradoException();
        var a = AsociacionReciclaje.Crear(Guid.NewGuid(), r.DistritoId, r.Nombre, r.Direccion, r.Latitud, r.Longitud, r.CapacidadKg, r.Materiales ?? []);
        catalogo.Agregar(a); await uow.GuardarCambiosAsync(ct); return a.Dto();
    }
}
