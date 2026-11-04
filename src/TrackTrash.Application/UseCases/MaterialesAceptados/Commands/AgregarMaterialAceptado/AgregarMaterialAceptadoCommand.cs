using MediatR;
using TrackTrash.Application.DTOs.MaterialesAceptados;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Application.UseCases.MaterialesAceptados.Commands.AgregarMaterialAceptado;

public sealed record AgregarMaterialAceptadoCommand : IRequest<MaterialAceptadoDto>
{
    public Guid AsociacionId { get; init; }
    public MaterialReciclable Material { get; init; }
}
