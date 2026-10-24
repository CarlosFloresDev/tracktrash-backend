using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

public sealed class PuntoAcopio
{
    private PuntoAcopio() { }
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = "";
    public string Direccion { get; private set; } = "";
    public double Latitud { get; private set; }
    public double Longitud { get; private set; }
    public bool Demostracion { get; private set; }
    public static PuntoAcopio Crear(string nombre, string direccion, double lat, double lng, bool demo)
    { Ubicacion.Validar(lat,lng); return new() { Id=Guid.NewGuid(), Nombre=nombre,Direccion=direccion,Latitud=lat,Longitud=lng,Demostracion=demo }; }
}

