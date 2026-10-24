using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Application.UseCases.Recompensas;

public sealed class ListarCanjesUseCase(IBeneficioRepository repo, IUsuarioActualService usuario)
{
    public async Task<IReadOnlyList<CanjeDto>> ExecuteAsync(CancellationToken ct) => (await repo.CanjesAsync(usuario.Id, ct)).Select(c => c.Dto()).ToList();
}
