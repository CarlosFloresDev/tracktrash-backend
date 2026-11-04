using TrackTrash.Domain.Entities;

namespace TrackTrash.Domain.Ports.Repositories;

public interface IContenedorRepository
{
    Task<IReadOnlyList<Contenedor>> CercanosAsync(double lat, double lng, double km, CancellationToken ct);
    Task<Contenedor?> ObtenerAsync(Guid id, CancellationToken ct);
    Task<DispositivoIoT?> DispositivoAsync(Guid id, CancellationToken ct);
    Task<bool> LecturaExisteAsync(Guid dispositivoId, long secuencia, CancellationToken ct);
    Task<bool> ReporteRecienteAsync(Guid contenedorId, Guid usuarioId, DateTimeOffset desde, CancellationToken ct);
    void Agregar(Contenedor contenedor);
    void Agregar(DispositivoIoT dispositivo);
    void Agregar(LecturaNivel lectura);
    void Agregar(ReporteCiudadano reporte);
}
