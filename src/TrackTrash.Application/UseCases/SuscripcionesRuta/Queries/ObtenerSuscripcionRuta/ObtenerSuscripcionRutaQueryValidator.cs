using FluentValidation;

namespace TrackTrash.Application.UseCases.SuscripcionesRuta.Queries.ObtenerSuscripcionRuta;

internal sealed class ObtenerSuscripcionRutaQueryValidator : AbstractValidator<ObtenerSuscripcionRutaQuery>
{
    public ObtenerSuscripcionRutaQueryValidator()
    {
        RuleFor(x => x.RutaId).NotEmpty();
    }
}
