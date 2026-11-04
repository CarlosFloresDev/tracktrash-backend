using MediatR;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Application.UseCases.PuntosAcopio.Commands.CrearPuntoAcopio;

public sealed record CrearPuntoAcopioCommand : IRequest<Guid>
{
    public string Nombre { get; init; } = null!;
    public string Direccion { get; init; } = null!;
    public double Latitud { get; init; }
    public double Longitud { get; init; }
}
