using TrackTrash.Domain.Entities;

namespace TrackTrash.Application.Ports.Services;

public interface INotificadorUsuarioService
{
    Task AgregarAsync(Notificacion notificacion, CancellationToken ct);
}
