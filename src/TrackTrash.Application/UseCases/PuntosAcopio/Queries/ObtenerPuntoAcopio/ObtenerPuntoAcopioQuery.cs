using MediatR;
using TrackTrash.Application.DTOs.PuntosAcopio;

namespace TrackTrash.Application.UseCases.PuntosAcopio.Queries.ObtenerPuntoAcopio;

public sealed record ObtenerPuntoAcopioQuery : IRequest<PuntoAcopioDto>
{
    public Guid Id { get; init; }
}
