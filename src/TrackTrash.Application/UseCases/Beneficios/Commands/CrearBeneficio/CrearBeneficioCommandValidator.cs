using TrackTrash.Application.DTOs.Beneficios;
using FluentValidation;

namespace TrackTrash.Application.UseCases.Beneficios.Commands.CrearBeneficio;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class CrearBeneficioCommandValidator : AbstractValidator<CrearBeneficioCommand>
{
    public CrearBeneficioCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().Length(3, 120);
        RuleFor(x => x.Descripcion).NotEmpty().Length(5, 500);
        RuleFor(x => x.CostoPuntos).GreaterThan(0);
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
    }
}
