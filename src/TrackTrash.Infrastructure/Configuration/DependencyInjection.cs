using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.Entregas;
using TrackTrash.Application.UseCases.Recompensas;
using TrackTrash.Application.UseCases.SolicitudesRecojo;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Infrastructure.Adapters.Identity;
using TrackTrash.Infrastructure.Persistence;
using TrackTrash.Infrastructure.Persistence.Repositories;

namespace TrackTrash.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration config, IHostEnvironment environment)
    {
        services.AddDbContext<TrackTrashDbContext>(o => o.UseNpgsql(
            config.GetConnectionString("TrackTrash") ?? throw new InvalidOperationException(
                "Configura ConnectionStrings:TrackTrash mediante user-secrets o variables de entorno."),
            npg => npg.MigrationsHistoryTable("__EFMigrationsHistory", "tracktrash")));
        services.AddScoped<ISolicitudRecojoRepository, SolicitudRecojoRepository>();
        services.AddScoped<IMovimientoPuntosRepository, MovimientoPuntosRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddHttpContextAccessor();
        services.AddScoped<IUsuarioActualService, UsuarioActualService>();
        services.AddSingleton<ICodigoEntregaService, CodigoEntregaService>();
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<CrearSolicitudRecojoUseCase>();
        services.AddScoped<ListarSolicitudesRecojoUseCase>();
        services.AddScoped<ObtenerSolicitudRecojoUseCase>();
        services.AddScoped<AceptarSolicitudRecojoUseCase>();
        services.AddScoped<ConfirmarEntregaUseCase>();
        services.AddScoped<ObtenerImpactoUseCase>();

        if (config.GetValue<bool>("Authentication:Demo"))
        {
            if (!environment.IsDevelopment())
                throw new InvalidOperationException("La autenticación de demostración solo está permitida en Development.");
            services.AddAuthentication("Demo").AddScheme<AuthenticationSchemeOptions, DemoAuthenticationHandler>("Demo", _ => { });
        }
        else
        {
            var authority = config["Authentication:Authority"];
            if (!Uri.TryCreate(authority, UriKind.Absolute, out var uri) || uri.Scheme != "https")
                throw new InvalidOperationException("Configura Authentication:Authority con la URL HTTPS de Supabase Auth.");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {
                o.Authority = authority;
                o.Audience = "authenticated";
                o.MapInboundClaims = false;
                o.TokenValidationParameters.NameClaimType = "sub";
                o.TokenValidationParameters.RoleClaimType = "tracktrash_role";
                o.Events = new JwtBearerEvents { OnTokenValidated = context =>
                {
                    var identity = (ClaimsIdentity)context.Principal!.Identity!;
                    if (!Guid.TryParse(identity.FindFirst("sub")?.Value, out _))
                    { context.Fail("El token no identifica a un usuario válido."); return Task.CompletedTask; }
                    var metadata = identity.FindFirst("app_metadata")?.Value;
                    var role = "vecino";
                    if (metadata is not null)
                    {
                        using var doc = JsonDocument.Parse(metadata);
                        if (doc.RootElement.TryGetProperty("tracktrash_role", out var r) && r.GetString() == "reciclador")
                            role = "reciclador";
                    }
                    identity.AddClaim(new Claim("tracktrash_role", role));
                    return Task.CompletedTask;
                }};
            });
        }
        services.AddAuthorization();
        return services;
    }
}
