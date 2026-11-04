using MediatR;
using TrackTrash.Application.DTOs.Asociaciones;

namespace TrackTrash.Application.UseCases.Asociaciones.Queries.ListarAsociaciones;

public sealed record ListarAsociacionesQuery : IRequest<IReadOnlyList<AsociacionDto>>;
