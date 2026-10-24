using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Rutas;
public sealed class CancelarSuscripcionUseCase(IRutaRepository repo,IUsuarioActualService usuario,IUnitOfWork uow)
{
    public async Task ExecuteAsync(Guid id,CancellationToken ct){var s=await repo.SuscripcionAsync(id,usuario.Id,ct);if(s is not null){repo.Quitar(s);await uow.GuardarCambiosAsync(ct);}}
}
