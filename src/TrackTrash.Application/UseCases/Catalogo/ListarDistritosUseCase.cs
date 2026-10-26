using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Application.UseCases.Catalogo;

public sealed class ListarDistritosUseCase(ICatalogoRepository repo)
{
    public async Task<IReadOnlyList<DistritoDto>> ExecuteAsync(CancellationToken ct) => (await repo.DistritosAsync(ct)).Select(d => new DistritoDto(d.Id, d.Nombre)).ToList();
}
