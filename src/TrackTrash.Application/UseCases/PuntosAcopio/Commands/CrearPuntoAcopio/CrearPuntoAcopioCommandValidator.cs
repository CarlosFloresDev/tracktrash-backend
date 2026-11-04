using FluentValidation;

namespace TrackTrash.Application.UseCases.PuntosAcopio.Commands.CrearPuntoAcopio;

internal sealed class CrearPuntoAcopioCommandValidator : AbstractValidator<CrearPuntoAcopioCommand>
{
    public CrearPuntoAcopioCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().Length(3, 120);
        RuleFor(x => x.Direccion).NotEmpty().Length(5, 250);
        RuleFor(x => x.Latitud).InclusiveBetween(-90, 90);
        RuleFor(x => x.Longitud).InclusiveBetween(-180, 180);
    }
}
