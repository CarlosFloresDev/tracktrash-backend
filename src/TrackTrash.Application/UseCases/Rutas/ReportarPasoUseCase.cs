using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Rutas;
public sealed class ReportarPasoUseCase(IRutaRepository repo,IUsuarioActualService usuario,IUnitOfWork uow,TimeProvider reloj)
{
    public async Task ExecuteAsync(Guid id,CancellationToken ct)
    {_=await repo.ObtenerAsync(id,ct)??throw new RecursoNoEncontradoException();var ahora=reloj.GetUtcNow();if(await repo.ReporteRecienteAsync(id,usuario.Id,ahora.AddMinutes(-30),ct))throw new Domain.Exceptions.ReglaNegocioException("Ya reportaste esta ruta recientemente.");repo.Agregar(ReportePasoCamion.Crear(id,usuario.Id,ahora));await uow.GuardarCambiosAsync(ct);}
}
