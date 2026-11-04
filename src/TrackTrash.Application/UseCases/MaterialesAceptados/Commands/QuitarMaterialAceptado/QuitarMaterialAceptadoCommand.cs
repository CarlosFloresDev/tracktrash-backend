using MediatR;
using TrackTrash.Application.DTOs.MaterialesAceptados;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Application.UseCases.MaterialesAceptados.Commands.QuitarMaterialAceptado;

public sealed record QuitarMaterialAceptadoCommand : IRequest<MaterialAceptadoDto>
{
    public Guid AsociacionId { get; init; }
    public MaterialReciclable Material { get; init; }
}
