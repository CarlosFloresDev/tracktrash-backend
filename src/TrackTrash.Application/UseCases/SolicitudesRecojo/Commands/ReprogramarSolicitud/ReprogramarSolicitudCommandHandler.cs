using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.ReprogramarSolicitud;

internal sealed class ReprogramarSolicitudCommandHandler(
    ISolicitudRecojoRepository repo,
    IHistoricoRepository historial,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork,
    TimeProvider reloj) : IRequestHandler<ReprogramarSolicitudCommand, SolicitudDto>
{
    public async Task<SolicitudDto> Handle(ReprogramarSolicitudCommand request, CancellationToken cancellationToken)
    {
        var s = await repo.ObtenerAsync(request.SolicitudId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        if (s.VecinoId != usuario.Id) throw new AccesoDenegadoException(); s.Reprogramar(request.ProgramadaPara, reloj.GetUtcNow());
        historial.Agregar(HistoricoSolicitud.Crear(request.SolicitudId, usuario.Id, "Fecha reprogramada", reloj.GetUtcNow())); await unitOfWork.GuardarCambiosAsync(cancellationToken); return SolicitudDto.Desde(s, usuario.Id);
    }
}
