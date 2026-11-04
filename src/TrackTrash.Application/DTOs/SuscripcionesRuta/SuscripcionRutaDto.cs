namespace TrackTrash.Application.DTOs.SuscripcionesRuta;

public sealed record SuscripcionRutaDto(Guid Id, Guid RutaId, int MinutosAntes, DateTimeOffset ProximoAviso);
