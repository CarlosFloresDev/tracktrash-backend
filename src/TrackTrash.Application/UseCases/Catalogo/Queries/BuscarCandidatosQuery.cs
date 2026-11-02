using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Catalogo.Queries;

public sealed record BuscarCandidatosQuery : IRequest<IReadOnlyList<AsociacionDto>>
{
    public MaterialReciclable Material { get; set; }

    public decimal Kg { get; set; }

    public double Latitud { get; set; }

    public double Longitud { get; set; }
}

internal sealed class BuscarCandidatosQueryHandler(
    ICatalogoRepository catalogo) : IRequestHandler<BuscarCandidatosQuery, IReadOnlyList<AsociacionDto>>
{
    public async Task<IReadOnlyList<AsociacionDto>> Handle(BuscarCandidatosQuery request, CancellationToken cancellationToken)
    {
        Ubicacion.Validar(request.Latitud, request.Longitud);
        if (!Enum.IsDefined(request.Material) || request.Kg <= 0 || request.Kg > 500) throw new Domain.Exceptions.ReglaNegocioException("Material o cantidad inválidos.");
        return (await catalogo.CandidatosAsync(request.Material, request.Kg, request.Latitud, request.Longitud, cancellationToken)).Select(a => a.Dto(Ubicacion.DistanciaKm(request.Latitud, request.Longitud, a.Latitud, a.Longitud))).ToList();
    }
}
