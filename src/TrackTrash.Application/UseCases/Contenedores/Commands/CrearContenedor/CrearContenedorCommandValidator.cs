using TrackTrash.Application.DTOs.Contenedores;
using FluentValidation;

namespace TrackTrash.Application.UseCases.Contenedores.Commands.CrearContenedor;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class CrearContenedorCommandValidator : AbstractValidator<CrearContenedorCommand>
{
    public CrearContenedorCommandValidator()
    {
        RuleFor(x => x.DistritoId).NotEmpty();
        RuleFor(x => x.Nombre).NotEmpty().Length(3, 120);
        RuleFor(x => x.Latitud).InclusiveBetween(-90, 90).WithMessage("La latitud debe estar entre -90 y 90.");
        RuleFor(x => x.Longitud).InclusiveBetween(-180, 180).WithMessage("La longitud debe estar entre -180 y 180.");
    }
}
