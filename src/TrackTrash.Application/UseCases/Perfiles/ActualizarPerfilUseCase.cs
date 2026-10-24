using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Perfiles;

public sealed class ActualizarPerfilUseCase(IPerfilRepository perfiles, IUsuarioActualService usuario, IUnitOfWork uow)
{
    public async Task ExecuteAsync(ActualizarPerfilRequest r, CancellationToken ct)
    {
        var p = await perfiles.ObtenerAsync(usuario.Id, ct);
        if (p is null) { p = PerfilUsuario.Crear(usuario.Id); perfiles.Agregar(p); }
        p.Actualizar(r.Nombre, r.Telefono ?? "", r.Direccion ?? "", r.Latitud, r.Longitud, r.RecibirAvisos);
        await uow.GuardarCambiosAsync(ct);
    }
}
