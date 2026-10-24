using System.ComponentModel.DataAnnotations;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Application.DTOs.SolicitudesRecojo;

public sealed record CrearSolicitudRequest(
    [Required] Guid ClaveSolicitud,
    [EnumDataType(typeof(MaterialReciclable))] MaterialReciclable Material,
    [Range(typeof(decimal), "0.01", "500")] decimal CantidadEstimadaKg,
    [Required, StringLength(250, MinimumLength = 8)] string Direccion,
    DateTimeOffset ProgramadaPara);

public sealed record ConfirmarEntregaRequest([Required, RegularExpression("^[0-9]{6}$")] string Codigo);

public sealed record SolicitudDto(Guid Id, string Material, decimal CantidadEstimadaKg,
    string? Direccion, DateTimeOffset ProgramadaPara, string Estado,
    string? CodigoConfirmacion, DateTimeOffset CreadaEn)
{
    public static SolicitudDto Desde(SolicitudRecojo s, Guid actorId) => new(
        s.Id, s.Material.ToString(), s.CantidadEstimadaKg,
        s.VecinoId == actorId || s.RecicladorId == actorId ? s.Direccion : null,
        s.ProgramadaPara, s.Estado.ToString(), s.VecinoId == actorId ? s.CodigoConfirmacion : null, s.CreadaEn);
}

public sealed record ImpactoDto(int Puntos);
