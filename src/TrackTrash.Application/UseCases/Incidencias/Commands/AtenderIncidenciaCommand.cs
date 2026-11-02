using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Incidencias.Commands;

public sealed record AtenderIncidenciaCommand : IRequest
{
    public Guid IncidenciaId { get; set; }
}

internal sealed class AtenderIncidenciaCommandHandler(
    IIncidenciaRepository repo,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork) : IRequestHandler<AtenderIncidenciaCommand>
{
    public async Task Handle(AtenderIncidenciaCommand request, CancellationToken cancellationToken)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException();
        var i = await repo.ObtenerAsync(request.IncidenciaId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        i.Atender(usuario.Id);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);
    }
}
