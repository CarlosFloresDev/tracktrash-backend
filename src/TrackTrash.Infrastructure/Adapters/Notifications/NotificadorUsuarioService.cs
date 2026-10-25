using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Infrastructure.Adapters.Notifications;

public sealed class NotificadorUsuarioService(IPerfilRepository perfiles, INotificacionRepository avisos) : INotificadorUsuarioService
{
    public async Task AgregarAsync(Notificacion notificacion, CancellationToken ct)
    {
        var perfil = await perfiles.ObtenerAsync(notificacion.UsuarioId, ct);
        if (perfil is null || perfil.PermiteAviso(notificacion.Tipo))
            avisos.Agregar(notificacion);
    }
}
