using TrackTrash.Application.DTOs.Rutas;
using FluentValidation;

namespace TrackTrash.Application.UseCases.Rutas.Commands.CancelarSuscripcion;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class CancelarSuscripcionCommandValidator : AbstractValidator<CancelarSuscripcionCommand>
{
    public CancelarSuscripcionCommandValidator()
    {
        RuleFor(x => x.RutaId).NotEmpty();
    }
}
