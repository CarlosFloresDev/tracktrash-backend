using MediatR;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands;

public sealed record CancelarSolicitudCommand : IRequest<SolicitudDto>
{
    public Guid SolicitudId { get; set; }

    public string Motivo { get; set; } = null!;
}

internal sealed class CancelarSolicitudCommandHandler(
    ISolicitudRecojoRepository repo,
    ICatalogoRepository catalogo,
    IHistoricoRepository historial,
    INotificadorUsuarioService avisos,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork,
    TimeProvider reloj) : IRequestHandler<CancelarSolicitudCommand, SolicitudDto>
{
    public async Task<SolicitudDto> Handle(CancelarSolicitudCommand request, CancellationToken cancellationToken)
    {
        var s = await repo.ObtenerAsync(request.SolicitudId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        if (s.VecinoId != usuario.Id && s.RecicladorId != usuario.Id && !usuario.EsAdministrador) throw new AccesoDenegadoException();
        if (s.Estado != EstadoSolicitud.Cancelada)
        {
            var reservada = s.Estado is EstadoSolicitud.Aceptada or EstadoSolicitud.EnCamino;
            s.Cancelar(request.Motivo);
            if (reservada && s.AsociacionId is Guid a) { var asoc = await catalogo.AsociacionAsync(a, cancellationToken); asoc?.Liberar(s.CantidadEstimadaKg); }
            historial.Agregar(HistoricoSolicitud.Crear(request.SolicitudId, usuario.Id, "Cancelación: " + s.MotivoCancelacion, reloj.GetUtcNow()));
            await avisos.AgregarAsync(Notificacion.Crear(s.VecinoId, $"cancelada:{request.SolicitudId}", "Recojo cancelado", s.MotivoCancelacion!, reloj.GetUtcNow(), TipoAviso.Recojos), cancellationToken);
            await unitOfWork.GuardarCambiosAsync(cancellationToken);
        }
        return SolicitudDto.Desde(s, usuario.Id);
    }
}
