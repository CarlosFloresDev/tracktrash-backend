using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using TrackTrash.Infrastructure.Adapters.Identity;

namespace TrackTrash.Tests;

public sealed class SupabaseSigningKeysTests
{
    private sealed class Document(string json) : IDocumentRetriever
    {
        public Task<string> GetDocumentAsync(string address, CancellationToken ct) => Task.FromResult(json);
    }

    [Fact]
    public async Task Lee_claves_publicas_desde_JWKS_sin_descubrimiento_OIDC()
    {
        using var rsa = RSA.Create(2048);
        var key = JsonWebKeyConverter.ConvertFromRSASecurityKey(new RsaSecurityKey(rsa));
        key.Kid = "test-key";
        var json = JsonSerializer.Serialize(new { keys = new[] { new { kty = "RSA", kid = key.Kid, n = key.N, e = key.E, use = "sig" } } });
        var configuration = await new SupabaseSigningKeyRetriever("https://example.supabase.co/auth/v1")
            .GetConfigurationAsync("https://example.supabase.co/auth/v1/.well-known/jwks.json", new Document(json), default);
        Assert.Equal("https://example.supabase.co/auth/v1", configuration.Issuer);
        Assert.Equal("test-key", Assert.Single(configuration.SigningKeys).KeyId);
    }
}
