using Microsoft.Extensions.Configuration;

namespace TrackTrash.Infrastructure.Configuration;

public static class RenderHosting
{
    public static bool HasManagedHttps(IConfiguration configuration)
    {
        if (!configuration.GetValue<bool>("RENDER")) return false;
        if (!Uri.TryCreate(configuration["RENDER_EXTERNAL_URL"], UriKind.Absolute, out var origin)
            || origin.Scheme != Uri.UriSchemeHttps)
            throw new InvalidOperationException("Render debe proporcionar RENDER_EXTERNAL_URL con HTTPS.");
        return true;
    }
}
