using MediatR;
using TrackTrash.Application.DTOs.MaterialesAceptados;

namespace TrackTrash.Application.UseCases.MaterialesAceptados.Queries.ListarMaterialesAceptados;

public sealed record ListarMaterialesAceptadosQuery : IRequest<IReadOnlyList<MaterialAceptadoDto>>
{
    public Guid AsociacionId { get; init; }
}
