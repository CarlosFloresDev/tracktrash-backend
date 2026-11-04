using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Infrastructure.Adapters;

public sealed class NotificadorUsuarioService(INotificacionRepository notificaciones) : INotificadorUsuarioService
{
    public Task AgregarAsync(Notificacion notificacion, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        notificaciones.Agregar(notificacion);
        return Task.CompletedTask;
    }
}
