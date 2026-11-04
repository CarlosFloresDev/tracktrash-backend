using TrackTrash.Application.DTOs.Distritos;
using MediatR;

namespace TrackTrash.Application.UseCases.Distritos.Queries.ListarDistritos;

public sealed record ListarDistritosQuery : IRequest<IReadOnlyList<DistritoDto>>;
