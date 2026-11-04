using MediatR;
using TrackTrash.Application.DTOs.Canjes;

namespace TrackTrash.Application.UseCases.Canjes.Commands.CanjearBeneficio;

public sealed record CanjearBeneficioCommand : IRequest<CanjeDto>
{
    public Guid BeneficioId { get; init; }

    public Guid Clave { get; init; }
}
