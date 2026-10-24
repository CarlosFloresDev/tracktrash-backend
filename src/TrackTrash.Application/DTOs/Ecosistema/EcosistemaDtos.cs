using System.ComponentModel.DataAnnotations;
using TrackTrash.Domain.Entities;
namespace TrackTrash.Application.DTOs.Ecosistema;

public sealed record ActualizarPerfilRequest([Required] string Nombre, string Telefono, string Direccion, double? Latitud, double? Longitud, bool RecibirAvisos = true);
public sealed record PerfilDto(Guid Id, string Nombre, string Telefono, string Direccion, double? Latitud, double? Longitud, bool RecibirAvisos, Guid? AsociacionId, string Rol, int RachaActual = 0, int RachaMaxima = 0);
public sealed record AsociacionDto(Guid Id, string Nombre, string Direccion, string Horario, double Latitud, double Longitud, decimal CapacidadKg, bool Verificada, bool Demostracion, string[] Materiales, double? DistanciaKm = null);
public sealed record CrearAsociacionRequest(Guid DistritoId, string Nombre, string Direccion, double Latitud, double Longitud, decimal CapacidadKg, MaterialReciclable[] Materiales);
public sealed record ContenedorDto(Guid Id, Guid DistritoId, string Nombre, double Latitud, double Longitud, int? Porcentaje, string Estado, string Fuente, DateTimeOffset? ActualizadoEn, bool Demostracion);
public sealed record CrearContenedorRequest(Guid DistritoId, string Nombre, double Latitud, double Longitud);
public sealed record MapaDto(IReadOnlyList<ContenedorDto> Contenedores, IReadOnlyList<AsociacionDto> Asociaciones, IReadOnlyList<PuntoAcopioDto> PuntosAcopio);
public sealed record PuntoAcopioDto(Guid Id, string Nombre, string Direccion, double Latitud, double Longitud, bool Demostracion);
public sealed record DistritoDto(Guid Id, string Nombre);
public sealed record CrearIncidenciaRequest(Guid ContenedorId, string Tipo, string Descripcion);
public sealed record ResolverIncidenciaRequest(string Resolucion);
public sealed record IncidenciaDto(Guid Id, Guid ContenedorId, string Tipo, string Descripcion, string Estado, string? Resolucion, DateTimeOffset CreadaEn, DateTimeOffset? ResueltaEn);
public sealed record NotificacionDto(Guid Id, string Titulo, string Mensaje, DateTimeOffset CreadaEn, bool Leida);
public sealed record BeneficioDto(Guid Id, string Nombre, string Descripcion, int CostoPuntos, int Stock, bool Demostracion);
public sealed record CrearBeneficioRequest(string Nombre, string Descripcion, int CostoPuntos, int Stock);
public sealed record CanjearRequest(Guid BeneficioId, Guid Clave);
public sealed record CanjeDto(Guid Id, Guid BeneficioId, int PuntosUsados, string Codigo, DateTimeOffset CreadoEn);
public sealed record RegistrarDispositivoRequest(Guid ContenedorId, double AlturaCm);
public sealed record DispositivoRegistradoDto(Guid Id, Guid ContenedorId, string Clave);
public sealed record LecturaRequest(long Secuencia, double[] DistanciasCm, int Bateria, DateTimeOffset MedidaEn);
public sealed record LecturaResultadoDto(bool Duplicada, int? Porcentaje, string Estado);
public sealed record HistoricoDto(string Accion, DateTimeOffset CreadoEn);

public static class MapeoEcosistema
{
    public static AsociacionDto Dto(this AsociacionReciclaje a, double? distancia = null) => new(a.Id, a.Nombre, a.Direccion, a.Horario, a.Latitud, a.Longitud, a.CapacidadKg, a.Verificada, a.Demostracion, a.Materiales.Select(m => m.Material.ToString()).ToArray(), distancia);
    public static ContenedorDto Dto(this Contenedor c, DateTimeOffset ahora) => new(c.Id, c.DistritoId, c.Nombre, c.Latitud, c.Longitud, c.Porcentaje, c.Estado(ahora), c.Fuente, c.ActualizadoEn, c.Demostracion);
    public static IncidenciaDto Dto(this Incidencia i) => new(i.Id, i.ContenedorId, i.Tipo, i.Descripcion, i.Estado.ToString(), i.Resolucion, i.CreadaEn, i.ResueltaEn);
    public static CanjeDto Dto(this Canje c) => new(c.Id, c.BeneficioId, c.PuntosUsados, c.Codigo, c.CreadoEn);
}
