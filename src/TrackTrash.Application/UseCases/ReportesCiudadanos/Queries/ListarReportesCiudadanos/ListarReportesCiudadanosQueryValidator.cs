using FluentValidation;

namespace TrackTrash.Application.UseCases.ReportesCiudadanos.Queries.ListarReportesCiudadanos;

internal sealed class ListarReportesCiudadanosQueryValidator : AbstractValidator<ListarReportesCiudadanosQuery>
{
    public ListarReportesCiudadanosQueryValidator()
    {
        RuleFor(x => x.ContenedorId).NotEmpty();
        RuleFor(x => x.Maximo).InclusiveBetween(1, 200);
    }
}
