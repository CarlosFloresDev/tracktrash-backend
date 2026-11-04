using MediatR;
using TrackTrash.Application.DTOs.LecturasNivel;

namespace TrackTrash.Application.UseCases.LecturasNivel.Queries.ListarLecturasNivel;

public sealed record ListarLecturasNivelQuery : IRequest<IReadOnlyList<LecturaNivelDto>>
{
    public Guid DispositivoId { get; init; }
    public int Maximo { get; init; } = 50;
}
