using TrackTrash.Application.DTOs.Rutas;
using MediatR;

namespace TrackTrash.Application.UseCases.Rutas.Commands.CancelarSuscripcion;

public sealed record CancelarSuscripcionCommand : IRequest
{
    public Guid RutaId { get; init; }
}
