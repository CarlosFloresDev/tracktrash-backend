using TrackTrash.Application.DTOs.SolicitudesRecojo;
using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Queries.ConsultarHistorial;

internal sealed class ConsultarHistorialQueryHandler(
    ISolicitudRecojoRepository repo,
    IHistoricoRepository historial,
    IUsuarioActualService usuario) : IRequestHandler<ConsultarHistorialQuery, IReadOnlyList<HistoricoDto>>
{
    public async Task<IReadOnlyList<HistoricoDto>> Handle(ConsultarHistorialQuery request, CancellationToken cancellationToken)
    {
        var s = await repo.ObtenerAsync(request.SolicitudId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        if (s.VecinoId != usuario.Id && s.RecicladorId != usuario.Id && !usuario.EsAdministrador) throw new RecursoNoEncontradoException();
        return (await historial.ListarAsync(request.SolicitudId, cancellationToken)).Select(e => new HistoricoDto(e.Accion, e.CreadoEn)).ToList();
    }
}
