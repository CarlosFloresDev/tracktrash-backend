using TrackTrash.Application.DTOs.MovimientosPuntos;
using MediatR;

namespace TrackTrash.Application.UseCases.MovimientosPuntos.Queries.ListarMovimientos;

public sealed record ListarMovimientosQuery : IRequest<IReadOnlyList<MovimientoDto>>;
