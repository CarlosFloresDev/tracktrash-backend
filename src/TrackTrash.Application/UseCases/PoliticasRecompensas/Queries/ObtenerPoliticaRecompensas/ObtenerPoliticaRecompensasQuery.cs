using MediatR;
using TrackTrash.Application.DTOs.PoliticasRecompensas;

namespace TrackTrash.Application.UseCases.PoliticasRecompensas.Queries.ObtenerPoliticaRecompensas;

public sealed record ObtenerPoliticaRecompensasQuery : IRequest<PoliticaRecompensasDto>;
