using MediatR;
using TrackTrash.Application.DTOs.Asociaciones;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Asociaciones.Commands.CrearAsociacion;

public sealed record CrearAsociacionCommand : IRequest<AsociacionDto>
{
    public Guid DistritoId { get; init; }

    public string Nombre { get; init; } = null!;

    public string Direccion { get; init; } = null!;

    public double Latitud { get; init; }

    public double Longitud { get; init; }

    public decimal CapacidadKg { get; init; }

    public MaterialReciclable[]? Materiales { get; init; }
}
