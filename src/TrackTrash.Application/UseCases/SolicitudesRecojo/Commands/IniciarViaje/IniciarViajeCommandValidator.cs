using TrackTrash.Application.DTOs.SolicitudesRecojo;
using FluentValidation;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.IniciarViaje;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class IniciarViajeCommandValidator : AbstractValidator<IniciarViajeCommand>
{
    public IniciarViajeCommandValidator()
    {
        RuleFor(x => x.SolicitudId).NotEmpty();
    }
}
