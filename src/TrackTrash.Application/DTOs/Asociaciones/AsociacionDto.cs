using TrackTrash.Domain.Entities;

namespace TrackTrash.Application.DTOs.Asociaciones;

public sealed record AsociacionDto(Guid Id, string Nombre, string Direccion, string Horario, double Latitud, double Longitud,
    decimal CapacidadKg, bool Verificada, bool Demostracion, string[] Materiales, double? DistanciaKm = null);

public static class AsociacionMapping
{
    public static AsociacionDto ToDto(this AsociacionReciclaje a, double? distancia = null) => new(a.Id, a.Nombre, a.Direccion, a.Horario,
        a.Latitud, a.Longitud, a.CapacidadKg, a.Verificada, a.Demostracion, a.Materiales.Select(m => m.Material.ToString()).ToArray(), distancia);
}
