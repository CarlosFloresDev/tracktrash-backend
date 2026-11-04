using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.AsignarSolicitud;

internal sealed class AsignarSolicitudCommandHandler(
    ISolicitudRecojoRepository repo,
    ICatalogoRepository catalogo,
    IHistoricoRepository historial,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork,
    TimeProvider reloj) : IRequestHandler<AsignarSolicitudCommand, SolicitudDto>
{
    public async Task<SolicitudDto> Handle(AsignarSolicitudCommand request, CancellationToken cancellationToken)
    {
        var s = await repo.ObtenerAsync(request.SolicitudId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        if (s.VecinoId != usuario.Id) throw new AccesoDenegadoException();
        if (!s.Latitud.HasValue) throw new Domain.Exceptions.ReglaNegocioException("La asignación automática requiere la ubicación del recojo.");
        var a = (await catalogo.CandidatosAsync(s.Material, s.CantidadEstimadaKg, s.Latitud.Value, s.Longitud!.Value, cancellationToken)).FirstOrDefault();
        if (a is null) throw new Domain.Exceptions.ReglaNegocioException("No hay asociaciones elegibles a menos de 20 km para ese material y cantidad.");
        s.Asignar(a.Id); historial.Agregar(HistoricoSolicitud.Crear(request.SolicitudId, usuario.Id, "Asignada a " + a.Nombre, reloj.GetUtcNow())); await unitOfWork.GuardarCambiosAsync(cancellationToken); return SolicitudDto.Desde(s, usuario.Id);
    }
}
