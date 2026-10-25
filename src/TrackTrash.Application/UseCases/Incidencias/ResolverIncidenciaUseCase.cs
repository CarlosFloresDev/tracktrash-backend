using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Incidencias;

public sealed class ResolverIncidenciaUseCase(IIncidenciaRepository repo, INotificadorUsuarioService avisos, IUsuarioActualService usuario, IUnitOfWork uow, TimeProvider reloj)
{
    public async Task ExecuteAsync(Guid id, string detalle, CancellationToken ct)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException(); var i = await repo.ObtenerAsync(id, ct) ?? throw new RecursoNoEncontradoException();
        i.Resolver(usuario.Id, detalle, reloj.GetUtcNow());
        if (i.ReportadaPor != Guid.Empty) await avisos.AgregarAsync(Notificacion.Crear(i.ReportadaPor, $"resuelta:{id}", "Tu reporte fue atendido", detalle, reloj.GetUtcNow(), TipoAviso.Incidencias), ct);
        await uow.GuardarCambiosAsync(ct);
    }
}
