using TrackTrash.Application.DTOs.SolicitudesRecojo;
using FluentValidation;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.CrearSolicitudRecojo;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class CrearSolicitudRecojoCommandValidator : AbstractValidator<CrearSolicitudRecojoCommand>
{
    public CrearSolicitudRecojoCommandValidator()
    {
        RuleFor(x => x.ClaveSolicitud).NotEmpty().WithMessage("Envía una clave de idempotencia.");
        RuleFor(x => x.Material).IsInEnum();
        RuleFor(x => x.CantidadEstimadaKg).GreaterThan(0).LessThanOrEqualTo(500)
            .Must(k => decimal.Round(k, 2) == k).WithMessage("La cantidad estimada admite hasta dos decimales.");
        RuleFor(x => x.Direccion).NotEmpty().Must(d => d is not null && d.Trim().Length is >= 8 and <= 250)
            .WithMessage("Escribe una dirección de entre 8 y 250 caracteres.");
        RuleFor(x => x.ProgramadaPara).NotEmpty();
        RuleFor(x => x).Must(x => x.Latitud.HasValue == x.Longitud.HasValue).WithName("Ubicacion").WithMessage("Indica ambas coordenadas o ninguna.");
        RuleFor(x => x.Latitud!.Value).InclusiveBetween(-90, 90).When(x => x.Latitud.HasValue).WithName("Latitud");
        RuleFor(x => x.Longitud!.Value).InclusiveBetween(-180, 180).When(x => x.Longitud.HasValue).WithName("Longitud");
        RuleFor(x => x.Latitud).NotNull().When(x => x.AutoAsignar).WithMessage("La asignación automática requiere la ubicación del recojo.");
    }
}
