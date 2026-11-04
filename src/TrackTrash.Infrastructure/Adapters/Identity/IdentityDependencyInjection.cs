using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using TrackTrash.Application.Ports.Services;

namespace TrackTrash.Infrastructure.Adapters.Identity;

public static class IdentityDependencyInjection
{
    public static IServiceCollection AddTrackTrashIdentity(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        var section = configuration.GetSection(JwtIdentityOptions.SectionName);
        services.AddOptions<JwtIdentityOptions>().Bind(section);
        var options = section.Get<JwtIdentityOptions>() ?? new JwtIdentityOptions();

        services.AddHttpContextAccessor();
        services.AddScoped<IUsuarioActualService, UsuarioActualService>();

        if (options.Demo)
        {
            if (!environment.IsDevelopment())
                throw new InvalidOperationException("La autenticación de demostración solo está permitida en Development.");
            services.AddAuthentication("Demo")
                .AddScheme<AuthenticationSchemeOptions, DemoAuthenticationHandler>("Demo", _ => { });
        }
        else
        {
            Validar(options);
            var authority = options.Authority.TrimEnd('/');
            services.AddScoped<JwtBearerValidationEvents>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwt =>
            {
                jwt.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    authority + "/.well-known/jwks.json",
                    new SupabaseSigningKeyRetriever(authority),
                    new HttpDocumentRetriever { RequireHttps = true })
                {
                    AutomaticRefreshInterval = TimeSpan.FromMinutes(options.RefreshIntervalMinutes)
                };
                jwt.MapInboundClaims = false;
                jwt.Audience = options.Audience;
                jwt.EventsType = typeof(JwtBearerValidationEvents);
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authority,
                    ValidateAudience = true,
                    ValidAudience = options.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromMinutes(1),
                    NameClaimType = ClaimsTrackTrash.Sujeto,
                    RoleClaimType = ClaimsTrackTrash.Rol
                };
            });
        }

        services.AddAuthorizationBuilder()
            .AddPolicy(PoliticasTrackTrash.SoloVecinos,
                policy => policy.RequireRole(RolesTrackTrash.Vecino))
            .AddPolicy(PoliticasTrackTrash.SoloRecicladores,
                policy => policy.RequireRole(RolesTrackTrash.Reciclador))
            .AddPolicy(PoliticasTrackTrash.SoloAdministradores,
                policy => policy.RequireRole(RolesTrackTrash.Administrador))
            .AddPolicy(PoliticasTrackTrash.Operadores,
                policy => policy.RequireRole(RolesTrackTrash.Reciclador, RolesTrackTrash.Administrador));
        return services;
    }

    private static void Validar(JwtIdentityOptions options)
    {
        if (!Uri.TryCreate(options.Authority, UriKind.Absolute, out var authority) || authority.Scheme != "https")
            throw new OptionsValidationException(JwtIdentityOptions.SectionName,
                typeof(JwtIdentityOptions), ["Authentication:Authority debe ser una URL HTTPS absoluta."]);
        if (string.IsNullOrWhiteSpace(options.Audience))
            throw new OptionsValidationException(JwtIdentityOptions.SectionName,
                typeof(JwtIdentityOptions), ["Authentication:Audience es obligatorio."]);
        if (string.IsNullOrWhiteSpace(options.RoleMetadataClaim) || string.IsNullOrWhiteSpace(options.RoleProperty))
            throw new OptionsValidationException(JwtIdentityOptions.SectionName,
                typeof(JwtIdentityOptions), ["Los nombres de claims de rol son obligatorios."]);
        if (options.RefreshIntervalMinutes is < 5 or > 1440)
            throw new OptionsValidationException(JwtIdentityOptions.SectionName,
                typeof(JwtIdentityOptions), ["Authentication:RefreshIntervalMinutes debe estar entre 5 y 1440."]);
    }
}
