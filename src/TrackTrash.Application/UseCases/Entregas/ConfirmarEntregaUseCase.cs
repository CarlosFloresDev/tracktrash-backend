using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Entregas;

public sealed class ConfirmarEntregaUseCase(ISolicitudRecojoRepository solicitudes,
    IMovimientoPuntosRepository puntos, IUnitOfWork uow, IUsuarioActualService usuario, TimeProvider reloj)
{
    public async Task<SolicitudDto> ExecuteAsync(Guid id, ConfirmarEntregaRequest request, CancellationToken ct)
    {
        if (!usuario.EsReciclador) throw new AccesoDenegadoException();
        var s = await solicitudes.ObtenerAsync(id, ct) ?? throw new RecursoNoEncontradoException();
        if (s.RecicladorId != usuario.Id) throw new AccesoDenegadoException();
        var ahora = reloj.GetUtcNow();
        if (s.Confirmar(usuario.Id, request.Codigo, ahora))
        {
            puntos.Agregar(MovimientoPuntos.PorEntrega(s, ahora));
            await uow.GuardarCambiosAsync(ct);
        }
        return SolicitudDto.Desde(s, usuario.Id);
    }
}
