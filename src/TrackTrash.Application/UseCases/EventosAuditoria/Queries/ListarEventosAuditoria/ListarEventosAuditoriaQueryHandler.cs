using MediatR;
using TrackTrash.Application.DTOs.EventosAuditoria;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Queries;
using TrackTrash.Application.Ports.Services;

namespace TrackTrash.Application.UseCases.EventosAuditoria.Queries.ListarEventosAuditoria;

internal sealed class ListarEventosAuditoriaQueryHandler(
    IAuditoriaConsulta repo,
    IUsuarioActualService usuario) : IRequestHandler<ListarEventosAuditoriaQuery, IReadOnlyList<EventoAuditoriaDto>>
{
    public Task<IReadOnlyList<EventoAuditoriaDto>> Handle(ListarEventosAuditoriaQuery request, CancellationToken cancellationToken)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException();
        return repo.ListarAsync(request.Entidad, request.Maximo, cancellationToken);
    }
}
