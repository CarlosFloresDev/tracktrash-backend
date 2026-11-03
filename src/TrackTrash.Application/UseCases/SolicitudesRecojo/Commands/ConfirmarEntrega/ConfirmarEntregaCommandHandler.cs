using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Common;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.ConfirmarEntrega;

internal sealed class ConfirmarEntregaCommandHandler(ISolicitudRecojoRepository solicitudes,
    IMovimientoPuntosRepository puntos, IUnitOfWork uow, IUsuarioActualService usuario, TimeProvider reloj)
    : IRequestHandler<ConfirmarEntregaCommand, SolicitudDto>
{
    public async Task<SolicitudDto> Handle(ConfirmarEntregaCommand request, CancellationToken ct)
    {
        if (!usuario.EsReciclador) throw new AccesoDenegadoException();
        var s = await solicitudes.ObtenerAsync(request.Id, ct) ?? throw new RecursoNoEncontradoException();
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
