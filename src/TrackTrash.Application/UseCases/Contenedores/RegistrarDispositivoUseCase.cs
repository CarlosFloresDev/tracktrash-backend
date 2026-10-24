using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Contenedores;

public sealed class RegistrarDispositivoUseCase(IContenedorRepository repo, IUsuarioActualService usuario, IUnitOfWork uow, ISeguridadDispositivoService seguridad)
{
    public async Task<DispositivoRegistradoDto> ExecuteAsync(RegistrarDispositivoRequest r, CancellationToken ct)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException();
        _ = await repo.ObtenerAsync(r.ContenedorId, ct) ?? throw new RecursoNoEncontradoException();
        var clave = seguridad.Generar(); var d = DispositivoIoT.Crear(r.ContenedorId, seguridad.Hash(clave), r.AlturaCm);
        repo.Agregar(d); await uow.GuardarCambiosAsync(ct); return new(d.Id, d.ContenedorId, clave);
    }
}
