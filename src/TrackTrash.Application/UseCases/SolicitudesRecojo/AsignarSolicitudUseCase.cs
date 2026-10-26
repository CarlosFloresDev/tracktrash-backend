using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.SolicitudesRecojo;

public sealed class AsignarSolicitudUseCase(ISolicitudRecojoRepository repo, ICatalogoRepository catalogo, IHistoricoRepository historial, IUsuarioActualService usuario, IUnitOfWork uow, TimeProvider reloj)
{
    public async Task<SolicitudDto> ExecuteAsync(Guid id, CancellationToken ct)
    {
        var s = await repo.ObtenerAsync(id, ct) ?? throw new RecursoNoEncontradoException();
        if (s.VecinoId != usuario.Id) throw new AccesoDenegadoException();
        if (!s.Latitud.HasValue) throw new Domain.Exceptions.ReglaNegocioException("La asignación automática requiere la ubicación del recojo.");
        var a = (await catalogo.CandidatosAsync(s.Material, s.CantidadEstimadaKg, s.Latitud.Value, s.Longitud!.Value, ct)).FirstOrDefault();
        if (a is null) throw new Domain.Exceptions.ReglaNegocioException("No hay asociaciones elegibles a menos de 20 km para ese material y cantidad.");
        s.Asignar(a.Id); historial.Agregar(HistoricoSolicitud.Crear(id, usuario.Id, "Asignada a " + a.Nombre, reloj.GetUtcNow())); await uow.GuardarCambiosAsync(ct); return SolicitudDto.Desde(s, usuario.Id);
    }
}
