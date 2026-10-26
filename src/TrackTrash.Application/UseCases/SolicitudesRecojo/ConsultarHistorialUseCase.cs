using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Application.UseCases.SolicitudesRecojo;

public sealed class ConsultarHistorialUseCase(ISolicitudRecojoRepository repo, IHistoricoRepository historial, IUsuarioActualService usuario)
{
    public async Task<IReadOnlyList<HistoricoDto>> ExecuteAsync(Guid id, CancellationToken ct)
    {
        var s = await repo.ObtenerAsync(id, ct) ?? throw new RecursoNoEncontradoException();
        if (s.VecinoId != usuario.Id && s.RecicladorId != usuario.Id && !usuario.EsAdministrador) throw new RecursoNoEncontradoException();
        return (await historial.ListarAsync(id, ct)).Select(e => new HistoricoDto(e.Accion, e.CreadoEn)).ToList();
    }
}
