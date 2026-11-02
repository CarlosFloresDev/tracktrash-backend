using MediatR;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Recompensas.Queries;

public sealed record ObtenerImpactoQuery : IRequest<ImpactoDto>;

internal sealed class ObtenerImpactoQueryHandler(
    IMovimientoPuntosRepository puntos,
    IUsuarioActualService usuario) : IRequestHandler<ObtenerImpactoQuery, ImpactoDto>
{
    public async Task<ImpactoDto> Handle(ObtenerImpactoQuery request, CancellationToken cancellationToken) =>
        new(await puntos.ObtenerTotalAsync(usuario.Id, cancellationToken));
}
