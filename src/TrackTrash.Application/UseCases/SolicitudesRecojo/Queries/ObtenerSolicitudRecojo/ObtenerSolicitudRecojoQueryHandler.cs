using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Common;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Queries.ObtenerSolicitudRecojo;

internal sealed class ObtenerSolicitudRecojoQueryHandler(ISolicitudRecojoRepository solicitudes, IUsuarioActualService usuario)
    : IRequestHandler<ObtenerSolicitudRecojoQuery, SolicitudDto>
{
    public async Task<SolicitudDto> Handle(ObtenerSolicitudRecojoQuery request, CancellationToken ct)
    {
        var s = await solicitudes.ObtenerAsync(request.Id, ct) ?? throw new RecursoNoEncontradoException();
        if (s.VecinoId != usuario.Id && !(usuario.EsReciclador &&
            (s.RecicladorId == usuario.Id || s.Estado == EstadoSolicitud.Pendiente)))
            throw new RecursoNoEncontradoException();
        return SolicitudDto.Desde(s, usuario.Id);
    }
}
