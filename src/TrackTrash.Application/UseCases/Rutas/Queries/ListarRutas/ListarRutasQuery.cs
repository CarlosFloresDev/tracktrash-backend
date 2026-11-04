using TrackTrash.Application.DTOs.Rutas;
using MediatR;

namespace TrackTrash.Application.UseCases.Rutas.Queries.ListarRutas;

public sealed record ListarRutasQuery : IRequest<IReadOnlyList<RutaDto>>;
