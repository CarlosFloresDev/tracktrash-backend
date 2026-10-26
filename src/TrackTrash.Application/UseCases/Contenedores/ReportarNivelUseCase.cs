using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Contenedores;

public sealed record ReportarNivelRequest(int Porcentaje);
public sealed class ReportarNivelUseCase(IContenedorRepository repo, IIncidenciaRepository incidencias, IUsuarioActualService usuario, IUnitOfWork uow, TimeProvider reloj)
{
    public async Task<ContenedorDto> ExecuteAsync(Guid id, int porcentaje, CancellationToken ct)
    {
        var c = await repo.ObtenerAsync(id, ct) ?? throw new RecursoNoEncontradoException(); var ahora = reloj.GetUtcNow();
        if (await repo.ReporteRecienteAsync(id, usuario.Id, ahora.AddMinutes(-5), ct)) throw new Domain.Exceptions.ReglaNegocioException("Ya reportaste este contenedor recientemente.");
        repo.Agregar(ReporteCiudadano.Crear(id, usuario.Id, porcentaje, ahora));
        if (c.Fuente != "Sensor IoT" || c.Estado(ahora) == "SinDatosRecientes") c.ActualizarNivel(porcentaje, ahora, "Reporte ciudadano no verificado");
        if (porcentaje > 80 && !await incidencias.SaturacionAbiertaAsync(id, ct)) incidencias.Agregar(Incidencia.Crear(id, usuario.Id, "Saturación", "Un vecino reportó un nivel de llenado superior al 80%.", ahora));
        await uow.GuardarCambiosAsync(ct); return c.Dto(ahora);
    }
}
