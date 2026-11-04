using TrackTrash.Application.DTOs.Contenedores;
using FluentValidation;

namespace TrackTrash.Application.UseCases.Contenedores.Queries.ConsultarMapa;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class ConsultarMapaQueryValidator : AbstractValidator<ConsultarMapaQuery>
{
    public ConsultarMapaQueryValidator()
    {
        RuleFor(x => x.Latitud).InclusiveBetween(-90, 90).WithMessage("La latitud debe estar entre -90 y 90.");
        RuleFor(x => x.Longitud).InclusiveBetween(-180, 180).WithMessage("La longitud debe estar entre -180 y 180.");
        RuleFor(x => x.RadioKm).InclusiveBetween(0.1, 50).WithMessage("Radio permitido: 0.1 a 50 km.");
    }
}
