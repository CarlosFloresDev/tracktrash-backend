using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;

namespace TrackTrash.Infrastructure.Adapters.Identity;

public sealed class UsuarioActualService(IHttpContextAccessor context) : IUsuarioActualService
{
    private ClaimsPrincipal Usuario => context.HttpContext?.User ?? throw new AccesoDenegadoException();
    public Guid Id => Guid.TryParse(Usuario.FindFirstValue(ClaimsTrackTrash.Sujeto), out var id) ? id : throw new AccesoDenegadoException();
    public RolUsuario Rol
    {
        get
        {
            var value = Usuario.FindFirstValue(ClaimsTrackTrash.Rol);
            return RolesTrackTrash.IntentarObtener(value, out var rol) ? rol : throw new AccesoDenegadoException();
        }
    }
    public bool EsReciclador => EstaEnRol(RolUsuario.Reciclador);
    public bool EsAdministrador => EstaEnRol(RolUsuario.Administrador);
    public bool EstaEnRol(RolUsuario rol) => Rol == rol;
}
