using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Notificaciones;

public sealed class LeerNotificacionUseCase(INotificacionRepository repo, IUsuarioActualService usuario, IUnitOfWork uow)
{
    public async Task ExecuteAsync(Guid id, CancellationToken ct)
    { var n = await repo.ObtenerAsync(id, ct) ?? throw new RecursoNoEncontradoException(); if (n.UsuarioId != usuario.Id) throw new RecursoNoEncontradoException(); n.Leer(); await uow.GuardarCambiosAsync(ct); }
}
