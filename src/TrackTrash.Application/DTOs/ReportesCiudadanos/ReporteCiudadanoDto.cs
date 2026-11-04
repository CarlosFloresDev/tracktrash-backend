namespace TrackTrash.Application.DTOs.ReportesCiudadanos;

public sealed record ReporteCiudadanoDto(Guid Id, Guid ContenedorId, Guid UsuarioId, int NivelEstimado, DateTimeOffset CreadoEn);
