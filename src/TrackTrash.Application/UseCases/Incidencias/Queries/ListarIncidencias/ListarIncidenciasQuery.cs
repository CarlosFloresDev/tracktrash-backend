using MediatR;
using TrackTrash.Application.DTOs.Incidencias;

namespace TrackTrash.Application.UseCases.Incidencias.Queries.ListarIncidencias;

public sealed record ListarIncidenciasQuery : IRequest<IReadOnlyList<IncidenciaDto>>;
