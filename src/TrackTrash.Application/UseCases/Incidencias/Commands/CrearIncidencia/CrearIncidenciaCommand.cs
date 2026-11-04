using MediatR;
using TrackTrash.Application.DTOs.Incidencias;

namespace TrackTrash.Application.UseCases.Incidencias.Commands.CrearIncidencia;

public sealed record CrearIncidenciaCommand : IRequest<IncidenciaDto>
{
    public Guid ContenedorId { get; init; }

    public string Tipo { get; init; } = null!;

    public string Descripcion { get; init; } = null!;
}
