using FluentValidation;

namespace TrackTrash.Application.UseCases.Asociaciones.Commands.VerificarAsociacion;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class VerificarAsociacionCommandValidator : AbstractValidator<VerificarAsociacionCommand>
{
    public VerificarAsociacionCommandValidator()
    {
        RuleFor(x => x.AsociacionId).NotEmpty();
    }
}
