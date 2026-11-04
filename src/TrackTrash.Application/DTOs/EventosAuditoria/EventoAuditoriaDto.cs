namespace TrackTrash.Application.DTOs.EventosAuditoria;

public sealed record EventoAuditoriaDto(Guid Id, string Actor, string Entidad, string Referencia, string Accion, string Campos, DateTimeOffset CreadoEn);
