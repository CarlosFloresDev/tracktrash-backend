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

namespace TrackTrash.Application.UseCases.Incidencias.Commands.CrearIncidencia;

internal sealed class CrearIncidenciaCommandHandler(
    IIncidenciaRepository repo,
    IContenedorRepository contenedores,
    INotificadorUsuarioService avisos,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork,
    TimeProvider reloj) : IRequestHandler<CrearIncidenciaCommand, IncidenciaDto>
{
    public async Task<IncidenciaDto> Handle(CrearIncidenciaCommand request, CancellationToken cancellationToken)
    {
        _ = await contenedores.ObtenerAsync(request.ContenedorId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        var i = Incidencia.Crear(request.ContenedorId, usuario.Id, request.Tipo, request.Descripcion, reloj.GetUtcNow()); repo.Agregar(i);
        await avisos.AgregarAsync(Notificacion.Crear(usuario.Id, $"incidencia:{i.Id}", "Recibimos tu reporte", "Puedes seguir su atención en Mis reportes.", reloj.GetUtcNow(), TipoAviso.Incidencias), cancellationToken);
        await unitOfWork.GuardarCambiosAsync(cancellationToken); return i.ToDto();
    }
}
