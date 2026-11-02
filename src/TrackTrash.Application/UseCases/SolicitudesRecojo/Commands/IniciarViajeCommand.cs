using MediatR;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands;

public sealed record IniciarViajeCommand : IRequest<SolicitudDto>
{
    public Guid SolicitudId { get; set; }
}

internal sealed class IniciarViajeCommandHandler(
    ISolicitudRecojoRepository repo,
    IHistoricoRepository historial,
    INotificadorUsuarioService avisos,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork,
    TimeProvider reloj) : IRequestHandler<IniciarViajeCommand, SolicitudDto>
{
    public async Task<SolicitudDto> Handle(IniciarViajeCommand request, CancellationToken cancellationToken)
    {
        if (!usuario.EsReciclador) throw new AccesoDenegadoException(); var s = await repo.ObtenerAsync(request.SolicitudId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        if (s.RecicladorId != usuario.Id) throw new AccesoDenegadoException();
        if (s.Estado != EstadoSolicitud.EnCamino)
        {
            s.IniciarViaje(usuario.Id); historial.Agregar(HistoricoSolicitud.Crear(request.SolicitudId, usuario.Id, "Reciclador en camino", reloj.GetUtcNow()));
            await avisos.AgregarAsync(Notificacion.Crear(s.VecinoId, $"camino:{request.SolicitudId}", "El reciclador está en camino", "Prepara los materiales y comparte el código solo al entregarlos.", reloj.GetUtcNow(), TipoAviso.Recojos), cancellationToken);
            await unitOfWork.GuardarCambiosAsync(cancellationToken);
        }
        return SolicitudDto.Desde(s, usuario.Id);
    }
}
