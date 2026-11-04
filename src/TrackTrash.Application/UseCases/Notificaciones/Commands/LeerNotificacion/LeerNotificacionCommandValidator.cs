using TrackTrash.Application.DTOs.Notificaciones;
using FluentValidation;

namespace TrackTrash.Application.UseCases.Notificaciones.Commands.LeerNotificacion;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class LeerNotificacionCommandValidator : AbstractValidator<LeerNotificacionCommand>
{
    public LeerNotificacionCommandValidator()
    {
        RuleFor(x => x.NotificacionId).NotEmpty();
    }
}
