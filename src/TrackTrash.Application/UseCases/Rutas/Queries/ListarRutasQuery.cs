using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Rutas.Queries;

public sealed record ListarRutasQuery : IRequest<IReadOnlyList<RutaDto>>;

internal sealed class ListarRutasQueryHandler(
    IRutaRepository repo,
    IUsuarioActualService usuario,
    TimeProvider reloj) : IRequestHandler<ListarRutasQuery, IReadOnlyList<RutaDto>>
{
    public async Task<IReadOnlyList<RutaDto>> Handle(ListarRutasQuery request, CancellationToken cancellationToken)
    {
        var suscripciones = (await repo.SuscripcionesAsync(usuario.Id, cancellationToken)).ToDictionary(s => s.RutaId);
        return (await repo.ListarAsync(cancellationToken)).Select(r =>
        {
            suscripciones.TryGetValue(r.Id, out var suscripcion);
            return new RutaDto(r.Id, r.Nombre, r.Zona, r.Dias, r.HoraLocal.ToString("HH:mm"),
                r.ProximoPaso(reloj.GetUtcNow()), r.Demostracion, suscripcion is not null, suscripcion?.MinutosAntes);
        }).ToList();
    }
}
