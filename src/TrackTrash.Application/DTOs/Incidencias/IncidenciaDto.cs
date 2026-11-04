using TrackTrash.Domain.Entities;

namespace TrackTrash.Application.DTOs.Incidencias;

public sealed record IncidenciaDto(Guid Id, Guid ContenedorId, string Tipo, string Descripcion, string Estado, string? Resolucion,
    DateTimeOffset CreadaEn, DateTimeOffset? ResueltaEn);

public static class IncidenciaMapping
{
    public static IncidenciaDto ToDto(this Incidencia i) => new(i.Id, i.ContenedorId, i.Tipo, i.Descripcion, i.Estado.ToString(), i.Resolucion, i.CreadaEn, i.ResueltaEn);
}
