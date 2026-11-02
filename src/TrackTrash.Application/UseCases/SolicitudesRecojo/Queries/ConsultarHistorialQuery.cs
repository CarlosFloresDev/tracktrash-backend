using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Queries;

public sealed record ConsultarHistorialQuery : IRequest<IReadOnlyList<HistoricoDto>>
{
    public Guid SolicitudId { get; set; }
}

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
