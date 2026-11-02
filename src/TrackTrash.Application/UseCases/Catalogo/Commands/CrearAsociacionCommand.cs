using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Catalogo.Commands;

public sealed record CrearAsociacionCommand : IRequest<AsociacionDto>
{
    public Guid DistritoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public double Latitud { get; set; }

    public double Longitud { get; set; }

    public decimal CapacidadKg { get; set; }

    public MaterialReciclable[]? Materiales { get; set; }
}

internal sealed class CrearAsociacionCommandHandler(
    ICatalogoRepository catalogo,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork) : IRequestHandler<CrearAsociacionCommand, AsociacionDto>
{
    public async Task<AsociacionDto> Handle(CrearAsociacionCommand request, CancellationToken cancellationToken)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException();
        if (!(await catalogo.DistritosAsync(cancellationToken)).Any(d => d.Id == request.DistritoId)) throw new RecursoNoEncontradoException();
        var a = AsociacionReciclaje.Crear(Guid.NewGuid(), request.DistritoId, request.Nombre, request.Direccion, request.Latitud, request.Longitud, request.CapacidadKg, request.Materiales ?? []);
        catalogo.Agregar(a); await unitOfWork.GuardarCambiosAsync(cancellationToken); return a.Dto();
    }
}
