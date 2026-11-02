using System.ComponentModel.DataAnnotations;
using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Perfiles.Commands;

public sealed record ActualizarPerfilCommand : IRequest
{
    [Required]
    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public double? Latitud { get; set; }

    public double? Longitud { get; set; }

    public bool RecibirAvisos { get; set; }

    public PreferenciasAvisosDto? PreferenciasAvisos { get; set; }
}

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
