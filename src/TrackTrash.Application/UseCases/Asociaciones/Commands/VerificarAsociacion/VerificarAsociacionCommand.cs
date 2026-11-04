using MediatR;

namespace TrackTrash.Application.UseCases.Asociaciones.Commands.VerificarAsociacion;

public sealed record VerificarAsociacionCommand : IRequest
{
    public Guid AsociacionId { get; init; }
}
