using MediatR;
using TrackTrash.Application.DTOs.PoliticasRecompensas;
using Politica = TrackTrash.Domain.Entities.PoliticaRecompensas;

namespace TrackTrash.Application.UseCases.PoliticasRecompensas.Queries.ObtenerPoliticaRecompensas;

internal sealed class ObtenerPoliticaRecompensasQueryHandler
    : IRequestHandler<ObtenerPoliticaRecompensasQuery, PoliticaRecompensasDto>
{
    public Task<PoliticaRecompensasDto> Handle(ObtenerPoliticaRecompensasQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(new PoliticaRecompensasDto(Politica.PuntosPorEntrega, Politica.MaximoDiario));
}
