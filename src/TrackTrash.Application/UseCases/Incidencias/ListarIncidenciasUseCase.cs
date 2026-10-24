using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Application.UseCases.Incidencias;
public sealed class ListarIncidenciasUseCase(IIncidenciaRepository repo,IUsuarioActualService usuario)
{
    public async Task<IReadOnlyList<IncidenciaDto>> ExecuteAsync(CancellationToken ct)=>(await repo.ListarAsync(usuario.Id,usuario.EsAdministrador,ct)).Select(i=>i.Dto()).ToList();
}
