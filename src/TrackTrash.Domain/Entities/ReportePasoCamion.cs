namespace TrackTrash.Domain.Entities;
public sealed class ReportePasoCamion
{
    private ReportePasoCamion(){}
    public Guid Id{get;private set;}
    public Guid RutaId{get;private set;}
    public Guid UsuarioId{get;private set;}
    public DateTimeOffset ReportadoEn{get;private set;}
    public static ReportePasoCamion Crear(Guid ruta,Guid usuario,DateTimeOffset ahora)=>new(){Id=Guid.NewGuid(),RutaId=ruta,UsuarioId=usuario,ReportadoEn=ahora};
}
