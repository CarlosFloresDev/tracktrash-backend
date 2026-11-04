using TrackTrash.Application.DTOs.DispositivosIoT;
using FluentValidation;

namespace TrackTrash.Application.UseCases.DispositivosIoT.Commands.RegistrarLectura;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class RegistrarLecturaCommandValidator : AbstractValidator<RegistrarLecturaCommand>
{
    public RegistrarLecturaCommandValidator()
    {
        RuleFor(x => x.DispositivoId).NotEmpty();
        RuleFor(x => x.ClaveDispositivo).NotEmpty().WithMessage("Falta la cabecera X-Device-Key.");
        RuleFor(x => x.Secuencia).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DistanciasCm).NotNull().Must(d => d is not null && d.Length is >= 3 and <= 11).WithMessage("Envía entre 3 y 11 distancias.");
        RuleFor(x => x.Bateria).InclusiveBetween(0, 100);
    }
}
