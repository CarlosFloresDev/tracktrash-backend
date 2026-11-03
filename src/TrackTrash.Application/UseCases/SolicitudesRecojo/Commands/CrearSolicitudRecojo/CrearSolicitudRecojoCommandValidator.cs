using FluentValidation;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.CrearSolicitudRecojo;

internal sealed class CrearSolicitudRecojoCommandValidator : AbstractValidator<CrearSolicitudRecojoCommand>
{
    public CrearSolicitudRecojoCommandValidator()
    {
        RuleFor(x => x.ClaveSolicitud).NotEmpty();
        RuleFor(x => x.Material).IsInEnum();
        RuleFor(x => x.CantidadEstimadaKg).InclusiveBetween(0.01m, 500m);
        RuleFor(x => x.Direccion).NotEmpty().Length(8, 250);
    }
}
