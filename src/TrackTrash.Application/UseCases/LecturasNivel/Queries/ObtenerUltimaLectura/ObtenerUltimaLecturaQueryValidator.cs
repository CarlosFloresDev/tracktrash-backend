using FluentValidation;

namespace TrackTrash.Application.UseCases.LecturasNivel.Queries.ObtenerUltimaLectura;

internal sealed class ObtenerUltimaLecturaQueryValidator : AbstractValidator<ObtenerUltimaLecturaQuery>
{
    public ObtenerUltimaLecturaQueryValidator()
    {
        RuleFor(x => x.DispositivoId).NotEmpty();
    }
}
