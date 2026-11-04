using TrackTrash.Application.DTOs.DispositivosIoT;
using MediatR;

namespace TrackTrash.Application.UseCases.DispositivosIoT.Commands.RegistrarLectura;

public sealed record RegistrarLecturaCommand : IRequest<LecturaResultadoDto>
{
    public Guid DispositivoId { get; init; }

    public string ClaveDispositivo { get; init; } = null!;

    public long Secuencia { get; init; }

    public double[]? DistanciasCm { get; init; }

    public int Bateria { get; init; }

    public DateTimeOffset MedidaEn { get; init; }
}
