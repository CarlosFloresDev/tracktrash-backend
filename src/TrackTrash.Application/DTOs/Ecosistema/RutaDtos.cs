namespace TrackTrash.Application.DTOs.Ecosistema;

public sealed record RutaDto(Guid Id, string Nombre, string Zona, string Dias, string HoraLocal, DateTimeOffset ProximoPaso, bool Demostracion, bool Suscrito);
public sealed record SuscribirRutaRequest(int MinutosAntes = 20);
