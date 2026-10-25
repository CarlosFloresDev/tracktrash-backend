using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Application.UseCases.Rutas;

public sealed class ListarRutasUseCase(IRutaRepository repo, IUsuarioActualService usuario, TimeProvider reloj)
{
    public async Task<IReadOnlyList<RutaDto>> ExecuteAsync(CancellationToken ct)
    {
        var suscripciones = (await repo.SuscripcionesAsync(usuario.Id, ct)).ToDictionary(s => s.RutaId);
        return (await repo.ListarAsync(ct)).Select(r =>
        {
            suscripciones.TryGetValue(r.Id, out var suscripcion);
            return new RutaDto(r.Id, r.Nombre, r.Zona, r.Dias, r.HoraLocal.ToString("HH:mm"),
                r.ProximoPaso(reloj.GetUtcNow()), r.Demostracion, suscripcion is not null, suscripcion?.MinutosAntes);
        }).ToList();
    }
}
