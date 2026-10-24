using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

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

