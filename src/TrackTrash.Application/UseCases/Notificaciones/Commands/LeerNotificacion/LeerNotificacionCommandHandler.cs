using TrackTrash.Application.DTOs.Notificaciones;
using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Notificaciones.Commands.LeerNotificacion;

internal sealed class LeerNotificacionCommandHandler(
    INotificacionRepository repo,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork) : IRequestHandler<LeerNotificacionCommand>
{
    public async Task Handle(LeerNotificacionCommand request, CancellationToken cancellationToken)
    {
        var n = await repo.ObtenerAsync(request.NotificacionId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        if (n.UsuarioId != usuario.Id) throw new RecursoNoEncontradoException();
        n.Leer();
        await unitOfWork.GuardarCambiosAsync(cancellationToken);
    }
}
