using TrackTrash.Application.DTOs.Beneficios;
using MediatR;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Beneficios.Queries.ListarBeneficios;

internal sealed class ListarBeneficiosQueryHandler(
    IBeneficioRepository repo) : IRequestHandler<ListarBeneficiosQuery, IReadOnlyList<BeneficioDto>>
{
    public async Task<IReadOnlyList<BeneficioDto>> Handle(ListarBeneficiosQuery request, CancellationToken cancellationToken) =>
        (await repo.ListarAsync(cancellationToken)).Select(b => new BeneficioDto(b.Id, b.Nombre, b.Descripcion, b.CostoPuntos, b.Stock, b.Demostracion)).ToList();
}
