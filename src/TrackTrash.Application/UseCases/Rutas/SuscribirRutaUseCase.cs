using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Rutas;

public sealed class SuscribirRutaUseCase(IRutaRepository repo, IUsuarioActualService usuario, IUnitOfWork uow, TimeProvider reloj)
{
    public async Task ExecuteAsync(Guid id, int minutos, CancellationToken ct)
    {
        var ruta = await repo.ObtenerAsync(id, ct) ?? throw new RecursoNoEncontradoException();
        var suscripcion = await repo.SuscripcionAsync(id, usuario.Id, ct);
        if (suscripcion is null)
            repo.Agregar(SuscripcionRuta.Crear(usuario.Id, ruta, minutos, reloj.GetUtcNow()));
        else
            suscripcion.CambiarAntelacion(ruta, minutos, reloj.GetUtcNow());
        await uow.GuardarCambiosAsync(ct);
    }
}
