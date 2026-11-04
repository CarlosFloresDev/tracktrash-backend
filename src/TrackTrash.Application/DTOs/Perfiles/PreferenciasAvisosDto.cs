namespace TrackTrash.Application.DTOs.Perfiles;

public sealed record PreferenciasAvisosDto(bool Horarios = true, bool Recojos = true, bool Incidencias = true, bool Recompensas = true);
