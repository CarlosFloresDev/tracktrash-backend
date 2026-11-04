using FluentValidation;

namespace TrackTrash.Application.UseCases.MaterialesAceptados.Commands.QuitarMaterialAceptado;

internal sealed class QuitarMaterialAceptadoCommandValidator : AbstractValidator<QuitarMaterialAceptadoCommand>
{
    public QuitarMaterialAceptadoCommandValidator()
    {
        RuleFor(x => x.AsociacionId).NotEmpty();
        RuleFor(x => x.Material).IsInEnum();
    }
}
