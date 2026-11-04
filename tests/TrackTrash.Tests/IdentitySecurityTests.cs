using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Infrastructure.Adapters.Identity;

namespace TrackTrash.Tests;

public sealed class IdentitySecurityTests
{
    [Theory]
    [InlineData("vecino", RolUsuario.Vecino)]
    [InlineData("reciclador", RolUsuario.Reciclador)]
    [InlineData("administrador", RolUsuario.Administrador)]
    public void Reconoce_unicamente_los_roles_soportados(string claim, RolUsuario esperado)
    {
        Assert.True(RolesTrackTrash.IntentarObtener(claim, out var actual));
        Assert.Equal(esperado, actual);
        Assert.Equal(claim, actual.ComoClaim());
    }

    [Theory]
    [InlineData("")]
    [InlineData("admin")]
    [InlineData("superusuario")]
    public void Rechaza_roles_no_soportados(string claim) =>
        Assert.False(RolesTrackTrash.IntentarObtener(claim, out _));

    [Fact]
    public void Usuario_actual_expone_id_y_rol_tipados()
    {
        var id = Guid.NewGuid();
        var identity = new ClaimsIdentity(
            [new Claim(ClaimsTrackTrash.Sujeto, id.ToString()),
             new Claim(ClaimsTrackTrash.Rol, RolesTrackTrash.Administrador)],
            "Test", ClaimsTrackTrash.Sujeto, ClaimsTrackTrash.Rol);
        var accessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };

        var usuario = new UsuarioActualService(accessor);

        Assert.Equal(id, usuario.Id);
        Assert.Equal(RolUsuario.Administrador, usuario.Rol);
        Assert.True(usuario.EsAdministrador);
        Assert.False(usuario.EsReciclador);
    }
}
