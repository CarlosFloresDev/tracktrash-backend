using MediatR;
using TrackTrash.Application.DTOs.LecturasNivel;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.LecturasNivel.Queries.ObtenerUltimaLectura;

internal sealed class ObtenerUltimaLecturaQueryHandler(
    IContenedorRepository repo,
    IUsuarioActualService usuario) : IRequestHandler<ObtenerUltimaLecturaQuery, LecturaNivelDto>
{
    public async Task<LecturaNivelDto> Handle(ObtenerUltimaLecturaQuery request, CancellationToken cancellationToken)
    {
        if (!usuario.EsReciclador && !usuario.EsAdministrador) throw new AccesoDenegadoException();
        var l = (await repo.LecturasAsync(request.DispositivoId, 1, cancellationToken)).FirstOrDefault()
            ?? throw new RecursoNoEncontradoException();
        return new LecturaNivelDto(l.Id, l.DispositivoId, l.Secuencia, l.Porcentaje, l.Bateria, l.MedidaEn, l.RecibidaEn);
    }
}
