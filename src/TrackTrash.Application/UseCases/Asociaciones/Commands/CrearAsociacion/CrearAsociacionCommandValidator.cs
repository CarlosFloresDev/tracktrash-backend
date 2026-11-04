using FluentValidation;

namespace TrackTrash.Application.UseCases.Asociaciones.Commands.CrearAsociacion;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class CrearAsociacionCommandValidator : AbstractValidator<CrearAsociacionCommand>
{
    public CrearAsociacionCommandValidator()
    {
        RuleFor(x => x.DistritoId).NotEmpty();
        RuleFor(x => x.Nombre).NotEmpty().Length(2, 120);
        RuleFor(x => x.Direccion).NotEmpty().Length(5, 250);
        RuleFor(x => x.Latitud).InclusiveBetween(-90, 90).WithMessage("La latitud debe estar entre -90 y 90.");
        RuleFor(x => x.Longitud).InclusiveBetween(-180, 180).WithMessage("La longitud debe estar entre -180 y 180.");
        RuleFor(x => x.CapacidadKg).GreaterThan(0).LessThanOrEqualTo(100000);
        RuleFor(x => x.Materiales).NotEmpty().WithMessage("Selecciona al menos un material.");
        RuleForEach(x => x.Materiales).IsInEnum();
    }
}
