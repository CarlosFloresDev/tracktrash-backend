using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

public sealed class HistoricoSolicitud
{
    private HistoricoSolicitud() { }
    public Guid Id { get; private set; }
    public Guid SolicitudId { get; private set; }
    public Guid ActorId { get; private set; }
    public string Accion { get; private set; } = "";
    public DateTimeOffset CreadoEn { get; private set; }
    public static HistoricoSolicitud Crear(Guid solicitud, Guid actor, string accion, DateTimeOffset ahora) => new() { Id = Guid.NewGuid(), SolicitudId = solicitud, ActorId = actor, Accion = accion, CreadoEn = ahora };
}
