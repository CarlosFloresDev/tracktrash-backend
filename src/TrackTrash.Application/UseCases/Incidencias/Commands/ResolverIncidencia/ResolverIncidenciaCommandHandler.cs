using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Incidencias.Commands.ResolverIncidencia;

internal sealed class ResolverIncidenciaCommandHandler(
    IIncidenciaRepository repo,
    INotificadorUsuarioService avisos,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork,
    TimeProvider reloj) : IRequestHandler<ResolverIncidenciaCommand>
{
    public async Task Handle(ResolverIncidenciaCommand request, CancellationToken cancellationToken)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException(); var i = await repo.ObtenerAsync(request.IncidenciaId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        i.Resolver(usuario.Id, request.Resolucion, reloj.GetUtcNow());
        if (i.ReportadaPor != Guid.Empty) await avisos.AgregarAsync(Notificacion.Crear(i.ReportadaPor, $"resuelta:{request.IncidenciaId}", "Tu reporte fue atendido", request.Resolucion, reloj.GetUtcNow(), TipoAviso.Incidencias), cancellationToken);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);
    }
}
