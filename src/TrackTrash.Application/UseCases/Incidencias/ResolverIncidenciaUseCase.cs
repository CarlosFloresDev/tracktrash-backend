using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Incidencias;

public sealed class ResolverIncidenciaUseCase(IIncidenciaRepository repo, INotificacionRepository avisos, IUsuarioActualService usuario, IUnitOfWork uow, TimeProvider reloj)
{
    public async Task ExecuteAsync(Guid id, string detalle, CancellationToken ct)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException(); var i = await repo.ObtenerAsync(id, ct) ?? throw new RecursoNoEncontradoException();
        i.Resolver(usuario.Id, detalle, reloj.GetUtcNow());
        if (i.ReportadaPor != Guid.Empty) avisos.Agregar(Notificacion.Crear(i.ReportadaPor, $"resuelta:{id}", "Tu reporte fue atendido", detalle, reloj.GetUtcNow()));
        await uow.GuardarCambiosAsync(ct);
    }
}
