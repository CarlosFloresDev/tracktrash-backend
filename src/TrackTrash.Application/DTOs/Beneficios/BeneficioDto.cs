namespace TrackTrash.Application.DTOs.Beneficios;

public sealed record BeneficioDto(Guid Id, string Nombre, string Descripcion, int CostoPuntos, int Stock, bool Demostracion);
