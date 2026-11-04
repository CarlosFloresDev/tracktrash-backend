using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using TrackTrash.Application.Ports.Services;

namespace TrackTrash.Infrastructure.Adapters.Identity;

public sealed class JwtBearerValidationEvents(IOptions<JwtIdentityOptions> options) : JwtBearerEvents
{
    public override Task TokenValidated(TokenValidatedContext context)
    {
        var identity = context.Principal?.Identity as ClaimsIdentity;
        if (identity is null || !Guid.TryParse(identity.FindFirst(ClaimsTrackTrash.Sujeto)?.Value, out _))
        {
            context.Fail("El token no identifica a un usuario válido.");
            return Task.CompletedTask;
        }

        var role = ObtenerRol(identity.FindFirst(options.Value.RoleMetadataClaim)?.Value,
            options.Value.RoleProperty);
        foreach (var existingRole in identity.FindAll(ClaimsTrackTrash.Rol).ToArray())
            identity.RemoveClaim(existingRole);
        identity.AddClaim(new Claim(ClaimsTrackTrash.Rol, role.ComoClaim()));
        return Task.CompletedTask;
    }

    private static RolUsuario ObtenerRol(string? metadata, string propiedad)
    {
        if (string.IsNullOrWhiteSpace(metadata)) return RolUsuario.Vecino;
        try
        {
            using var document = JsonDocument.Parse(metadata);
            if (document.RootElement.ValueKind == JsonValueKind.Object
                && document.RootElement.TryGetProperty(propiedad, out var value)
                && RolesTrackTrash.IntentarObtener(value.GetString(), out var rol))
                return rol;
        }
        catch (JsonException)
        {
            // Un claim mal formado no eleva privilegios: conserva el rol mínimo.
        }
        return RolUsuario.Vecino;
    }
}
