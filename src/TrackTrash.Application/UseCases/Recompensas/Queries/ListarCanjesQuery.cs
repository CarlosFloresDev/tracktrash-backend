using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Recompensas.Queries;

public sealed record ListarCanjesQuery : IRequest<IReadOnlyList<CanjeDto>>;

internal sealed class ListarCanjesQueryHandler(
    IBeneficioRepository repo,
    IUsuarioActualService usuario) : IRequestHandler<ListarCanjesQuery, IReadOnlyList<CanjeDto>>
{
    public async Task<IReadOnlyList<CanjeDto>> Handle(ListarCanjesQuery request, CancellationToken cancellationToken) =>
        (await repo.CanjesAsync(usuario.Id, cancellationToken)).Select(c => c.Dto()).ToList();
}
