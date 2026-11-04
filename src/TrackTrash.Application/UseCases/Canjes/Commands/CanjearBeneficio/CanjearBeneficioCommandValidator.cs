using FluentValidation;

namespace TrackTrash.Application.UseCases.Canjes.Commands.CanjearBeneficio;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class CanjearBeneficioCommandValidator : AbstractValidator<CanjearBeneficioCommand>
{
    public CanjearBeneficioCommandValidator()
    {
        RuleFor(x => x.BeneficioId).NotEmpty();
        RuleFor(x => x.Clave).NotEmpty().WithMessage("Envía una clave de idempotencia.");
    }
}
