using TrackTrash.Application.DTOs.SolicitudesRecojo;
using FluentValidation;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.ConfirmarEntrega;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class ConfirmarEntregaCommandValidator : AbstractValidator<ConfirmarEntregaCommand>
{
    public ConfirmarEntregaCommandValidator()
    {
        RuleFor(x => x.SolicitudId).NotEmpty();
        RuleFor(x => x.Codigo).NotEmpty().Matches("^[0-9]{6}$").WithMessage("El código debe tener seis dígitos.");
    }
}
