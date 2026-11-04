using TrackTrash.Domain.Entities;

namespace TrackTrash.Application.DTOs.Contenedores;

public sealed record ContenedorDto(Guid Id, Guid DistritoId, string Nombre, double Latitud, double Longitud, int? Porcentaje,
    string Estado, string Fuente, DateTimeOffset? ActualizadoEn, bool Demostracion);

public static class ContenedorMapping
{
    public static ContenedorDto ToDto(this Contenedor c, DateTimeOffset ahora) => new(c.Id, c.DistritoId, c.Nombre, c.Latitud, c.Longitud,
        c.Porcentaje, c.Estado(ahora), c.Fuente, c.ActualizadoEn, c.Demostracion);
}
