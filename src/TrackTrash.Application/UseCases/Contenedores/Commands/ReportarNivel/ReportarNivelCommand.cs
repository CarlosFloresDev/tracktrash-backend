using MediatR;
using TrackTrash.Application.DTOs.Contenedores;

namespace TrackTrash.Application.UseCases.Contenedores.Commands.ReportarNivel;

public sealed record ReportarNivelCommand : IRequest<ContenedorDto>
{
    public Guid ContenedorId { get; init; }

    public int Porcentaje { get; init; }
}
