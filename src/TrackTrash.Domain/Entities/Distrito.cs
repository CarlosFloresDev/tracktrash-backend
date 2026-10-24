using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

public sealed class Distrito
{
    private Distrito() { }
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = "";
    public static Distrito Crear(Guid id, string nombre) => new() { Id = id, Nombre = Ubicacion.Texto(nombre, 2, 80, "Distrito") };
}

