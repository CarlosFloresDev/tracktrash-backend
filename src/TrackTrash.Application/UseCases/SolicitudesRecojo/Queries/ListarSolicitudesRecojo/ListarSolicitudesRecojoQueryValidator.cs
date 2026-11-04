using TrackTrash.Application.DTOs.SolicitudesRecojo;
using FluentValidation;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Queries.ListarSolicitudesRecojo;

// Valida la FORMA de la petición antes de llegar al handler (ValidationBehavior -> HTTP 400).
// Las invariantes de negocio siguen protegidas en el dominio (ReglaNegocioException -> HTTP 422).
internal sealed class ListarSolicitudesRecojoQueryValidator : AbstractValidator<ListarSolicitudesRecojoQuery>
{
    public ListarSolicitudesRecojoQueryValidator()
    {
        RuleFor(x => x.Pagina).InclusiveBetween(1, 10000).WithMessage("La página debe estar entre 1 y 10000.");
    }
}
