namespace TrackTrash.Domain.Entities;

public sealed class MovimientoPuntos
{
    private MovimientoPuntos() { }
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Guid SolicitudId { get; private set; }
    public int Puntos { get; private set; }
    public DateTimeOffset CreadoEn { get; private set; }

    // Recompensa inicial por entrega confirmada; no acredita peso verificado.
    public static MovimientoPuntos PorEntrega(SolicitudRecojo solicitud, DateTimeOffset ahora)
    {
        if (solicitud.Estado != EstadoSolicitud.Confirmada)
            throw new Exceptions.ReglaNegocioException("No se otorgan puntos por una entrega sin confirmar.");
        return new() { Id = Guid.NewGuid(), UsuarioId = solicitud.VecinoId,
            SolicitudId = solicitud.Id, Puntos = 20, CreadoEn = ahora.ToUniversalTime() };
    }
}
