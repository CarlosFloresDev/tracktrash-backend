using TrackTrash.Application.DTOs.Contenedores;
using FluentValidation;

namespace TrackTrash.Application.UseCases.Contenedores.Commands.ReportarNivel;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class ReportarNivelCommandValidator : AbstractValidator<ReportarNivelCommand>
{
    public ReportarNivelCommandValidator()
    {
        RuleFor(x => x.ContenedorId).NotEmpty();
        RuleFor(x => x.Porcentaje).InclusiveBetween(0, 100);
    }
}
