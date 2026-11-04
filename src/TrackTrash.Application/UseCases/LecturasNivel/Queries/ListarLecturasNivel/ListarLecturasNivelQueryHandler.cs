using MediatR;
using TrackTrash.Application.DTOs.LecturasNivel;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.LecturasNivel.Queries.ListarLecturasNivel;

internal sealed class ListarLecturasNivelQueryHandler(
    IContenedorRepository repo,
    IUsuarioActualService usuario) : IRequestHandler<ListarLecturasNivelQuery, IReadOnlyList<LecturaNivelDto>>
{
    public async Task<IReadOnlyList<LecturaNivelDto>> Handle(ListarLecturasNivelQuery request, CancellationToken cancellationToken)
    {
        if (!usuario.EsReciclador && !usuario.EsAdministrador) throw new AccesoDenegadoException();
        return (await repo.LecturasAsync(request.DispositivoId, request.Maximo, cancellationToken))
            .Select(l => new LecturaNivelDto(l.Id, l.DispositivoId, l.Secuencia, l.Porcentaje, l.Bateria, l.MedidaEn, l.RecibidaEn)).ToList();
    }
}
