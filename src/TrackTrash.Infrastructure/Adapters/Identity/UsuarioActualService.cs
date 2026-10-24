using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;

namespace TrackTrash.Infrastructure.Adapters.Identity;

public sealed class UsuarioActualService(IHttpContextAccessor context) : IUsuarioActualService
{
    private ClaimsPrincipal Usuario => context.HttpContext?.User ?? throw new AccesoDenegadoException();
    public Guid Id => Guid.TryParse(Usuario.FindFirstValue("sub"), out var id) ? id : throw new AccesoDenegadoException();
    public bool EsReciclador => Usuario.IsInRole("reciclador");
}
