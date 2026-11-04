using MediatR;
using TrackTrash.Application.DTOs.Asociaciones;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Application.UseCases.Asociaciones.Queries.BuscarCandidatos;

public sealed record BuscarCandidatosQuery : IRequest<IReadOnlyList<AsociacionDto>>
{
    public MaterialReciclable Material { get; init; }

    public decimal Kg { get; init; }

    public double Latitud { get; init; }

    public double Longitud { get; init; }
}
