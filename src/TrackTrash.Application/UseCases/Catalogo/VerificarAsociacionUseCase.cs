using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Catalogo;

public sealed class VerificarAsociacionUseCase(ICatalogoRepository catalogo, IUsuarioActualService usuario, IUnitOfWork uow)
{
    public async Task ExecuteAsync(Guid id, CancellationToken ct)
    { if (!usuario.EsAdministrador) throw new AccesoDenegadoException(); var a = await catalogo.AsociacionAsync(id, ct) ?? throw new RecursoNoEncontradoException(); a.Verificar(); await uow.GuardarCambiosAsync(ct); }
}
