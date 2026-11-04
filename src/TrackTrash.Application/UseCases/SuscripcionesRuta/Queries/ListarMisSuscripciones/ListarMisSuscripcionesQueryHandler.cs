using MediatR;
using TrackTrash.Application.DTOs.SuscripcionesRuta;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SuscripcionesRuta.Queries.ListarMisSuscripciones;

internal sealed class ListarMisSuscripcionesQueryHandler(
    IRutaRepository repo,
    IUsuarioActualService usuario) : IRequestHandler<ListarMisSuscripcionesQuery, IReadOnlyList<SuscripcionRutaDto>>
{
    public async Task<IReadOnlyList<SuscripcionRutaDto>> Handle(ListarMisSuscripcionesQuery request, CancellationToken cancellationToken) =>
        (await repo.SuscripcionesAsync(usuario.Id, cancellationToken))
            .Select(s => new SuscripcionRutaDto(s.Id, s.RutaId, s.MinutosAntes, s.ProximoAviso)).ToList();
}
