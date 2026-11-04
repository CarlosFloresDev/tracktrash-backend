using FluentValidation;

namespace TrackTrash.Application.UseCases.Incidencias.Commands.AtenderIncidencia;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class AtenderIncidenciaCommandValidator : AbstractValidator<AtenderIncidenciaCommand>
{
    public AtenderIncidenciaCommandValidator()
    {
        RuleFor(x => x.IncidenciaId).NotEmpty();
    }
}
