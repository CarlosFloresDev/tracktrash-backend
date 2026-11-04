using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.DTOs.Asociaciones;
using TrackTrash.Application.DTOs.Canjes;
using TrackTrash.Application.DTOs.Contenedores;
using TrackTrash.Application.DTOs.Incidencias;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Contenedores.Commands.ReportarNivel;

internal sealed class ReportarNivelCommandHandler(
    IContenedorRepository repo,
    IIncidenciaRepository incidencias,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork,
    TimeProvider reloj) : IRequestHandler<ReportarNivelCommand, ContenedorDto>
{
    public async Task<ContenedorDto> Handle(ReportarNivelCommand request, CancellationToken cancellationToken)
    {
        var c = await repo.ObtenerAsync(request.ContenedorId, cancellationToken) ?? throw new RecursoNoEncontradoException(); var ahora = reloj.GetUtcNow();
        if (await repo.ReporteRecienteAsync(request.ContenedorId, usuario.Id, ahora.AddMinutes(-5), cancellationToken)) throw new Domain.Exceptions.ReglaNegocioException("Ya reportaste este contenedor recientemente.");
        repo.Agregar(ReporteCiudadano.Crear(request.ContenedorId, usuario.Id, request.Porcentaje, ahora));
        if (c.Fuente != "Sensor IoT" || c.Estado(ahora) == "SinDatosRecientes") c.ActualizarNivel(request.Porcentaje, ahora, "Reporte ciudadano no verificado");
        if (request.Porcentaje > 80 && !await incidencias.SaturacionAbiertaAsync(request.ContenedorId, cancellationToken)) incidencias.Agregar(Incidencia.Crear(request.ContenedorId, usuario.Id, "Saturación", "Un vecino reportó un nivel de llenado superior al 80%.", ahora));
        await unitOfWork.GuardarCambiosAsync(cancellationToken); return c.ToDto(ahora);
    }
}
