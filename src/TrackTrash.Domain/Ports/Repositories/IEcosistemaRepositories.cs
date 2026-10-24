using TrackTrash.Domain.Entities;

namespace TrackTrash.Domain.Ports.Repositories;

public interface IPerfilRepository
{
    Task<PerfilUsuario?> ObtenerAsync(Guid usuario, CancellationToken ct);
    void Agregar(PerfilUsuario perfil);
}
public interface ICatalogoRepository
{
    Task<IReadOnlyList<Distrito>> DistritosAsync(CancellationToken ct);
    Task<IReadOnlyList<AsociacionReciclaje>> AsociacionesAsync(CancellationToken ct);
    Task<AsociacionReciclaje?> AsociacionAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<AsociacionReciclaje>> CandidatosAsync(MaterialReciclable material,decimal kg,double lat,double lng,CancellationToken ct);
    Task<IReadOnlyList<PuntoAcopio>> PuntosAsync(CancellationToken ct);
    void Agregar(AsociacionReciclaje asociacion);
}
public interface IContenedorRepository
{
    Task<IReadOnlyList<Contenedor>> CercanosAsync(double lat,double lng,double radioKm,CancellationToken ct);
    Task<Contenedor?> ObtenerAsync(Guid id,CancellationToken ct);
    Task<DispositivoIoT?> DispositivoAsync(Guid id,CancellationToken ct);
    Task<bool> LecturaExisteAsync(Guid dispositivo,long secuencia,CancellationToken ct);
    void Agregar(Contenedor contenedor);
    void Agregar(DispositivoIoT dispositivo);
    void Agregar(LecturaNivel lectura);
}
public interface IIncidenciaRepository
{
    Task<IReadOnlyList<Incidencia>> ListarAsync(Guid usuario,bool operador,CancellationToken ct);
    Task<Incidencia?> ObtenerAsync(Guid id,CancellationToken ct);
    Task<bool> SaturacionAbiertaAsync(Guid contenedor,CancellationToken ct);
    void Agregar(Incidencia incidencia);
}
public interface INotificacionRepository
{
    Task<IReadOnlyList<Notificacion>> ListarAsync(Guid usuario,CancellationToken ct);
    Task<Notificacion?> ObtenerAsync(Guid id,CancellationToken ct);
    void Agregar(Notificacion notificacion);
}
public interface IBeneficioRepository
{
    Task<IReadOnlyList<Beneficio>> ListarAsync(CancellationToken ct);
    Task<Beneficio?> ObtenerAsync(Guid id,CancellationToken ct);
    Task<Canje?> PorClaveAsync(Guid usuario,Guid clave,CancellationToken ct);
    Task<IReadOnlyList<Canje>> CanjesAsync(Guid usuario,CancellationToken ct);
    void Agregar(Beneficio beneficio);
    void Agregar(Canje canje);
}
public interface IHistoricoRepository
{
    void Agregar(HistoricoSolicitud evento);
    Task<IReadOnlyList<HistoricoSolicitud>> ListarAsync(Guid solicitud,CancellationToken ct);
}
public sealed record IndicadoresOperacion(int Contenedores,int Saturados,int IncidenciasAbiertas,int RecojosConfirmados,decimal KgEstimadosConfirmados);
public interface IIndicadoresRepository
{
    Task<IndicadoresOperacion> ObtenerAsync(DateTimeOffset ahora,CancellationToken ct);
}
