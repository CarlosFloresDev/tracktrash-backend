using FluentValidation;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.ConfirmarEntrega;

internal sealed class ConfirmarEntregaCommandValidator : AbstractValidator<ConfirmarEntregaCommand>
{
    public ConfirmarEntregaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Codigo).NotEmpty().Matches("^[0-9]{6}$").WithMessage("El código debe tener 6 dígitos.");
    }
}
