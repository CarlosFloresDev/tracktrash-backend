using MediatR;
using TrackTrash.Application.DTOs.Contenedores;

namespace TrackTrash.Application.UseCases.Contenedores.Commands.CrearContenedor;

public sealed record CrearContenedorCommand : IRequest<ContenedorDto>
{
    public Guid DistritoId { get; init; }

    public string Nombre { get; init; } = null!;

    public double Latitud { get; init; }

    public double Longitud { get; init; }
}
