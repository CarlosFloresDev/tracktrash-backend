using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace TrackTrash.Infrastructure.Adapters.Identity;

// Supabase publica JWKS; no dependemos de un documento de descubrimiento OIDC.
public sealed class SupabaseSigningKeyRetriever(string issuer) : IConfigurationRetriever<OpenIdConnectConfiguration>
{
    public async Task<OpenIdConnectConfiguration> GetConfigurationAsync(string address, IDocumentRetriever retriever, CancellationToken ct)
    {
        var document = await retriever.GetDocumentAsync(address, ct);
        var configuration = new OpenIdConnectConfiguration { Issuer = issuer };
        foreach (var key in new JsonWebKeySet(document).GetSigningKeys())
            configuration.SigningKeys.Add(key);
        return configuration;
    }
}
