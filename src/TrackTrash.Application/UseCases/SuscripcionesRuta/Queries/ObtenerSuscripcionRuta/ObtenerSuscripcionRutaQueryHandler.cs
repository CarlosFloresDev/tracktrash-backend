using MediatR;
using TrackTrash.Application.DTOs.SuscripcionesRuta;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SuscripcionesRuta.Queries.ObtenerSuscripcionRuta;

internal sealed class ObtenerSuscripcionRutaQueryHandler(
    IRutaRepository repo,
    IUsuarioActualService usuario) : IRequestHandler<ObtenerSuscripcionRutaQuery, SuscripcionRutaDto>
{
    public async Task<SuscripcionRutaDto> Handle(ObtenerSuscripcionRutaQuery request, CancellationToken cancellationToken)
    {
        var s = await repo.SuscripcionAsync(request.RutaId, usuario.Id, cancellationToken) ?? throw new RecursoNoEncontradoException();
        return new SuscripcionRutaDto(s.Id, s.RutaId, s.MinutosAntes, s.ProximoAviso);
    }
}
