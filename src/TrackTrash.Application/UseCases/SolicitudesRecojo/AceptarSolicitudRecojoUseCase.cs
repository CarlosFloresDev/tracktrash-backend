using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo;

public sealed class AceptarSolicitudRecojoUseCase(ISolicitudRecojoRepository solicitudes,
    IUnitOfWork uow, IUsuarioActualService usuario,IPerfilRepository perfiles,ICatalogoRepository catalogo,IHistoricoRepository historial,INotificacionRepository avisos,TimeProvider reloj)
{
    public async Task<SolicitudDto> ExecuteAsync(Guid id, CancellationToken ct)
    {
        if (!usuario.EsReciclador) throw new AccesoDenegadoException();
        var s = await solicitudes.ObtenerAsync(id, ct) ?? throw new RecursoNoEncontradoException();
        if(s.Estado==Domain.Entities.EstadoSolicitud.Pendiente)
        {
            var perfil=await perfiles.ObtenerAsync(usuario.Id,ct);
            if(perfil?.AsociacionId is null)throw new AccesoDenegadoException();
            var a=await catalogo.AsociacionAsync(perfil.AsociacionId.Value,ct)??throw new AccesoDenegadoException();
            if(s.AsociacionId.HasValue&&s.AsociacionId!=a.Id)throw new AccesoDenegadoException();
            a.Reservar(s.CantidadEstimadaKg,s.Material);s.Asignar(a.Id);
            historial.Agregar(Domain.Entities.HistoricoSolicitud.Crear(s.Id,usuario.Id,"Recojo aceptado",reloj.GetUtcNow()));
            avisos.Agregar(Domain.Entities.Notificacion.Crear(s.VecinoId,$"aceptada:{s.Id}","Tu recojo fue aceptado","El reciclador ya puede coordinar tu entrega.",reloj.GetUtcNow()));
        }
        s.Aceptar(usuario.Id);
        await uow.GuardarCambiosAsync(ct);
        return SolicitudDto.Desde(s, usuario.Id);
    }
}
