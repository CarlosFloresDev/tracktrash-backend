namespace TrackTrash.Domain.Ports.Persistence;

public interface IUnitOfWork
{
    Task GuardarCambiosAsync(CancellationToken ct);
}
