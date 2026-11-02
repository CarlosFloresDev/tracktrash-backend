using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Catalogo.Commands;

public sealed record VerificarAsociacionCommand : IRequest
{
    public Guid AsociacionId { get; set; }
}

internal sealed class VerificarAsociacionCommandHandler(
    ICatalogoRepository catalogo,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork) : IRequestHandler<VerificarAsociacionCommand>
{
    public async Task Handle(VerificarAsociacionCommand request, CancellationToken cancellationToken)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException();
        var a = await catalogo.AsociacionAsync(request.AsociacionId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        a.Verificar();
        await unitOfWork.GuardarCambiosAsync(cancellationToken);
    }
}
