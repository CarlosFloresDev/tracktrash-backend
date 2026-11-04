using TrackTrash.Application.DTOs.Beneficios;
using MediatR;

namespace TrackTrash.Application.UseCases.Beneficios.Commands.CrearBeneficio;

public sealed record CrearBeneficioCommand : IRequest<Guid>
{
    public string Nombre { get; init; } = null!;

    public string Descripcion { get; init; } = null!;

    public int CostoPuntos { get; init; }

    public int Stock { get; init; }
}
