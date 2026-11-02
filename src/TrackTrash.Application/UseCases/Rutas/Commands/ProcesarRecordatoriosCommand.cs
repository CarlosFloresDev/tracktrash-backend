using MediatR;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Rutas.Commands;

public sealed record ProcesarRecordatoriosCommand : IRequest;

internal sealed class ProcesarRecordatoriosCommandHandler(
    IRutaRepository rutas,
    INotificadorUsuarioService avisos,
    IUnitOfWork unitOfWork,
    TimeProvider reloj) : IRequestHandler<ProcesarRecordatoriosCommand>
{
    public async Task Handle(ProcesarRecordatoriosCommand request, CancellationToken cancellationToken)
    {
        var ahora = reloj.GetUtcNow();
        foreach (var s in await rutas.PendientesAsync(ahora, cancellationToken))
        {
            var r = await rutas.ObtenerAsync(s.RutaId, cancellationToken); if (r is null) continue;
            if (ahora - s.ProximoAviso < TimeSpan.FromMinutes(s.MinutosAntes))
                await avisos.AgregarAsync(Notificacion.Crear(s.UsuarioId, $"horario:{s.Id}:{s.ProximoAviso:yyyyMMddHHmm}", "Recuerda tu horario de recolección", $"La ruta {r.Nombre} tiene un paso programado próximo. Este aviso se basa en horarios, no en GPS del camión.", ahora, TipoAviso.Horarios), cancellationToken);
            s.Programar(r, ahora);
        }
        await unitOfWork.GuardarCambiosAsync(cancellationToken);
    }
}
