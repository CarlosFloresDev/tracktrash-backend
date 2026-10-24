namespace TrackTrash.Domain.Entities;

public sealed class ReporteCiudadano
{
    private ReporteCiudadano() { }
    public Guid Id { get; private set; }
    public Guid ContenedorId { get; private set; }
    public Guid UsuarioId { get; private set; }
    public int NivelEstimado { get; private set; }
    public DateTimeOffset CreadoEn { get; private set; }
    public static ReporteCiudadano Crear(Guid contenedor, Guid usuario, int porcentaje, DateTimeOffset ahora)
    { if (porcentaje is < 0 or > 100) throw new Exceptions.ReglaNegocioException("El nivel debe estar entre 0 y 100%."); return new() { Id = Guid.NewGuid(), ContenedorId = contenedor, UsuarioId = usuario, NivelEstimado = porcentaje, CreadoEn = ahora }; }
}
