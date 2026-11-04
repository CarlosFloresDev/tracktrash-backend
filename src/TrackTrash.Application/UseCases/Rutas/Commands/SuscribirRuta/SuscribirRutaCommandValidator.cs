using TrackTrash.Application.DTOs.Rutas;
using FluentValidation;

namespace TrackTrash.Application.UseCases.Rutas.Commands.SuscribirRuta;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class SuscribirRutaCommandValidator : AbstractValidator<SuscribirRutaCommand>
{
    public SuscribirRutaCommandValidator()
    {
        RuleFor(x => x.RutaId).NotEmpty();
        RuleFor(x => x.MinutosAntes).InclusiveBetween(5, 60).WithMessage("El aviso debe anticiparse entre 5 y 60 minutos.");
    }
}
