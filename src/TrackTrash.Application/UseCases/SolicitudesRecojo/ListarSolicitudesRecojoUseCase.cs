using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo;

public sealed class ListarSolicitudesRecojoUseCase(ISolicitudRecojoRepository solicitudes, IUsuarioActualService usuario)
{
    public async Task<IReadOnlyList<SolicitudDto>> ExecuteAsync(int pagina, CancellationToken ct)
    {
        if (pagina < 1 || pagina > 10000) throw new Domain.Exceptions.ReglaNegocioException("La página debe estar entre 1 y 10000.");
        var lista = await solicitudes.ListarAsync(usuario.Id, usuario.EsReciclador, pagina, ct);
        return lista.Select(s => SolicitudDto.Desde(s, usuario.Id)).ToList();
    }
}
