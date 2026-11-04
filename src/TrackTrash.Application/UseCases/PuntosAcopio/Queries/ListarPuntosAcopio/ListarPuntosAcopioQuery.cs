using MediatR;
using TrackTrash.Application.DTOs.PuntosAcopio;

namespace TrackTrash.Application.UseCases.PuntosAcopio.Queries.ListarPuntosAcopio;

public sealed record ListarPuntosAcopioQuery : IRequest<IReadOnlyList<PuntoAcopioDto>>;
