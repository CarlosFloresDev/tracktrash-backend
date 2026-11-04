using TrackTrash.Application.DTOs.Rutas;
using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Rutas.Commands.ReportarPaso;

internal sealed class ReportarPasoCommandHandler(
    IRutaRepository repo,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork,
    TimeProvider reloj) : IRequestHandler<ReportarPasoCommand>
{
    public async Task Handle(ReportarPasoCommand request, CancellationToken cancellationToken)
    {
        _ = await repo.ObtenerAsync(request.RutaId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        var ahora = reloj.GetUtcNow();
        if (await repo.ReporteRecienteAsync(request.RutaId, usuario.Id, ahora.AddMinutes(-30), cancellationToken)) throw new Domain.Exceptions.ReglaNegocioException("Ya reportaste esta ruta recientemente.");
        repo.Agregar(ReportePasoCamion.Crear(request.RutaId, usuario.Id, ahora));
        await unitOfWork.GuardarCambiosAsync(cancellationToken);
    }
}
