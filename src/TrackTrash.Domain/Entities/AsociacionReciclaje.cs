using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

public sealed class AsociacionReciclaje
{
    private AsociacionReciclaje() { }
    public Guid Id { get; private set; }
    public Guid DistritoId { get; private set; }
    public string Nombre { get; private set; } = "";
    public string Direccion { get; private set; } = "";
    public string Horario { get; private set; } = "";
    public double Latitud { get; private set; }
    public double Longitud { get; private set; }
    public decimal CapacidadKg { get; private set; }
    public decimal ReservadoKg { get; private set; }
    public int Version { get; private set; }
    public bool Verificada { get; private set; }
    public bool Demostracion { get; private set; }
    public List<MaterialAceptado> Materiales { get; private set; } = [];
    public static AsociacionReciclaje Crear(Guid id, Guid distrito, string nombre, string direccion, double lat, double lng,
        decimal capacidad, IEnumerable<MaterialReciclable> materiales, bool demo = false)
    {
        Ubicacion.Validar(lat,lng);
        if (capacidad <= 0 || capacidad > 100000) throw new ReglaNegocioException("La capacidad no es válida.");
        var items = materiales.Distinct().ToArray();
        if (items.Length == 0 || items.Any(m => !Enum.IsDefined(m))) throw new ReglaNegocioException("Selecciona materiales válidos.");
        var a = new AsociacionReciclaje { Id=id, DistritoId=distrito, Nombre=Ubicacion.Texto(nombre,2,120,"Nombre"),
            Direccion=Ubicacion.Texto(direccion,5,250,"Dirección"), Latitud=lat, Longitud=lng, CapacidadKg=capacidad,
            Demostracion=demo, Horario="Lunes a sábado · 08:00–17:00" };
        a.Materiales = items.Select(m => new MaterialAceptado(id,m)).ToList();
        return a;
    }
    public void Verificar() => Verificada = true;
    public void Reservar(decimal kg,MaterialReciclable material)
    {
        if(!Verificada||!Materiales.Any(m=>m.Material==material)||kg>CapacidadKg-ReservadoKg)throw new ReglaNegocioException("La asociación no tiene capacidad o no recibe ese material.");
        ReservadoKg+=kg;Version++;
    }
    public void Liberar(decimal kg){ReservadoKg=Math.Max(0,ReservadoKg-kg);Version++;}
}

