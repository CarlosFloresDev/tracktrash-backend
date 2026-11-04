using MediatR;
using TrackTrash.Application.DTOs.LecturasNivel;

namespace TrackTrash.Application.UseCases.LecturasNivel.Queries.ObtenerUltimaLectura;

public sealed record ObtenerUltimaLecturaQuery : IRequest<LecturaNivelDto>
{
    public Guid DispositivoId { get; init; }
}
