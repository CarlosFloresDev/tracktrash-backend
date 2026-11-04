using TrackTrash.Application.DTOs.Rutas;
using MediatR;

namespace TrackTrash.Application.UseCases.Rutas.Commands.ReportarPaso;

public sealed record ReportarPasoCommand : IRequest
{
    public Guid RutaId { get; init; }
}
