namespace TrackTrash.Domain.Entities;

public sealed class SuscripcionRuta
{
    private SuscripcionRuta() { }
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Guid RutaId { get; private set; }
    public int MinutosAntes { get; private set; }
    public DateTimeOffset ProximoAviso { get; private set; }
    public int Version { get; private set; }
    public static SuscripcionRuta Crear(Guid usuario, RutaRecoleccion ruta, int minutos, DateTimeOffset ahora)
    {
        if (minutos is < 5 or > 60) throw new Exceptions.ReglaNegocioException("El aviso debe anticiparse entre 5 y 60 minutos.");
        var s = new SuscripcionRuta { Id = Guid.NewGuid(), UsuarioId = usuario, RutaId = ruta.Id, MinutosAntes = minutos }; s.Programar(ruta, ahora); return s;
    }
    public void Programar(RutaRecoleccion ruta, DateTimeOffset ahora) { ProximoAviso = ruta.ProximoPaso(ahora.AddMinutes(MinutosAntes)).AddMinutes(-MinutosAntes); Version++; }
}
