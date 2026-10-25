using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Incidencias;

public sealed class CrearIncidenciaUseCase(IIncidenciaRepository repo, IContenedorRepository contenedores, INotificadorUsuarioService avisos, IUsuarioActualService usuario, IUnitOfWork uow, TimeProvider reloj)
{
    public async Task<IncidenciaDto> ExecuteAsync(CrearIncidenciaRequest r, CancellationToken ct)
    {
        _ = await contenedores.ObtenerAsync(r.ContenedorId, ct) ?? throw new RecursoNoEncontradoException();
        var i = Incidencia.Crear(r.ContenedorId, usuario.Id, r.Tipo, r.Descripcion, reloj.GetUtcNow()); repo.Agregar(i);
        await avisos.AgregarAsync(Notificacion.Crear(usuario.Id, $"incidencia:{i.Id}", "Recibimos tu reporte", "Puedes seguir su atención en Mis reportes.", reloj.GetUtcNow(), TipoAviso.Incidencias), ct);
        await uow.GuardarCambiosAsync(ct); return i.Dto();
    }
}
