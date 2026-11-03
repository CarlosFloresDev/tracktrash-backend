using MediatR;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.MovimientosPuntos.Queries.ObtenerImpacto;

internal sealed class ObtenerImpactoQueryHandler(IMovimientoPuntosRepository puntos, IUsuarioActualService usuario)
    : IRequestHandler<ObtenerImpactoQuery, ImpactoDto>
{
    public async Task<ImpactoDto> Handle(ObtenerImpactoQuery request, CancellationToken ct) =>
        new(await puntos.ObtenerTotalAsync(usuario.Id, ct));
}
