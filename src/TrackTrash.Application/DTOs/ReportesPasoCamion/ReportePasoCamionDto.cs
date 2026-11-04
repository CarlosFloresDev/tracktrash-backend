namespace TrackTrash.Application.DTOs.ReportesPasoCamion;

public sealed record ReportePasoCamionDto(Guid Id, Guid RutaId, Guid UsuarioId, DateTimeOffset ReportadoEn);
