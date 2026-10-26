using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Application.UseCases.Notificaciones;

public sealed class ListarNotificacionesUseCase(INotificacionRepository repo, IUsuarioActualService usuario)
{
    public async Task<IReadOnlyList<NotificacionDto>> ExecuteAsync(CancellationToken ct) => (await repo.ListarAsync(usuario.Id, ct)).Select(n => new NotificacionDto(n.Id, n.Titulo, n.Mensaje, n.CreadaEn, n.Leida, n.Tipo.ToString())).ToList();
}
