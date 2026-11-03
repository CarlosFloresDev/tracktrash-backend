using TrackTrash.Domain.Entities;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Common;

public sealed record SolicitudDto(Guid Id, string Material, decimal CantidadEstimadaKg,
    string? Direccion, DateTimeOffset ProgramadaPara, string Estado,
    string? CodigoConfirmacion, DateTimeOffset CreadaEn)
{
    public static SolicitudDto Desde(SolicitudRecojo s, Guid actorId) => new(
        s.Id, s.Material.ToString(), s.CantidadEstimadaKg,
        s.VecinoId == actorId || s.RecicladorId == actorId ? s.Direccion : null,
        s.ProgramadaPara, s.Estado.ToString(), s.VecinoId == actorId ? s.CodigoConfirmacion : null, s.CreadaEn);
}
