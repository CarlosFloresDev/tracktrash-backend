using TrackTrash.Application.DTOs.Rutas;
using FluentValidation;

namespace TrackTrash.Application.UseCases.Rutas.Commands.ReportarPaso;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class ReportarPasoCommandValidator : AbstractValidator<ReportarPasoCommand>
{
    public ReportarPasoCommandValidator()
    {
        RuleFor(x => x.RutaId).NotEmpty();
    }
}
