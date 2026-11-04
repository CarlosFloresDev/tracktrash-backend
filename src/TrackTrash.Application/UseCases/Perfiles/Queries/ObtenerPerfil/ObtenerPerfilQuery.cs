using TrackTrash.Application.DTOs.Perfiles;
using MediatR;

namespace TrackTrash.Application.UseCases.Perfiles.Queries.ObtenerPerfil;

public sealed record ObtenerPerfilQuery : IRequest<PerfilDto>;
