using FluentValidation;

namespace TrackTrash.Application.UseCases.PuntosAcopio.Queries.ObtenerPuntoAcopio;

internal sealed class ObtenerPuntoAcopioQueryValidator : AbstractValidator<ObtenerPuntoAcopioQuery>
{
    public ObtenerPuntoAcopioQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
