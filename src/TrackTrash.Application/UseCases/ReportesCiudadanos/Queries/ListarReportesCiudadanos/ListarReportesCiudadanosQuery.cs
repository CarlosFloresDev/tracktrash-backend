using MediatR;
using TrackTrash.Application.DTOs.ReportesCiudadanos;

namespace TrackTrash.Application.UseCases.ReportesCiudadanos.Queries.ListarReportesCiudadanos;

public sealed record ListarReportesCiudadanosQuery : IRequest<IReadOnlyList<ReporteCiudadanoDto>>
{
    public Guid ContenedorId { get; init; }
    public int Maximo { get; init; } = 50;
}
