using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Contenedores;

public sealed class CrearContenedorUseCase(IContenedorRepository repo, ICatalogoRepository catalogo, IUsuarioActualService usuario, IUnitOfWork uow, TimeProvider reloj)
{
    public async Task<ContenedorDto> ExecuteAsync(CrearContenedorRequest r, CancellationToken ct)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException();
        if (!(await catalogo.DistritosAsync(ct)).Any(d => d.Id == r.DistritoId)) throw new RecursoNoEncontradoException();
        var c = Contenedor.Crear(r.DistritoId, r.Nombre, r.Latitud, r.Longitud); repo.Agregar(c); await uow.GuardarCambiosAsync(ct); return c.Dto(reloj.GetUtcNow());
    }
}
