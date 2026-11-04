using System.ComponentModel.DataAnnotations;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Application.DTOs.SolicitudesRecojo;

public sealed record CrearSolicitudRequest(
    [Required] Guid ClaveSolicitud,
    [EnumDataType(typeof(MaterialReciclable))] MaterialReciclable Material,
    [Range(typeof(decimal), "0.01", "500")] decimal CantidadEstimadaKg,
    [Required, StringLength(250, MinimumLength = 8)] string Direccion,
    DateTimeOffset ProgramadaPara,
    double? Latitud = null,
    double? Longitud = null,
    bool AutoAsignar = false);

public sealed record ConfirmarEntregaRequest([Required, RegularExpression("^[0-9]{6}$")] string Codigo);
