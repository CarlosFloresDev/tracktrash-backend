using TrackTrash.Application.DTOs.DispositivosIoT;
using FluentValidation;

namespace TrackTrash.Application.UseCases.DispositivosIoT.Commands.RegistrarDispositivo;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class RegistrarDispositivoCommandValidator : AbstractValidator<RegistrarDispositivoCommand>
{
    public RegistrarDispositivoCommandValidator()
    {
        RuleFor(x => x.ContenedorId).NotEmpty();
        RuleFor(x => x.AlturaCm).InclusiveBetween(20, 500).WithMessage("Altura permitida: 20 a 500 cm.");
    }
}
