using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

public sealed class Contenedor
{
    private Contenedor() { }
    public Guid Id { get; private set; }
    public Guid DistritoId { get; private set; }
    public string Nombre { get; private set; } = "";
    public double Latitud { get; private set; }
    public double Longitud { get; private set; }
    public int? Porcentaje { get; private set; }
    public DateTimeOffset? ActualizadoEn { get; private set; }
    public string Fuente { get; private set; } = "Sin datos";
    public bool Demostracion { get; private set; }
    public int Version { get; private set; }
    public static Contenedor Crear(Guid distrito, string nombre, double lat, double lng, bool demo = false)
    { Ubicacion.Validar(lat,lng); return new() { Id=Guid.NewGuid(),DistritoId=distrito,Nombre=Ubicacion.Texto(nombre,3,120,"Nombre"),Latitud=lat,Longitud=lng,Demostracion=demo }; }
    public void ActualizarNivel(int porcentaje, DateTimeOffset medida, string fuente)
    {
        if (porcentaje is < 0 or > 100) throw new ReglaNegocioException("El nivel debe estar entre 0 y 100.");
        if (ActualizadoEn.HasValue && medida <= ActualizadoEn) return;
        Porcentaje=porcentaje; ActualizadoEn=medida.ToUniversalTime(); Fuente=fuente; Version++;
    }
    public string Estado(DateTimeOffset ahora) => !ActualizadoEn.HasValue || ahora-ActualizadoEn > TimeSpan.FromMinutes(30)
        ? "SinDatosRecientes" : Porcentaje > 80 ? "Saturado" : Porcentaje >= 40 ? "Atencion" : "Disponible";
}

