using TrackTrash.Application.DTOs.Perfiles;
using MediatR;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Perfiles.Commands.ActualizarPerfil;

internal sealed class ActualizarPerfilCommandHandler(
    IPerfilRepository perfiles,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork) : IRequestHandler<ActualizarPerfilCommand>
{
    public async Task Handle(ActualizarPerfilCommand request, CancellationToken cancellationToken)
    {
        var p = await perfiles.ObtenerAsync(usuario.Id, cancellationToken);
        if (p is null) { p = PerfilUsuario.Crear(usuario.Id); perfiles.Agregar(p); }
        p.Actualizar(request.Nombre, request.Telefono ?? "", request.Direccion ?? "", request.Latitud, request.Longitud, request.RecibirAvisos);
        if (request.PreferenciasAvisos is { } preferencias)
            p.ActualizarPreferenciasAvisos(preferencias.Horarios, preferencias.Recojos, preferencias.Incidencias, preferencias.Recompensas);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);
    }
}
