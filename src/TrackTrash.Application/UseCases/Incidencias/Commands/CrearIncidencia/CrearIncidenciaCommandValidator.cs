using FluentValidation;

namespace TrackTrash.Application.UseCases.Incidencias.Commands.CrearIncidencia;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class CrearIncidenciaCommandValidator : AbstractValidator<CrearIncidenciaCommand>
{
    public CrearIncidenciaCommandValidator()
    {
        RuleFor(x => x.ContenedorId).NotEmpty();
        RuleFor(x => x.Tipo).NotEmpty().Length(3, 60);
        RuleFor(x => x.Descripcion).NotEmpty().Length(10, 1000);
    }
}
