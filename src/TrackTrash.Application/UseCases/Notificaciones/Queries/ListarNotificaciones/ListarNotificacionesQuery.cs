using TrackTrash.Application.DTOs.Notificaciones;
using MediatR;

namespace TrackTrash.Application.UseCases.Notificaciones.Queries.ListarNotificaciones;

public sealed record ListarNotificacionesQuery : IRequest<IReadOnlyList<NotificacionDto>>;
