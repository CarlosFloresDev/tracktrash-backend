using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Operacion.Queries;

public sealed record ObtenerIndicadoresQuery : IRequest<IndicadoresOperacion>;

internal sealed class ObtenerIndicadoresQueryHandler(
    IIndicadoresRepository repo,
    IUsuarioActualService usuario,
    TimeProvider reloj) : IRequestHandler<ObtenerIndicadoresQuery, IndicadoresOperacion>
{
    public Task<IndicadoresOperacion> Handle(ObtenerIndicadoresQuery request, CancellationToken cancellationToken)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException();
        return repo.ObtenerAsync(reloj.GetUtcNow(), cancellationToken);
    }
}
