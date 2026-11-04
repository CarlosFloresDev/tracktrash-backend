using TrackTrash.Application.DTOs.SolicitudesRecojo;
using FluentValidation;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.ReprogramarSolicitud;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class ReprogramarSolicitudCommandValidator : AbstractValidator<ReprogramarSolicitudCommand>
{
    public ReprogramarSolicitudCommandValidator()
    {
        RuleFor(x => x.SolicitudId).NotEmpty();
        RuleFor(x => x.ProgramadaPara).NotEmpty();
    }
}
