using MediatR;
using TrackTrash.Application.DTOs.SuscripcionesRuta;

namespace TrackTrash.Application.UseCases.SuscripcionesRuta.Queries.ListarMisSuscripciones;

public sealed record ListarMisSuscripcionesQuery : IRequest<IReadOnlyList<SuscripcionRutaDto>>;
