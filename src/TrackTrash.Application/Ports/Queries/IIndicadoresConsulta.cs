using TrackTrash.Application.DTOs.Indicadores;

namespace TrackTrash.Application.Ports.Queries;

public interface IIndicadoresConsulta
{
    Task<IndicadoresDto> ObtenerAsync(DateTimeOffset ahora, CancellationToken ct);
}
