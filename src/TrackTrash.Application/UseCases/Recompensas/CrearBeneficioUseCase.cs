using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Recompensas;

public sealed class CrearBeneficioUseCase(IBeneficioRepository repo, IUsuarioActualService usuario, IUnitOfWork uow)
{
    public async Task<Guid> ExecuteAsync(CrearBeneficioRequest r, CancellationToken ct)
    { if (!usuario.EsAdministrador) throw new AccesoDenegadoException(); var b = Beneficio.Crear(r.Nombre, r.Descripcion, r.CostoPuntos, r.Stock); repo.Agregar(b); await uow.GuardarCambiosAsync(ct); return b.Id; }
}
