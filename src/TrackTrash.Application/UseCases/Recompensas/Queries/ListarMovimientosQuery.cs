using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Recompensas.Queries;

public sealed record ListarMovimientosQuery : IRequest<IReadOnlyList<MovimientoDto>>;

internal sealed class ListarMovimientosQueryHandler(
    IMovimientoPuntosRepository repo,
    IUsuarioActualService usuario) : IRequestHandler<ListarMovimientosQuery, IReadOnlyList<MovimientoDto>>
{
    public async Task<IReadOnlyList<MovimientoDto>> Handle(ListarMovimientosQuery request, CancellationToken cancellationToken) =>
        (await repo.ListarAsync(usuario.Id, cancellationToken)).Select(m => new MovimientoDto(m.Id, m.Puntos, m.Puntos > 0 ? "Entrega confirmada" : "Canje de beneficio", m.CreadoEn)).ToList();
}
