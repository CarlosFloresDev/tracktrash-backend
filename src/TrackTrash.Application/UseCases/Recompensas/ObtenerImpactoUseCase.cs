using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Recompensas;

public sealed class ObtenerImpactoUseCase(IMovimientoPuntosRepository puntos, IUsuarioActualService usuario)
{
    public async Task<ImpactoDto> ExecuteAsync(CancellationToken ct) => new(await puntos.ObtenerTotalAsync(usuario.Id, ct));
}
