using TrackTrash.Application.DTOs.Perfiles;

namespace TrackTrash.Application.DTOs.Perfiles;

public sealed record PerfilDto(Guid Id, string Nombre, string Telefono, string Direccion, double? Latitud, double? Longitud,
    bool RecibirAvisos, Guid? AsociacionId, string Rol, int RachaActual = 0, int RachaMaxima = 0, PreferenciasAvisosDto? PreferenciasAvisos = null);
