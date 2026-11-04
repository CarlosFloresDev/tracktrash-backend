using TrackTrash.Application.DTOs.Rutas;
using MediatR;

namespace TrackTrash.Application.UseCases.Rutas.Commands.SuscribirRuta;

public sealed record SuscribirRutaCommand : IRequest
{
    public Guid RutaId { get; init; }

    public int MinutosAntes { get; init; }
}
