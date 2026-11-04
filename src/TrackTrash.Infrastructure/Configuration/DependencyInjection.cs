using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.Ports.Queries;
using TrackTrash.Application.Behaviors;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Infrastructure.Adapters.Identity;
using TrackTrash.Infrastructure.Adapters;
using TrackTrash.Infrastructure.Persistence;
using TrackTrash.Infrastructure.Persistence.Repositories;
using TrackTrash.Infrastructure.Persistence.Queries;

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
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ISolicitudRecojoRepository, SolicitudRecojoRepository>();
        services.AddScoped<IMovimientoPuntosRepository, MovimientoPuntosRepository>();
        services.AddScoped<IBeneficioRepository, BeneficioRepository>();
        services.AddScoped<ICatalogoRepository, CatalogoRepository>();
        services.AddScoped<IContenedorRepository, ContenedorRepository>();
        services.AddScoped<IHistoricoRepository, HistoricoRepository>();
        services.AddScoped<IIncidenciaRepository, IncidenciaRepository>();
        services.AddScoped<INotificacionRepository, NotificacionRepository>();
        services.AddScoped<IPerfilRepository, PerfilRepository>();
        services.AddScoped<IRutaRepository, RutaRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<INotificadorUsuarioService, NotificadorUsuarioService>();
        services.AddSingleton<ISeguridadDispositivoService, SeguridadDispositivoService>();
        services.AddScoped<IIndicadoresConsulta, IndicadoresConsulta>();
        services.AddScoped<IAuditoriaConsulta, AuditoriaConsulta>();
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(ValidationBehavior<,>).Assembly));
        services.AddValidatorsFromAssembly(typeof(ValidationBehavior<,>).Assembly, includeInternalTypes: true);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddSingleton<ICodigoEntregaService, CodigoEntregaService>();
        services.AddSingleton(TimeProvider.System);
        services.AddTrackTrashIdentity(config, environment);
        return services;
    }
}
