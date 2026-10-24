using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo;

public sealed class AceptarSolicitudRecojoUseCase(ISolicitudRecojoRepository solicitudes,
    IUnitOfWork uow, IUsuarioActualService usuario)
{
    public async Task<SolicitudDto> ExecuteAsync(Guid id, CancellationToken ct)
    {
        if (!usuario.EsReciclador) throw new AccesoDenegadoException();
        var s = await solicitudes.ObtenerAsync(id, ct) ?? throw new RecursoNoEncontradoException();
        s.Aceptar(usuario.Id);
        await uow.GuardarCambiosAsync(ct);
        return SolicitudDto.Desde(s, usuario.Id);
    }
}
