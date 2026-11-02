using MediatR;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands;

public sealed record AceptarSolicitudRecojoCommand : IRequest<SolicitudDto>
{
    public Guid SolicitudId { get; set; }
}

internal sealed class AceptarSolicitudRecojoCommandHandler(
    ISolicitudRecojoRepository solicitudes,
    IUnitOfWork unitOfWork,
    IUsuarioActualService usuario,
    IPerfilRepository perfiles,
    ICatalogoRepository catalogo,
    IHistoricoRepository historial,
    INotificadorUsuarioService avisos,
    TimeProvider reloj) : IRequestHandler<AceptarSolicitudRecojoCommand, SolicitudDto>
{
    public async Task<SolicitudDto> Handle(AceptarSolicitudRecojoCommand request, CancellationToken cancellationToken)
    {
        if (!usuario.EsReciclador) throw new AccesoDenegadoException();
        var s = await solicitudes.ObtenerAsync(request.SolicitudId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        if (s.Estado == Domain.Entities.EstadoSolicitud.Pendiente)
        {
            var perfil = await perfiles.ObtenerAsync(usuario.Id, cancellationToken);
            if (perfil?.AsociacionId is null) throw new AccesoDenegadoException();
            var a = await catalogo.AsociacionAsync(perfil.AsociacionId.Value, cancellationToken) ?? throw new AccesoDenegadoException();
            if (s.AsociacionId.HasValue && s.AsociacionId != a.Id) throw new AccesoDenegadoException();
            a.Reservar(s.CantidadEstimadaKg, s.Material); s.Asignar(a.Id);
            historial.Agregar(Domain.Entities.HistoricoSolicitud.Crear(s.Id, usuario.Id, "Recojo aceptado", reloj.GetUtcNow()));
            await avisos.AgregarAsync(Domain.Entities.Notificacion.Crear(s.VecinoId, $"aceptada:{s.Id}", "Tu recojo fue aceptado", "El reciclador ya puede coordinar tu entrega.", reloj.GetUtcNow(), TipoAviso.Recojos), cancellationToken);
        }
        s.Aceptar(usuario.Id);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);
        return SolicitudDto.Desde(s, usuario.Id);
    }
}
