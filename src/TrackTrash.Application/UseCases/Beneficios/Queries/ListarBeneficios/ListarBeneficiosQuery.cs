using TrackTrash.Application.DTOs.Beneficios;
using MediatR;

namespace TrackTrash.Application.UseCases.Beneficios.Queries.ListarBeneficios;

public sealed record ListarBeneficiosQuery : IRequest<IReadOnlyList<BeneficioDto>>;
