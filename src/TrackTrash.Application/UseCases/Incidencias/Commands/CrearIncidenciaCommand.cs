using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Incidencias.Commands;

public sealed record CrearIncidenciaCommand : IRequest<IncidenciaDto>
{
    public Guid ContenedorId { get; set; }

    public string Tipo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;
}

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
        await unitOfWork.GuardarCambiosAsync(cancellationToken); return i.Dto();
    }
}
