using FluentValidation;

namespace TrackTrash.Application.UseCases.ReportesPasoCamion.Queries.ListarReportesPasoCamion;

internal sealed class ListarReportesPasoCamionQueryValidator : AbstractValidator<ListarReportesPasoCamionQuery>
{
    public ListarReportesPasoCamionQueryValidator()
    {
        RuleFor(x => x.RutaId).NotEmpty();
        RuleFor(x => x.Maximo).InclusiveBetween(1, 200);
    }
}
