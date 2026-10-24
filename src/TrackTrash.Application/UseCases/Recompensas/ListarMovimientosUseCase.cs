using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Application.UseCases.Recompensas;

public sealed record MovimientoDto(Guid Id, int Puntos, string Motivo, DateTimeOffset CreadoEn);
public sealed class ListarMovimientosUseCase(IMovimientoPuntosRepository repo, IUsuarioActualService usuario)
{
    public async Task<IReadOnlyList<MovimientoDto>> ExecuteAsync(CancellationToken ct) => (await repo.ListarAsync(usuario.Id, ct)).Select(m => new MovimientoDto(m.Id, m.Puntos, m.Puntos > 0 ? "Entrega confirmada" : "Canje de beneficio", m.CreadoEn)).ToList();
}
