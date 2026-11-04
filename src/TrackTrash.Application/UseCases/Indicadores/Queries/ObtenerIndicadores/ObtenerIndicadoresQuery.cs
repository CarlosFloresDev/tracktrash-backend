using TrackTrash.Application.DTOs.Indicadores;
using MediatR;

namespace TrackTrash.Application.UseCases.Indicadores.Queries.ObtenerIndicadores;

public sealed record ObtenerIndicadoresQuery : IRequest<IndicadoresDto>;
