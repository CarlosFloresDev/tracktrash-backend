using MediatR;

namespace TrackTrash.Application.UseCases.Incidencias.Commands.AtenderIncidencia;

public sealed record AtenderIncidenciaCommand : IRequest
{
    public Guid IncidenciaId { get; init; }
}
