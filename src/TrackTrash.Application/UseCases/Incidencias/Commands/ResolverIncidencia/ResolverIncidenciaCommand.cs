using MediatR;

namespace TrackTrash.Application.UseCases.Incidencias.Commands.ResolverIncidencia;

public sealed record ResolverIncidenciaCommand : IRequest
{
    public Guid IncidenciaId { get; init; }

    public string Resolucion { get; init; } = null!;
}
