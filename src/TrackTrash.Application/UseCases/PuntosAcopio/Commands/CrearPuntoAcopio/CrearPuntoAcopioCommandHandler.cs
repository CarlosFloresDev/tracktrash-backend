using MediatR;
using TrackTrash.Domain.Entities;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.PuntosAcopio.Commands.CrearPuntoAcopio;

internal sealed class CrearPuntoAcopioCommandHandler(
    ICatalogoRepository repo,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork) : IRequestHandler<CrearPuntoAcopioCommand, Guid>
{
    public async Task<Guid> Handle(CrearPuntoAcopioCommand request, CancellationToken cancellationToken)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException();
        var punto = PuntoAcopio.Crear(request.Nombre, request.Direccion, request.Latitud, request.Longitud, false);
        repo.Agregar(punto);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);
        return punto.Id;
    }
}
