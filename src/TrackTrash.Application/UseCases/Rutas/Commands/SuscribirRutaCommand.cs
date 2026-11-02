using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Rutas.Commands;

public sealed record SuscribirRutaCommand : IRequest
{
    public Guid RutaId { get; set; }

    public int MinutosAntes { get; set; }
}

internal sealed class SuscribirRutaCommandHandler(
    IRutaRepository repo,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork,
    TimeProvider reloj) : IRequestHandler<SuscribirRutaCommand>
{
    public async Task Handle(SuscribirRutaCommand request, CancellationToken cancellationToken)
    {
        var ruta = await repo.ObtenerAsync(request.RutaId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        var suscripcion = await repo.SuscripcionAsync(request.RutaId, usuario.Id, cancellationToken);
        if (suscripcion is null)
            repo.Agregar(SuscripcionRuta.Crear(usuario.Id, ruta, request.MinutosAntes, reloj.GetUtcNow()));
        else
            suscripcion.CambiarAntelacion(ruta, request.MinutosAntes, reloj.GetUtcNow());
        await unitOfWork.GuardarCambiosAsync(cancellationToken);
    }
}
