using FluentValidation;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Queries.ObtenerSolicitudRecojo;

internal sealed class ObtenerSolicitudRecojoQueryValidator : AbstractValidator<ObtenerSolicitudRecojoQuery>
{
    public ObtenerSolicitudRecojoQueryValidator() => RuleFor(x => x.Id).NotEmpty();
}
