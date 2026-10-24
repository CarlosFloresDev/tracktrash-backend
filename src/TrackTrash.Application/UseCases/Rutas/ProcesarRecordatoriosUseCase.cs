using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Rutas;

public sealed class ProcesarRecordatoriosUseCase(IRutaRepository rutas, IPerfilRepository perfiles, INotificacionRepository avisos, IUnitOfWork uow, TimeProvider reloj)
{
    public async Task ExecuteAsync(CancellationToken ct)
    {
        var ahora = reloj.GetUtcNow();
        foreach (var s in await rutas.PendientesAsync(ahora, ct))
        {
            var r = await rutas.ObtenerAsync(s.RutaId, ct); if (r is null) continue;
            var perfil = await perfiles.ObtenerAsync(s.UsuarioId, ct);
            if (perfil?.RecibirAvisos != false && ahora - s.ProximoAviso < TimeSpan.FromMinutes(s.MinutosAntes))
                avisos.Agregar(Notificacion.Crear(s.UsuarioId, $"horario:{s.Id}:{s.ProximoAviso:yyyyMMddHHmm}", "Recuerda tu horario de recolección", $"La ruta {r.Nombre} tiene un paso programado próximo. Este aviso se basa en horarios, no en GPS del camión.", ahora));
            s.Programar(r, ahora);
        }
        await uow.GuardarCambiosAsync(ct);
    }
}
