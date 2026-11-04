using MediatR;
using TrackTrash.Application.DTOs.MaterialesAceptados;
using TrackTrash.Application.Exceptions;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.MaterialesAceptados.Queries.ListarMaterialesAceptados;

internal sealed class ListarMaterialesAceptadosQueryHandler(
    ICatalogoRepository repo) : IRequestHandler<ListarMaterialesAceptadosQuery, IReadOnlyList<MaterialAceptadoDto>>
{
    public async Task<IReadOnlyList<MaterialAceptadoDto>> Handle(ListarMaterialesAceptadosQuery request, CancellationToken cancellationToken)
    {
        var asociacion = await repo.AsociacionAsync(request.AsociacionId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        return asociacion.Materiales.Select(m => new MaterialAceptadoDto(m.AsociacionId, m.Material.ToString())).ToList();
    }
}
