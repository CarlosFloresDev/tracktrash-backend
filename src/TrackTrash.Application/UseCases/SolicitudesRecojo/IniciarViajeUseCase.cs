using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.SolicitudesRecojo;
public sealed class IniciarViajeUseCase(ISolicitudRecojoRepository repo,IHistoricoRepository historial,INotificacionRepository avisos,IUsuarioActualService usuario,IUnitOfWork uow,TimeProvider reloj)
{
    public async Task<SolicitudDto> ExecuteAsync(Guid id,CancellationToken ct)
    {
        if(!usuario.EsReciclador)throw new AccesoDenegadoException();var s=await repo.ObtenerAsync(id,ct)??throw new RecursoNoEncontradoException();
        if(s.RecicladorId!=usuario.Id)throw new AccesoDenegadoException();
        if(s.Estado!=EstadoSolicitud.EnCamino)
        {
            s.IniciarViaje(usuario.Id);historial.Agregar(HistoricoSolicitud.Crear(id,usuario.Id,"Reciclador en camino",reloj.GetUtcNow()));
            avisos.Agregar(Notificacion.Crear(s.VecinoId,$"camino:{id}","El reciclador está en camino","Prepara los materiales y comparte el código solo al entregarlos.",reloj.GetUtcNow()));
            await uow.GuardarCambiosAsync(ct);
        }
        return SolicitudDto.Desde(s,usuario.Id);
    }
}
