using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Application.UseCases.Catalogo;
public sealed class ConsultarAliadosUseCase(ICatalogoRepository catalogo)
{
    public async Task<IReadOnlyList<AsociacionDto>> ExecuteAsync(CancellationToken ct)=>(await catalogo.AsociacionesAsync(ct)).Select(a=>a.Dto()).ToList();
}
