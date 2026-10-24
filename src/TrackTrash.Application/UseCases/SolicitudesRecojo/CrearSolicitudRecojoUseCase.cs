using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo;

public sealed class CrearSolicitudRecojoUseCase(ISolicitudRecojoRepository solicitudes,
    IUnitOfWork uow, IUsuarioActualService usuario, ICodigoEntregaService codigos, TimeProvider reloj)
{
    public async Task<SolicitudDto> ExecuteAsync(CrearSolicitudRequest request, CancellationToken ct)
    {
        if (usuario.EsReciclador) throw new AccesoDenegadoException();
        var existente = await solicitudes.ObtenerPorClaveAsync(usuario.Id, request.ClaveSolicitud, ct);
        if (existente is not null)
        {
            if (existente.Material != request.Material || existente.CantidadEstimadaKg != request.CantidadEstimadaKg
                || existente.Direccion != request.Direccion.Trim()
                || existente.ProgramadaPara.UtcTicks / 10 != request.ProgramadaPara.UtcTicks / 10)
                throw new Domain.Exceptions.ReglaNegocioException("Esta clave de solicitud ya se utilizó con otros datos.");
            return SolicitudDto.Desde(existente, usuario.Id);
        }
        var solicitud = SolicitudRecojo.Crear(usuario.Id, request.ClaveSolicitud, request.Material,
            request.CantidadEstimadaKg, request.Direccion, request.ProgramadaPara, reloj.GetUtcNow(), codigos.Generar());
        solicitudes.Agregar(solicitud);
        await uow.GuardarCambiosAsync(ct);
        return SolicitudDto.Desde(solicitud, usuario.Id);
    }
}
