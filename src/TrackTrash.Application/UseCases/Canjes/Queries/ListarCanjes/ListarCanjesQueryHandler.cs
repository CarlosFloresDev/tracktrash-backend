using MediatR;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.DTOs.Asociaciones;
using TrackTrash.Application.DTOs.Canjes;
using TrackTrash.Application.DTOs.Contenedores;
using TrackTrash.Application.DTOs.Incidencias;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Canjes.Queries.ListarCanjes;

internal sealed class ListarCanjesQueryHandler(
    IBeneficioRepository repo,
    IUsuarioActualService usuario) : IRequestHandler<ListarCanjesQuery, IReadOnlyList<CanjeDto>>
{
    public async Task<IReadOnlyList<CanjeDto>> Handle(ListarCanjesQuery request, CancellationToken cancellationToken) =>
        (await repo.CanjesAsync(usuario.Id, cancellationToken)).Select(c => c.ToDto()).ToList();
}
