using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Application.UseCases.Rutas;

public sealed class ListarRutasUseCase(IRutaRepository repo, IUsuarioActualService usuario, TimeProvider reloj)
{
    public async Task<IReadOnlyList<RutaDto>> ExecuteAsync(CancellationToken ct)
    { var subs = (await repo.SuscripcionesAsync(usuario.Id, ct)).Select(s => s.RutaId).ToHashSet(); return (await repo.ListarAsync(ct)).Select(r => new RutaDto(r.Id, r.Nombre, r.Zona, r.Dias, r.HoraLocal.ToString("HH:mm"), r.ProximoPaso(reloj.GetUtcNow()), r.Demostracion, subs.Contains(r.Id))).ToList(); }
}
