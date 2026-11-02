using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Recompensas.Queries;

public sealed record ListarBeneficiosQuery : IRequest<IReadOnlyList<BeneficioDto>>;

internal sealed class ListarBeneficiosQueryHandler(
    IBeneficioRepository repo) : IRequestHandler<ListarBeneficiosQuery, IReadOnlyList<BeneficioDto>>
{
    public async Task<IReadOnlyList<BeneficioDto>> Handle(ListarBeneficiosQuery request, CancellationToken cancellationToken) =>
        (await repo.ListarAsync(cancellationToken)).Select(b => new BeneficioDto(b.Id, b.Nombre, b.Descripcion, b.CostoPuntos, b.Stock, b.Demostracion)).ToList();
}
