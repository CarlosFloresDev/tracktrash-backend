using TrackTrash.Application.DTOs.MovimientosPuntos;
using MediatR;

namespace TrackTrash.Application.UseCases.MovimientosPuntos.Queries.ObtenerImpacto;

public sealed record ObtenerImpactoQuery : IRequest<ImpactoDto>;
