using TrackTrash.Application.DTOs.DispositivosIoT;
using MediatR;

namespace TrackTrash.Application.UseCases.DispositivosIoT.Commands.RegistrarDispositivo;

public sealed record RegistrarDispositivoCommand : IRequest<DispositivoRegistradoDto>
{
    public Guid ContenedorId { get; init; }

    public double AlturaCm { get; init; }
}
