using MediatR;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.CrearSolicitudRecojo;

public sealed record CrearSolicitudRecojoCommand : IRequest<SolicitudDto>
{
    public Guid ClaveSolicitud { get; init; }
    public MaterialReciclable Material { get; init; }
    public decimal CantidadEstimadaKg { get; init; }
    public string Direccion { get; init; } = null!;

    public DateTimeOffset ProgramadaPara { get; init; }

    public double? Latitud { get; init; }

    public double? Longitud { get; init; }

    public bool AutoAsignar { get; init; }
}
