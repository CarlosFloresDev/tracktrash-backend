using MediatR;
using TrackTrash.Application.DTOs.MaterialesAceptados;
using TrackTrash.Domain.Entities;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.MaterialesAceptados.Commands.QuitarMaterialAceptado;

internal sealed class QuitarMaterialAceptadoCommandHandler(
    ICatalogoRepository repo,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork) : IRequestHandler<QuitarMaterialAceptadoCommand, MaterialAceptadoDto>
{
    public async Task<MaterialAceptadoDto> Handle(QuitarMaterialAceptadoCommand request, CancellationToken cancellationToken)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException();
        var asociacion = await repo.AsociacionAsync(request.AsociacionId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        asociacion.QuitarMaterial(request.Material);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);
        return new MaterialAceptadoDto(asociacion.Id, request.Material.ToString());
    }
}
