using FluentValidation;

namespace TrackTrash.Application.UseCases.MaterialesAceptados.Commands.AgregarMaterialAceptado;

internal sealed class AgregarMaterialAceptadoCommandValidator : AbstractValidator<AgregarMaterialAceptadoCommand>
{
    public AgregarMaterialAceptadoCommandValidator()
    {
        RuleFor(x => x.AsociacionId).NotEmpty();
        RuleFor(x => x.Material).IsInEnum();
    }
}
