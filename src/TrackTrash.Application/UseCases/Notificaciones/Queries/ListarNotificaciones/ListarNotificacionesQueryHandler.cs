using TrackTrash.Application.DTOs.Notificaciones;
using MediatR;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Notificaciones.Queries.ListarNotificaciones;

internal sealed class ListarNotificacionesQueryHandler(
    INotificacionRepository repo,
    IUsuarioActualService usuario) : IRequestHandler<ListarNotificacionesQuery, IReadOnlyList<NotificacionDto>>
{
    public async Task<IReadOnlyList<NotificacionDto>> Handle(ListarNotificacionesQuery request, CancellationToken cancellationToken) =>
        (await repo.ListarAsync(usuario.Id, cancellationToken)).Select(n => new NotificacionDto(n.Id, n.Titulo, n.Mensaje, n.CreadaEn, n.Leida, n.Tipo.ToString())).ToList();
}
