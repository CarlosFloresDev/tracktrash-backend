using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Recompensas.Commands;

public sealed record CrearBeneficioCommand : IRequest<Guid>
{
    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int CostoPuntos { get; set; }

    public int Stock { get; set; }
}

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
