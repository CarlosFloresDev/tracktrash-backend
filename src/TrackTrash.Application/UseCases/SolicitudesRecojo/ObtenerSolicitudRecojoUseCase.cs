using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo;

public sealed class ObtenerSolicitudRecojoUseCase(ISolicitudRecojoRepository solicitudes, IUsuarioActualService usuario)
{
    public async Task<SolicitudDto> ExecuteAsync(Guid id, CancellationToken ct)
    {
        var s = await solicitudes.ObtenerAsync(id, ct) ?? throw new RecursoNoEncontradoException();
        if (s.VecinoId != usuario.Id && !(usuario.EsReciclador &&
            (s.RecicladorId == usuario.Id || s.Estado == EstadoSolicitud.Pendiente)))
            throw new RecursoNoEncontradoException();
        return SolicitudDto.Desde(s, usuario.Id);
    }
}
