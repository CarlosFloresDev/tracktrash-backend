using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Application.UseCases.Recompensas;

public sealed class ListarBeneficiosUseCase(IBeneficioRepository repo)
{
    public async Task<IReadOnlyList<BeneficioDto>> ExecuteAsync(CancellationToken ct) => (await repo.ListarAsync(ct)).Select(b => new BeneficioDto(b.Id, b.Nombre, b.Descripcion, b.CostoPuntos, b.Stock, b.Demostracion)).ToList();
}
