using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Infrastructure.Persistence;

public sealed class TrackTrashDbContext(DbContextOptions<TrackTrashDbContext> options) : DbContext(options)
{
    public DbSet<SolicitudRecojo> Solicitudes => Set<SolicitudRecojo>();
    public DbSet<MovimientoPuntos> MovimientosPuntos => Set<MovimientoPuntos>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.HasDefaultSchema("tracktrash");
        model.ApplyConfigurationsFromAssembly(typeof(TrackTrashDbContext).Assembly);
    }
}
