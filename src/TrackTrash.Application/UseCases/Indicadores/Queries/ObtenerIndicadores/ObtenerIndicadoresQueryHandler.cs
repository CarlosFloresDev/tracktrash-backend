using TrackTrash.Application.DTOs.Indicadores;
using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Queries;
using TrackTrash.Application.Ports.Services;

namespace TrackTrash.Application.UseCases.Indicadores.Queries.ObtenerIndicadores;

internal sealed class ObtenerIndicadoresQueryHandler(
    IIndicadoresConsulta repo,
    IUsuarioActualService usuario,
    TimeProvider reloj) : IRequestHandler<ObtenerIndicadoresQuery, IndicadoresDto>
{
    public Task<IndicadoresDto> Handle(ObtenerIndicadoresQuery request, CancellationToken cancellationToken)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException();
        return repo.ObtenerAsync(reloj.GetUtcNow(), cancellationToken);
    }
}
