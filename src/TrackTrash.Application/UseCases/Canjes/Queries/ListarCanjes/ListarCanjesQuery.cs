using MediatR;
using TrackTrash.Application.DTOs.Canjes;

namespace TrackTrash.Application.UseCases.Canjes.Queries.ListarCanjes;

public sealed record ListarCanjesQuery : IRequest<IReadOnlyList<CanjeDto>>;
