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

namespace TrackTrash.Application.UseCases.Asociaciones.Commands.CrearAsociacion;

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
        catalogo.Agregar(a); await unitOfWork.GuardarCambiosAsync(cancellationToken); return a.ToDto();
    }
}
