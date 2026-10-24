using TrackTrash.Domain.Exceptions;
namespace TrackTrash.Domain.Entities;
public sealed class RutaRecoleccion
{
    private RutaRecoleccion(){}
    public Guid Id{get;private set;}
    public Guid DistritoId{get;private set;}
    public string Nombre{get;private set;}="";
    public string Zona{get;private set;}="";
    public string Dias{get;private set;}="";
    public TimeOnly HoraLocal{get;private set;}
    public bool Demostracion{get;private set;}
    public static RutaRecoleccion Crear(Guid distrito,string nombre,string zona,int[] dias,TimeOnly hora,bool demo=false)
    {
        if(dias.Length==0||dias.Any(d=>d is <0 or >6))throw new ReglaNegocioException("Selecciona días válidos de la semana.");
        return new(){Id=Guid.NewGuid(),DistritoId=distrito,Nombre=Ubicacion.Texto(nombre,3,120,"Ruta"),Zona=Ubicacion.Texto(zona,3,150,"Zona"),Dias=string.Join(',',dias.Distinct().Order()),HoraLocal=hora,Demostracion=demo};
    }
    public DateTimeOffset ProximoPaso(DateTimeOffset ahora)
    {
        var local=ahora.ToOffset(TimeSpan.FromHours(-5));var dias=Dias.Split(',').Select(int.Parse).ToHashSet();
        for(var d=0;d<=7;d++)
        {
            var fecha=local.Date.AddDays(d);if(!dias.Contains((int)fecha.DayOfWeek))continue;
            var candidato=new DateTimeOffset(fecha.Year,fecha.Month,fecha.Day,HoraLocal.Hour,HoraLocal.Minute,0,TimeSpan.FromHours(-5));
            if(candidato>ahora)return candidato.ToUniversalTime();
        }
        throw new ReglaNegocioException("La ruta no tiene próximos horarios.");
    }
}
