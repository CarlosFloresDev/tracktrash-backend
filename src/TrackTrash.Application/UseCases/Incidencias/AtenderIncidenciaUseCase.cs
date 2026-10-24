using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Incidencias;
public sealed class AtenderIncidenciaUseCase(IIncidenciaRepository repo,IUsuarioActualService usuario,IUnitOfWork uow)
{
    public async Task ExecuteAsync(Guid id,CancellationToken ct)
    {if(!usuario.EsAdministrador)throw new AccesoDenegadoException();var i=await repo.ObtenerAsync(id,ct)??throw new RecursoNoEncontradoException();i.Atender(usuario.Id);await uow.GuardarCambiosAsync(ct);}
}
