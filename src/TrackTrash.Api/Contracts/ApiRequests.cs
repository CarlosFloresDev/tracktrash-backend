using TrackTrash.Application.DTOs.Perfiles;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Api.Contracts;

public sealed record CrearAsociacionRequest(Guid DistritoId, string Nombre, string Direccion,
    double Latitud, double Longitud, decimal CapacidadKg, MaterialReciclable[] Materiales);
public sealed record CrearContenedorRequest(Guid DistritoId, string Nombre, double Latitud, double Longitud);
public sealed record ReportarNivelRequest(int Porcentaje);
public sealed record RegistrarDispositivoRequest(Guid ContenedorId, double AlturaCm);
public sealed record RegistrarLecturaRequest(Guid DispositivoId, string ClaveDispositivo, long Secuencia,
    double[] DistanciasCm, int Bateria, DateTimeOffset MedidaEn);
public sealed record CrearIncidenciaRequest(Guid ContenedorId, string Tipo, string Descripcion);
public sealed record ResolverIncidenciaRequest(string Resolucion);
public sealed record SuscribirRutaRequest(int MinutosAntes);
public sealed record CrearBeneficioRequest(string Nombre, string Descripcion, int CostoPuntos, int Stock);
public sealed record CanjearBeneficioRequest(Guid BeneficioId, Guid Clave);
public sealed record CancelarSolicitudRequest(string Motivo);
public sealed record ReprogramarSolicitudRequest(DateTimeOffset ProgramadaPara);
public sealed record ActualizarPerfilRequest(string Nombre, string? Telefono, string? Direccion,
    double? Latitud, double? Longitud, bool RecibirAvisos, PreferenciasAvisosDto? PreferenciasAvisos);
