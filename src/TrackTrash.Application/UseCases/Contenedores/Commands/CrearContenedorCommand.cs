using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Contenedores.Commands;

public sealed record CrearContenedorCommand : IRequest<ContenedorDto>
{
    public Guid DistritoId { get; set; }

    public string Nombre { get; set; } = null!;

    public double Latitud { get; set; }

    public double Longitud { get; set; }
}

internal sealed class CrearContenedorCommandHandler(
    IContenedorRepository repo,
    ICatalogoRepository catalogo,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork,
    TimeProvider reloj) : IRequestHandler<CrearContenedorCommand, ContenedorDto>
{
    public async Task<ContenedorDto> Handle(CrearContenedorCommand request, CancellationToken cancellationToken)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException();
        if (!(await catalogo.DistritosAsync(cancellationToken)).Any(d => d.Id == request.DistritoId)) throw new RecursoNoEncontradoException();
        var c = Contenedor.Crear(request.DistritoId, request.Nombre, request.Latitud, request.Longitud); repo.Agregar(c); await unitOfWork.GuardarCambiosAsync(cancellationToken); return c.Dto(reloj.GetUtcNow());
    }
}
