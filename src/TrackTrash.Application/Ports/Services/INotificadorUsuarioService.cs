using TrackTrash.Domain.Entities;

namespace TrackTrash.Application.Ports.Services;

public interface INotificadorUsuarioService
{
    // Añade el aviso permitido a la unidad de trabajo actual, sin confirmar
    // por separado el proceso de negocio que lo originó.
    Task AgregarAsync(Notificacion notificacion, CancellationToken ct);
}
