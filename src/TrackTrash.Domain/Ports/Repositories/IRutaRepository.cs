using TrackTrash.Domain.Entities;
namespace TrackTrash.Domain.Ports.Repositories;

public interface IRutaRepository
{
    Task<IReadOnlyList<RutaRecoleccion>> ListarAsync(CancellationToken ct);
    Task<RutaRecoleccion?> ObtenerAsync(Guid id, CancellationToken ct);
    Task<SuscripcionRuta?> SuscripcionAsync(Guid ruta, Guid usuario, CancellationToken ct);
    Task<IReadOnlyList<SuscripcionRuta>> SuscripcionesAsync(Guid usuario, CancellationToken ct);
    Task<IReadOnlyList<SuscripcionRuta>> PendientesAsync(DateTimeOffset ahora, CancellationToken ct);
    Task<bool> ReporteRecienteAsync(Guid ruta, Guid usuario, DateTimeOffset desde, CancellationToken ct);
    void Agregar(SuscripcionRuta s);
    void Agregar(ReportePasoCamion r);
    void Quitar(SuscripcionRuta s);
}
