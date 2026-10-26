using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Application.UseCases.Operacion;

public sealed class ObtenerIndicadoresUseCase(IIndicadoresRepository repo, IUsuarioActualService usuario, TimeProvider reloj)
{
    public Task<IndicadoresOperacion> ExecuteAsync(CancellationToken ct)
    { if (!usuario.EsAdministrador) throw new AccesoDenegadoException(); return repo.ObtenerAsync(reloj.GetUtcNow(), ct); }
}
