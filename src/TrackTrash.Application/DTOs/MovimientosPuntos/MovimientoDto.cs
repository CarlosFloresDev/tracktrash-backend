namespace TrackTrash.Application.DTOs.MovimientosPuntos;

public sealed record MovimientoDto(Guid Id, int Puntos, string Motivo, DateTimeOffset CreadoEn);
