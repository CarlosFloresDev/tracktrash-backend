using MediatR;
using TrackTrash.Application.DTOs.Perfiles;

namespace TrackTrash.Application.UseCases.Perfiles.Commands.ActualizarPerfil;

public sealed record ActualizarPerfilCommand : IRequest
{
    public string Nombre { get; init; } = null!;

    public string? Telefono { get; init; }

    public string? Direccion { get; init; }

    public double? Latitud { get; init; }

    public double? Longitud { get; init; }

    public bool RecibirAvisos { get; init; }

    public PreferenciasAvisosDto? PreferenciasAvisos { get; init; }
}
