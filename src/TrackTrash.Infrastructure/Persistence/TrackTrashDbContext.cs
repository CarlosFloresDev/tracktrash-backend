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
        var s = model.Entity<SolicitudRecojo>();
        s.ToTable("solicitudes_recojo");
        s.HasKey(x => x.Id);
        s.Property(x => x.Material).HasConversion<string>().HasMaxLength(30);
        s.Property(x => x.Estado).HasConversion<string>().HasMaxLength(30);
        s.Property(x => x.CantidadEstimadaKg).HasPrecision(10, 2);
        s.Property(x => x.Direccion).HasMaxLength(250);
        s.Property(x => x.CodigoConfirmacion).HasMaxLength(6);
        s.Property(x => x.Version).IsConcurrencyToken();
        s.HasIndex(x => new { x.VecinoId, x.ClaveSolicitud }).IsUnique();
        s.HasIndex(x => new { x.Estado, x.CreadaEn });
        s.HasIndex(x => new { x.RecicladorId, x.CreadaEn });
        var p = model.Entity<MovimientoPuntos>();
        p.ToTable("movimientos_puntos");
        p.HasKey(x => x.Id);
        p.HasIndex(x => x.SolicitudId).IsUnique();
        p.HasIndex(x => x.UsuarioId);
        p.HasOne<SolicitudRecojo>().WithMany().HasForeignKey(x => x.SolicitudId).OnDelete(DeleteBehavior.Restrict);
    }
}
