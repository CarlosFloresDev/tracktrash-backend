namespace TrackTrash.Application.DTOs.Notificaciones;

public sealed record NotificacionDto(Guid Id, string Titulo, string Mensaje, DateTimeOffset CreadaEn, bool Leida, string Tipo = "Sistema");
