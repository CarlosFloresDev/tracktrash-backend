using MediatR;
using TrackTrash.Application.DTOs.SuscripcionesRuta;

namespace TrackTrash.Application.UseCases.SuscripcionesRuta.Queries.ObtenerSuscripcionRuta;

public sealed record ObtenerSuscripcionRutaQuery : IRequest<SuscripcionRutaDto>
{
    public Guid RutaId { get; init; }
}
