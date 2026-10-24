using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.SolicitudesRecojo;

public sealed class CancelarSolicitudUseCase(ISolicitudRecojoRepository repo, ICatalogoRepository catalogo, IHistoricoRepository historial, INotificacionRepository avisos, IUsuarioActualService usuario, IUnitOfWork uow, TimeProvider reloj)
{
    public async Task<SolicitudDto> ExecuteAsync(Guid id, string motivo, CancellationToken ct)
    {
        var s = await repo.ObtenerAsync(id, ct) ?? throw new RecursoNoEncontradoException();
        if (s.VecinoId != usuario.Id && s.RecicladorId != usuario.Id && !usuario.EsAdministrador) throw new AccesoDenegadoException();
        if (s.Estado != EstadoSolicitud.Cancelada)
        {
            var reservada = s.Estado is EstadoSolicitud.Aceptada or EstadoSolicitud.EnCamino;
            s.Cancelar(motivo);
            if (reservada && s.AsociacionId is Guid a) { var asoc = await catalogo.AsociacionAsync(a, ct); asoc?.Liberar(s.CantidadEstimadaKg); }
            historial.Agregar(HistoricoSolicitud.Crear(id, usuario.Id, "Cancelación: " + s.MotivoCancelacion, reloj.GetUtcNow()));
            avisos.Agregar(Notificacion.Crear(s.VecinoId, $"cancelada:{id}", "Recojo cancelado", s.MotivoCancelacion!, reloj.GetUtcNow()));
            await uow.GuardarCambiosAsync(ct);
        }
        return SolicitudDto.Desde(s, usuario.Id);
    }
}
