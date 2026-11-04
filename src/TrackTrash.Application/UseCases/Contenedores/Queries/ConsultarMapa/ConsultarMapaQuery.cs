using TrackTrash.Application.DTOs.Contenedores;
using MediatR;

namespace TrackTrash.Application.UseCases.Contenedores.Queries.ConsultarMapa;

public sealed record ConsultarMapaQuery : IRequest<MapaDto>
{
    public double Latitud { get; init; }

    public double Longitud { get; init; }

    public double RadioKm { get; init; }
}
