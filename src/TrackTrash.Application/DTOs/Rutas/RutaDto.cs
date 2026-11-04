namespace TrackTrash.Application.DTOs.Rutas;

public sealed record RutaDto(Guid Id, string Nombre, string Zona, string Dias, string HoraLocal, DateTimeOffset ProximoPaso, bool Demostracion, bool Suscrito, int? MinutosAntes = null);
