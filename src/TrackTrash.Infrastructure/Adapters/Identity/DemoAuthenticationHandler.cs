using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TrackTrash.Infrastructure.Adapters.Identity;

// Exclusivamente para desarrollo local. Este esquema no se registra en Production.
public sealed class DemoAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger, UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var value = Request.Headers.Authorization.ToString();
        if (!value.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(AuthenticateResult.NoResult());
        var actor = value[7..] switch
        {
            "demo-vecino" => ("11111111-1111-1111-1111-111111111111", "vecino"),
            "demo-reciclador" => ("22222222-2222-2222-2222-222222222222", "reciclador"),
            "demo-vecino-2" => ("33333333-3333-3333-3333-333333333333", "vecino"),
            "demo-reciclador-2" => ("44444444-4444-4444-4444-444444444444", "reciclador"),
            "demo-admin" => ("55555555-5555-5555-5555-555555555555", "admin"),
            _ => ("", "")
        };
        if (actor.Item1 == "") return Task.FromResult(AuthenticateResult.Fail("Sesión de demostración inválida."));
        var identity = new ClaimsIdentity([new Claim("sub", actor.Item1), new Claim("tracktrash_role", actor.Item2)],
            Scheme.Name, "sub", "tracktrash_role");
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name)));
    }
}
