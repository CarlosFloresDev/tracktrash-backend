namespace TrackTrash.Application.Ports.Services;

public interface IUsuarioActualService
{
    Guid Id { get; }
    bool EsReciclador { get; }
}
