namespace TrackTrash.Domain.Entities;

public sealed class EventoAuditoria
{
    private EventoAuditoria() { }
    public Guid Id { get; private set; }
    public string Actor { get; private set; } = "";
    public string Entidad { get; private set; } = "";
    public string Referencia { get; private set; } = "";
    public string Accion { get; private set; } = "";
    public string Campos { get; private set; } = "";
    public DateTimeOffset CreadoEn { get; private set; }
    public static EventoAuditoria Crear(string actor, string entidad, string referencia, string accion, string campos, DateTimeOffset ahora) =>
        new() { Id = Guid.NewGuid(), Actor = actor, Entidad = entidad, Referencia = referencia, Accion = accion, Campos = campos, CreadoEn = ahora };
}
