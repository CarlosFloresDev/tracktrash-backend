using MediatR;
using TrackTrash.Application.DTOs.ReportesPasoCamion;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.ReportesPasoCamion.Queries.ListarReportesPasoCamion;

internal sealed class ListarReportesPasoCamionQueryHandler(
    IRutaRepository repo,
    IUsuarioActualService usuario) : IRequestHandler<ListarReportesPasoCamionQuery, IReadOnlyList<ReportePasoCamionDto>>
{
    public async Task<IReadOnlyList<ReportePasoCamionDto>> Handle(ListarReportesPasoCamionQuery request, CancellationToken cancellationToken)
    {
        if (!usuario.EsReciclador && !usuario.EsAdministrador) throw new AccesoDenegadoException();
        return (await repo.ReportesPasoAsync(request.RutaId, request.Maximo, cancellationToken))
            .Select(r => new ReportePasoCamionDto(r.Id, r.RutaId, r.UsuarioId, r.ReportadoEn)).ToList();
    }
}
