using TrackTrash.Domain.Entities;

namespace TrackTrash.Domain.Ports.Repositories;

public interface IRutaRepository
{
    Task<IReadOnlyList<RutaRecoleccion>> ListarAsync(CancellationToken ct);
    Task<RutaRecoleccion?> ObtenerAsync(Guid id, CancellationToken ct);
    Task<SuscripcionRuta?> SuscripcionAsync(Guid rutaId, Guid usuarioId, CancellationToken ct);
    Task<IReadOnlyList<SuscripcionRuta>> SuscripcionesAsync(Guid usuarioId, CancellationToken ct);
    Task<IReadOnlyList<SuscripcionRuta>> PendientesAsync(DateTimeOffset ahora, CancellationToken ct);
    Task<bool> ReporteRecienteAsync(Guid rutaId, Guid usuarioId, DateTimeOffset desde, CancellationToken ct);
    void Agregar(SuscripcionRuta suscripcion);
    void Agregar(ReportePasoCamion reporte);
    void Quitar(SuscripcionRuta suscripcion);
}
