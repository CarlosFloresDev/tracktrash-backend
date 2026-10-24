using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

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

