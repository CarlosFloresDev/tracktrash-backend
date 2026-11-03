using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Common;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.AceptarSolicitudRecojo;

internal sealed class AceptarSolicitudRecojoCommandHandler(ISolicitudRecojoRepository solicitudes,
    IUnitOfWork uow, IUsuarioActualService usuario)
    : IRequestHandler<AceptarSolicitudRecojoCommand, SolicitudDto>
{
    public async Task<SolicitudDto> Handle(AceptarSolicitudRecojoCommand request, CancellationToken ct)
    {
        if (!usuario.EsReciclador) throw new AccesoDenegadoException();
        var s = await solicitudes.ObtenerAsync(request.Id, ct) ?? throw new RecursoNoEncontradoException();
        s.Aceptar(usuario.Id);
        await uow.GuardarCambiosAsync(ct);
        return SolicitudDto.Desde(s, usuario.Id);
    }
}
