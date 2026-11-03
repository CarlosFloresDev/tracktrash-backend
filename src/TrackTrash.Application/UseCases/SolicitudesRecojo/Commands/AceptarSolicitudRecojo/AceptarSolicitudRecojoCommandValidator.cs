using FluentValidation;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.AceptarSolicitudRecojo;

internal sealed class AceptarSolicitudRecojoCommandValidator : AbstractValidator<AceptarSolicitudRecojoCommand>
{
    public AceptarSolicitudRecojoCommandValidator() => RuleFor(x => x.Id).NotEmpty();
}
