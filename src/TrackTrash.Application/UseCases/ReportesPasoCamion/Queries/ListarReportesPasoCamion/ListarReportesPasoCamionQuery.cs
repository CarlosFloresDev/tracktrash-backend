using MediatR;
using TrackTrash.Application.DTOs.ReportesPasoCamion;

namespace TrackTrash.Application.UseCases.ReportesPasoCamion.Queries.ListarReportesPasoCamion;

public sealed record ListarReportesPasoCamionQuery : IRequest<IReadOnlyList<ReportePasoCamionDto>>
{
    public Guid RutaId { get; init; }
    public int Maximo { get; init; } = 50;
}
