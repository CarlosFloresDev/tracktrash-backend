using FluentValidation;

namespace TrackTrash.Application.UseCases.LecturasNivel.Queries.ListarLecturasNivel;

internal sealed class ListarLecturasNivelQueryValidator : AbstractValidator<ListarLecturasNivelQuery>
{
    public ListarLecturasNivelQueryValidator()
    {
        RuleFor(x => x.DispositivoId).NotEmpty();
        RuleFor(x => x.Maximo).InclusiveBetween(1, 200);
    }
}
