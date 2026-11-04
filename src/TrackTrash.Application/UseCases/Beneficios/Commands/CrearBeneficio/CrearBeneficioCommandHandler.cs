using TrackTrash.Application.DTOs.Beneficios;
using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Beneficios.Commands.CrearBeneficio;

internal sealed class CrearBeneficioCommandHandler(
    IBeneficioRepository repo,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork) : IRequestHandler<CrearBeneficioCommand, Guid>
{
    public async Task<Guid> Handle(CrearBeneficioCommand request, CancellationToken cancellationToken)
    {
        if (!usuario.EsAdministrador) throw new AccesoDenegadoException();
        var b = Beneficio.Crear(request.Nombre, request.Descripcion, request.CostoPuntos, request.Stock);
        repo.Agregar(b);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);
        return b.Id;
    }
}
