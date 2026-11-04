using TrackTrash.Application.DTOs.Notificaciones;
using MediatR;

namespace TrackTrash.Application.UseCases.Notificaciones.Commands.LeerNotificacion;

public sealed record LeerNotificacionCommand : IRequest
{
    public Guid NotificacionId { get; init; }
}
