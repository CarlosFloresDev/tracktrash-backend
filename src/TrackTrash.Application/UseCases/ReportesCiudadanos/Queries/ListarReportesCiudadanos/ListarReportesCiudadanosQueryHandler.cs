using MediatR;
using TrackTrash.Application.DTOs.ReportesCiudadanos;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.ReportesCiudadanos.Queries.ListarReportesCiudadanos;

internal sealed class ListarReportesCiudadanosQueryHandler(
    IContenedorRepository repo,
    IUsuarioActualService usuario) : IRequestHandler<ListarReportesCiudadanosQuery, IReadOnlyList<ReporteCiudadanoDto>>
{
    public async Task<IReadOnlyList<ReporteCiudadanoDto>> Handle(ListarReportesCiudadanosQuery request, CancellationToken cancellationToken)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException();
        return (await repo.ReportesAsync(request.ContenedorId, request.Maximo, cancellationToken))
            .Select(r => new ReporteCiudadanoDto(r.Id, r.ContenedorId, r.UsuarioId, r.NivelEstimado, r.CreadoEn)).ToList();
    }
}
