using FluentValidation;

namespace TrackTrash.Application.UseCases.EventosAuditoria.Queries.ListarEventosAuditoria;

internal sealed class ListarEventosAuditoriaQueryValidator : AbstractValidator<ListarEventosAuditoriaQuery>
{
    public ListarEventosAuditoriaQueryValidator()
    {
        RuleFor(x => x.Entidad).MaximumLength(80);
        RuleFor(x => x.Maximo).InclusiveBetween(1, 200);
    }
}
