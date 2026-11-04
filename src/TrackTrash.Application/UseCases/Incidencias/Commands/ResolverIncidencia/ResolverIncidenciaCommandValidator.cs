using FluentValidation;

namespace TrackTrash.Application.UseCases.Incidencias.Commands.ResolverIncidencia;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class ResolverIncidenciaCommandValidator : AbstractValidator<ResolverIncidenciaCommand>
{
    public ResolverIncidenciaCommandValidator()
    {
        RuleFor(x => x.IncidenciaId).NotEmpty();
        RuleFor(x => x.Resolucion).NotEmpty().Length(10, 1000);
    }
}
