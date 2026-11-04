using TrackTrash.Application.DTOs.Perfiles;
using FluentValidation;

namespace TrackTrash.Application.UseCases.Perfiles.Commands.ActualizarPerfil;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class ActualizarPerfilCommandValidator : AbstractValidator<ActualizarPerfilCommand>
{
    public ActualizarPerfilCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().Length(2, 100);
        RuleFor(x => x.Telefono).MaximumLength(20).Matches("^[0-9+ ]*$").WithMessage("El teléfono no es válido.");
        RuleFor(x => x.Direccion).MaximumLength(250);
        RuleFor(x => x).Must(x => x.Latitud.HasValue == x.Longitud.HasValue).WithName("Ubicacion").WithMessage("Indica ambas coordenadas o ninguna.");
        RuleFor(x => x.Latitud!.Value).InclusiveBetween(-90, 90).When(x => x.Latitud.HasValue).WithName("Latitud");
        RuleFor(x => x.Longitud!.Value).InclusiveBetween(-180, 180).When(x => x.Longitud.HasValue).WithName("Longitud");
    }
}
