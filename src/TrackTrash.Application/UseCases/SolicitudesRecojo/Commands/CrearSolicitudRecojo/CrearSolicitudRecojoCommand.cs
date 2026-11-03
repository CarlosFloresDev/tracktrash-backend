using MediatR;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Common;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.CrearSolicitudRecojo;

public sealed record CrearSolicitudRecojoCommand : IRequest<SolicitudDto>
{
    public required Guid ClaveSolicitud { get; init; }
    public required MaterialReciclable Material { get; init; }
    public required decimal CantidadEstimadaKg { get; init; }
    public required string Direccion { get; init; }
    public required DateTimeOffset ProgramadaPara { get; init; }
}
