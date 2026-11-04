using TrackTrash.Application.DTOs.MovimientosPuntos;
using MediatR;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.MovimientosPuntos.Queries.ObtenerImpacto;

internal sealed class ObtenerImpactoQueryHandler(
    IMovimientoPuntosRepository puntos,
    IUsuarioActualService usuario) : IRequestHandler<ObtenerImpactoQuery, ImpactoDto>
{
    public async Task<ImpactoDto> Handle(ObtenerImpactoQuery request, CancellationToken cancellationToken) =>
        new(await puntos.ObtenerTotalAsync(usuario.Id, cancellationToken));
}
