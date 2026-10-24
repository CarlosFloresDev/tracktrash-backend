using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Infrastructure.Persistence;

public sealed class TrackTrashDbContext(DbContextOptions<TrackTrashDbContext> options) : DbContext(options)
{
    public DbSet<SolicitudRecojo> Solicitudes => Set<SolicitudRecojo>();
    public DbSet<MovimientoPuntos> MovimientosPuntos => Set<MovimientoPuntos>();
    public DbSet<PerfilUsuario> Perfiles => Set<PerfilUsuario>();
    public DbSet<Distrito> Distritos => Set<Distrito>();
    public DbSet<AsociacionReciclaje> Asociaciones => Set<AsociacionReciclaje>();
    public DbSet<PuntoAcopio> PuntosAcopio => Set<PuntoAcopio>();
    public DbSet<Contenedor> Contenedores => Set<Contenedor>();
    public DbSet<DispositivoIoT> Dispositivos => Set<DispositivoIoT>();
    public DbSet<LecturaNivel> Lecturas => Set<LecturaNivel>();
    public DbSet<Incidencia> Incidencias => Set<Incidencia>();
    public DbSet<Notificacion> Notificaciones => Set<Notificacion>();
    public DbSet<Beneficio> Beneficios => Set<Beneficio>();
    public DbSet<Canje> Canjes => Set<Canje>();
    public DbSet<HistoricoSolicitud> Historial => Set<HistoricoSolicitud>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.HasDefaultSchema("tracktrash");
        model.ConfigurarEcosistema();
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
        p.HasIndex(x => x.CanjeId).IsUnique();
        p.HasOne<Canje>().WithMany().HasForeignKey(x => x.CanjeId).OnDelete(DeleteBehavior.Restrict);
        p.HasOne<SolicitudRecojo>().WithMany().HasForeignKey(x => x.SolicitudId).OnDelete(DeleteBehavior.Restrict);
    }
}
