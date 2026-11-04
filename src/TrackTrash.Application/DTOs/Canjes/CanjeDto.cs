using TrackTrash.Domain.Entities;

namespace TrackTrash.Application.DTOs.Canjes;

public sealed record CanjeDto(Guid Id, Guid BeneficioId, int PuntosUsados, string Codigo, DateTimeOffset CreadoEn);

public static class CanjeMapping
{
    public static CanjeDto ToDto(this Canje c) => new(c.Id, c.BeneficioId, c.PuntosUsados, c.Codigo, c.CreadoEn);
}
