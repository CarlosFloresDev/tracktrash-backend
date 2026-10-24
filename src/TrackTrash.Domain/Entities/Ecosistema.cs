using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

public static class Ubicacion
{
    public static void Validar(double lat, double lng)
    {
        if (!double.IsFinite(lat) || !double.IsFinite(lng) || lat is < -90 or > 90 || lng is < -180 or > 180)
            throw new ReglaNegocioException("Las coordenadas no son válidas.");
    }
    public static double DistanciaKm(double lat1, double lon1, double lat2, double lon2)
    {
        var a = Math.Sin((lat2-lat1)*Math.PI/360); var b = Math.Sin((lon2-lon1)*Math.PI/360);
        return 12742 * Math.Asin(Math.Min(1, Math.Sqrt(a*a + Math.Cos(lat1*Math.PI/180)*Math.Cos(lat2*Math.PI/180)*b*b)));
    }
    public static string Texto(string? value, int min, int max, string campo)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim().Length < min || value.Trim().Length > max)
            throw new ReglaNegocioException($"{campo} debe tener entre {min} y {max} caracteres.");
        return value.Trim();
    }
}

public sealed class Distrito
{
    private Distrito() { }
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = "";
    public static Distrito Crear(Guid id, string nombre) => new() { Id = id, Nombre = Ubicacion.Texto(nombre, 2, 80, "Distrito") };
}

public sealed class PerfilUsuario
{
    private PerfilUsuario() { }
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = "Vecino TrackTrash";
    public string Telefono { get; private set; } = "";
    public string Direccion { get; private set; } = "";
    public double? Latitud { get; private set; }
    public double? Longitud { get; private set; }
    public Guid? AsociacionId { get; private set; }
    public bool RecibirAvisos { get; private set; } = true;
    public int Version { get; private set; }
    public static PerfilUsuario Crear(Guid id) => new() { Id = id };
    public void Actualizar(string nombre, string telefono, string direccion, double? lat, double? lng, bool avisos)
    {
        Nombre = Ubicacion.Texto(nombre, 2, 100, "Nombre");
        if (telefono.Length > 20 || telefono.Any(c => !char.IsAsciiDigit(c) && c is not '+' and not ' '))
            throw new ReglaNegocioException("El teléfono no es válido.");
        if (direccion.Length > 250 || lat.HasValue != lng.HasValue) throw new ReglaNegocioException("La dirección o ubicación no es válida.");
        if (lat.HasValue) Ubicacion.Validar(lat.Value, lng!.Value);
        Telefono = telefono.Trim(); Direccion = direccion.Trim(); Latitud = lat; Longitud = lng; RecibirAvisos = avisos; Version++;
    }
    public void VincularAsociacion(Guid id) { AsociacionId = id; Version++; }
    public void RegistrarOperacionSaldo() { Version++; }
}

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
}

public sealed class MaterialAceptado
{
    private MaterialAceptado() { }
    public MaterialAceptado(Guid asociacion, MaterialReciclable material) { AsociacionId=asociacion; Material=material; }
    public Guid AsociacionId { get; private set; }
    public MaterialReciclable Material { get; private set; }
}

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

public sealed class DispositivoIoT
{
    private DispositivoIoT() { }
    public Guid Id { get; private set; }
    public Guid ContenedorId { get; private set; }
    public string ClaveHash { get; private set; } = "";
    public double AlturaCm { get; private set; }
    public DateTimeOffset? UltimoContacto { get; private set; }
    public int? Bateria { get; private set; }
    public int Version { get; private set; }
    public static DispositivoIoT Crear(Guid contenedor, string hash, double altura)
    {
        if (!double.IsFinite(altura) || altura is < 20 or > 500) throw new ReglaNegocioException("Altura permitida: 20 a 500 cm.");
        return new() {Id=Guid.NewGuid(),ContenedorId=contenedor,ClaveHash=hash,AlturaCm=altura};
    }
    public void Contactar(DateTimeOffset ahora, int bateria) { UltimoContacto=ahora; Bateria=bateria; Version++; }
}

public sealed class LecturaNivel
{
    private LecturaNivel() { }
    public Guid Id { get; private set; }
    public Guid DispositivoId { get; private set; }
    public long Secuencia { get; private set; }
    public int Porcentaje { get; private set; }
    public int Bateria { get; private set; }
    public DateTimeOffset MedidaEn { get; private set; }
    public DateTimeOffset RecibidaEn { get; private set; }
    public static LecturaNivel Crear(Guid dispositivo,long secuencia,IReadOnlyList<double> distancias,double altura,int bateria,DateTimeOffset medida,DateTimeOffset ahora)
    {
        if(secuencia<0 || distancias.Count is < 3 or > 11 || distancias.Any(d=>!double.IsFinite(d)||d<0||d>altura)
            || bateria is < 0 or > 100 || medida>ahora.AddMinutes(2) || medida<ahora.AddDays(-7))
            throw new ReglaNegocioException("La lectura requiere 3–11 distancias válidas, secuencia, batería y fecha dentro de siete días.");
        var orden=distancias.Order().ToArray(); var mitad=orden.Length/2;
        var mediana=orden.Length%2==1?orden[mitad]:(orden[mitad-1]+orden[mitad])/2;
        return new() {Id=Guid.NewGuid(),DispositivoId=dispositivo,Secuencia=secuencia,Porcentaje=(int)Math.Round(100*(1-mediana/altura)),Bateria=bateria,MedidaEn=medida.ToUniversalTime(),RecibidaEn=ahora};
    }
}

public enum EstadoIncidencia { Abierta, EnAtencion, Resuelta }
public sealed class Incidencia
{
    private Incidencia() { }
    public Guid Id { get; private set; }
    public Guid ContenedorId { get; private set; }
    public Guid ReportadaPor { get; private set; }
    public string Tipo { get; private set; } = "";
    public string Descripcion { get; private set; } = "";
    public EstadoIncidencia Estado { get; private set; }
    public Guid? ResponsableId { get; private set; }
    public string? Resolucion { get; private set; }
    public DateTimeOffset CreadaEn { get; private set; }
    public DateTimeOffset? ResueltaEn { get; private set; }
    public int Version { get; private set; }
    public static Incidencia Crear(Guid contenedor,Guid actor,string tipo,string descripcion,DateTimeOffset ahora) =>
        new(){Id=Guid.NewGuid(),ContenedorId=contenedor,ReportadaPor=actor,Tipo=Ubicacion.Texto(tipo,3,60,"Tipo"),Descripcion=Ubicacion.Texto(descripcion,10,1000,"Descripción"),CreadaEn=ahora};
    public void Atender(Guid responsable)
    {
        if(Estado!=EstadoIncidencia.Abierta) throw new ReglaNegocioException("La incidencia ya está siendo atendida o resuelta.");
        Estado=EstadoIncidencia.EnAtencion;ResponsableId=responsable;Version++;
    }
    public void Resolver(Guid responsable,string detalle,DateTimeOffset ahora)
    {
        if(Estado!=EstadoIncidencia.EnAtencion || ResponsableId!=responsable) throw new ReglaNegocioException("Solo el responsable asignado puede resolver una incidencia en atención.");
        Resolucion=Ubicacion.Texto(detalle,10,1000,"Resolución");Estado=EstadoIncidencia.Resuelta;ResueltaEn=ahora;Version++;
    }
}

public sealed class Notificacion
{
    private Notificacion() { }
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public string ClaveUnica { get; private set; } = "";
    public string Titulo { get; private set; } = "";
    public string Mensaje { get; private set; } = "";
    public DateTimeOffset CreadaEn { get; private set; }
    public bool Leida { get; private set; }
    public static Notificacion Crear(Guid usuario,string clave,string titulo,string mensaje,DateTimeOffset ahora) =>
        new(){Id=Guid.NewGuid(),UsuarioId=usuario,ClaveUnica=clave,Titulo=titulo,Mensaje=mensaje,CreadaEn=ahora};
    public void Leer() => Leida=true;
}

public sealed class Beneficio
{
    private Beneficio() { }
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = "";
    public string Descripcion { get; private set; } = "";
    public int CostoPuntos { get; private set; }
    public int Stock { get; private set; }
    public bool Activo { get; private set; } = true;
    public bool Demostracion { get; private set; }
    public int Version { get; private set; }
    public static Beneficio Crear(string nombre,string descripcion,int costo,int stock,bool demo=false)
    {
        if(costo<=0||stock<0) throw new ReglaNegocioException("Costo y stock no válidos.");
        return new(){Id=Guid.NewGuid(),Nombre=Ubicacion.Texto(nombre,3,120,"Nombre"),Descripcion=Ubicacion.Texto(descripcion,5,500,"Descripción"),CostoPuntos=costo,Stock=stock,Demostracion=demo};
    }
    public void Reservar(int saldo)
    {
        if(!Activo||Stock<1) throw new ReglaNegocioException("Este beneficio no tiene stock disponible.");
        if(saldo<CostoPuntos) throw new ReglaNegocioException("Todavía no tienes suficientes puntos.");
        Stock--;Version++;
    }
}

public sealed class Canje
{
    private Canje() { }
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Guid BeneficioId { get; private set; }
    public Guid Clave { get; private set; }
    public int PuntosUsados { get; private set; }
    public string Codigo { get; private set; } = "";
    public DateTimeOffset CreadoEn { get; private set; }
    public static Canje Crear(Guid usuario,Beneficio b,Guid clave,string codigo,DateTimeOffset ahora)
    {
        if(clave==Guid.Empty) throw new ReglaNegocioException("El canje necesita una clave de operación.");
        return new(){Id=Guid.NewGuid(),UsuarioId=usuario,BeneficioId=b.Id,Clave=clave,Codigo=codigo,PuntosUsados=b.CostoPuntos,CreadoEn=ahora};
    }
}

public sealed class HistoricoSolicitud
{
    private HistoricoSolicitud() { }
    public Guid Id { get; private set; }
    public Guid SolicitudId { get; private set; }
    public Guid ActorId { get; private set; }
    public string Accion { get; private set; } = "";
    public DateTimeOffset CreadoEn { get; private set; }
    public static HistoricoSolicitud Crear(Guid solicitud,Guid actor,string accion,DateTimeOffset ahora) => new(){Id=Guid.NewGuid(),SolicitudId=solicitud,ActorId=actor,Accion=accion,CreadoEn=ahora};
}
