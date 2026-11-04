using TrackTrash.Application.DTOs.Rutas;
using MediatR;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Rutas.Commands.CancelarSuscripcion;

internal sealed class CancelarSuscripcionCommandHandler(
    IRutaRepository repo,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork) : IRequestHandler<CancelarSuscripcionCommand>
{
    public async Task Handle(CancelarSuscripcionCommand request, CancellationToken cancellationToken)
    {
        var s = await repo.SuscripcionAsync(request.RutaId, usuario.Id, cancellationToken);
        if (s is not null) { repo.Quitar(s); await unitOfWork.GuardarCambiosAsync(cancellationToken); }
    }
}
