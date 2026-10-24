using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

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

