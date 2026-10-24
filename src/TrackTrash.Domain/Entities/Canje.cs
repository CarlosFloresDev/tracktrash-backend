using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

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

