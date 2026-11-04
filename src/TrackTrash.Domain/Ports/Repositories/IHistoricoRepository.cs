using TrackTrash.Domain.Entities;

namespace TrackTrash.Domain.Ports.Repositories;

public interface IHistoricoRepository
{
    void Agregar(HistoricoSolicitud historico);
    Task<IReadOnlyList<HistoricoSolicitud>> ListarAsync(Guid solicitudId, CancellationToken ct);
}
