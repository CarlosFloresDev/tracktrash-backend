namespace TrackTrash.Application.DTOs.PuntosAcopio;

public sealed record PuntoAcopioDto(Guid Id, string Nombre, string Direccion, double Latitud, double Longitud, bool Demostracion);
