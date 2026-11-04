using TrackTrash.Application.DTOs.DispositivosIoT;
using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.DispositivosIoT.Commands.RegistrarDispositivo;

internal sealed class RegistrarDispositivoCommandHandler(
    IContenedorRepository repo,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork,
    ISeguridadDispositivoService seguridad) : IRequestHandler<RegistrarDispositivoCommand, DispositivoRegistradoDto>
{
    public async Task<DispositivoRegistradoDto> Handle(RegistrarDispositivoCommand request, CancellationToken cancellationToken)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException();
        _ = await repo.ObtenerAsync(request.ContenedorId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        var clave = seguridad.Generar(); var d = DispositivoIoT.Crear(request.ContenedorId, seguridad.Hash(clave), request.AlturaCm);
        repo.Agregar(d); await unitOfWork.GuardarCambiosAsync(cancellationToken); return new(d.Id, d.ContenedorId, clave);
    }
}
