using FluentValidation;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Queries.ListarSolicitudesRecojo;

internal sealed class ListarSolicitudesRecojoQueryValidator : AbstractValidator<ListarSolicitudesRecojoQuery>
{
    public ListarSolicitudesRecojoQueryValidator() =>
        RuleFor(x => x.Pagina).InclusiveBetween(1, 10000).WithMessage("La página debe estar entre 1 y 10000.");
}
