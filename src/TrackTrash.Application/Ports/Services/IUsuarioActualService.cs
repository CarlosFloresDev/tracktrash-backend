namespace TrackTrash.Application.Ports.Services;

public interface IUsuarioActualService
{
    Guid Id { get; }
    RolUsuario Rol { get; }
    bool EsReciclador { get; }
    bool EsAdministrador { get; }
    bool EstaEnRol(RolUsuario rol);
}
