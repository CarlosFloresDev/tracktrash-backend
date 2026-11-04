using FluentValidation;

namespace TrackTrash.Application.UseCases.MaterialesAceptados.Queries.ListarMaterialesAceptados;

internal sealed class ListarMaterialesAceptadosQueryValidator : AbstractValidator<ListarMaterialesAceptadosQuery>
{
    public ListarMaterialesAceptadosQueryValidator()
    {
        RuleFor(x => x.AsociacionId).NotEmpty();
    }
}
