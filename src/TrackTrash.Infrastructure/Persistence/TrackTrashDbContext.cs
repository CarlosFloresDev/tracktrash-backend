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
    public DbSet<MaterialAceptado> MaterialesAceptados => Set<MaterialAceptado>();
    public DbSet<PuntoAcopio> PuntosAcopio => Set<PuntoAcopio>();
    public DbSet<Contenedor> Contenedores => Set<Contenedor>();
    public DbSet<DispositivoIoT> Dispositivos => Set<DispositivoIoT>();
    public DbSet<LecturaNivel> Lecturas => Set<LecturaNivel>();
    public DbSet<ReporteCiudadano> ReportesCiudadanos => Set<ReporteCiudadano>();
    public DbSet<RutaRecoleccion> Rutas => Set<RutaRecoleccion>();
    public DbSet<SuscripcionRuta> SuscripcionesRuta => Set<SuscripcionRuta>();
    public DbSet<ReportePasoCamion> ReportesPasoCamion => Set<ReportePasoCamion>();
    public DbSet<Incidencia> Incidencias => Set<Incidencia>();
    public DbSet<Notificacion> Notificaciones => Set<Notificacion>();
    public DbSet<Beneficio> Beneficios => Set<Beneficio>();
    public DbSet<Canje> Canjes => Set<Canje>();
    public DbSet<HistoricoSolicitud> Historial => Set<HistoricoSolicitud>();
    public DbSet<EventoAuditoria> EventosAuditoria => Set<EventoAuditoria>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.HasDefaultSchema("tracktrash");
        ConfigurarSolicitudes(model);
        ConfigurarUsuariosYRecompensas(model);
        ConfigurarCatalogos(model);
        ConfigurarContenedores(model);
        ConfigurarRutas(model);
        ConfigurarOperaciones(model);
    }

    private static void ConfigurarSolicitudes(ModelBuilder model)
    {
        var s = model.Entity<SolicitudRecojo>();
        s.ToTable("solicitudes_recojo"); s.HasKey(x => x.Id);
        s.Property(x => x.Material).HasConversion<string>().HasMaxLength(30);
        s.Property(x => x.Estado).HasConversion<string>().HasMaxLength(30);
        s.Property(x => x.CantidadEstimadaKg).HasPrecision(10, 2);
        s.Property(x => x.Direccion).HasMaxLength(250);
        s.Property(x => x.MotivoCancelacion).HasMaxLength(300);
        s.Property(x => x.CodigoConfirmacion).HasMaxLength(6);
        s.Property(x => x.Version).IsConcurrencyToken();
        s.HasIndex(x => new { x.VecinoId, x.ClaveSolicitud }).IsUnique();
        s.HasIndex(x => new { x.Estado, x.CreadaEn });
        s.HasIndex(x => new { x.RecicladorId, x.CreadaEn });

        var h = model.Entity<HistoricoSolicitud>();
        h.ToTable("historico_solicitudes"); h.HasKey(x => x.Id);
        h.Property(x => x.Accion).HasMaxLength(100);
        h.HasIndex(x => new { x.SolicitudId, x.CreadoEn });
        h.HasOne<SolicitudRecojo>().WithMany().HasForeignKey(x => x.SolicitudId).OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigurarUsuariosYRecompensas(ModelBuilder model)
    {
        var perfil = model.Entity<PerfilUsuario>();
        perfil.ToTable("perfiles_usuario"); perfil.HasKey(x => x.Id);
        perfil.Property(x => x.Nombre).HasMaxLength(100);
        perfil.Property(x => x.Telefono).HasMaxLength(20);
        perfil.Property(x => x.Direccion).HasMaxLength(250);
        perfil.Property(x => x.Version).IsConcurrencyToken();

        var movimiento = model.Entity<MovimientoPuntos>();
        movimiento.ToTable("movimientos_puntos"); movimiento.HasKey(x => x.Id);
        movimiento.HasIndex(x => x.SolicitudId).IsUnique().HasFilter("\"SolicitudId\" IS NOT NULL");
        movimiento.HasIndex(x => x.CanjeId).IsUnique().HasFilter("\"CanjeId\" IS NOT NULL");
        movimiento.HasIndex(x => new { x.UsuarioId, x.CreadoEn });
        movimiento.HasOne<SolicitudRecojo>().WithMany().HasForeignKey(x => x.SolicitudId).OnDelete(DeleteBehavior.Restrict);

        var beneficio = model.Entity<Beneficio>();
        beneficio.ToTable("beneficios"); beneficio.HasKey(x => x.Id);
        beneficio.Property(x => x.Nombre).HasMaxLength(120);
        beneficio.Property(x => x.Descripcion).HasMaxLength(500);
        beneficio.Property(x => x.Version).IsConcurrencyToken();

        var canje = model.Entity<Canje>();
        canje.ToTable("canjes"); canje.HasKey(x => x.Id);
        canje.Property(x => x.Codigo).HasMaxLength(50);
        canje.HasIndex(x => new { x.UsuarioId, x.Clave }).IsUnique();
        canje.HasIndex(x => new { x.UsuarioId, x.CreadoEn });
        canje.HasOne<Beneficio>().WithMany().HasForeignKey(x => x.BeneficioId).OnDelete(DeleteBehavior.Restrict);
        movimiento.HasOne<Canje>().WithMany().HasForeignKey(x => x.CanjeId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigurarCatalogos(ModelBuilder model)
    {
        var distrito = model.Entity<Distrito>();
        distrito.ToTable("distritos"); distrito.HasKey(x => x.Id);
        distrito.Property(x => x.Nombre).HasMaxLength(80); distrito.HasIndex(x => x.Nombre).IsUnique();

        var asociacion = model.Entity<AsociacionReciclaje>();
        asociacion.ToTable("asociaciones_reciclaje"); asociacion.HasKey(x => x.Id);
        asociacion.Property(x => x.Nombre).HasMaxLength(120);
        asociacion.Property(x => x.Direccion).HasMaxLength(250);
        asociacion.Property(x => x.Horario).HasMaxLength(150);
        asociacion.Property(x => x.CapacidadKg).HasPrecision(12, 2);
        asociacion.Property(x => x.ReservadoKg).HasPrecision(12, 2);
        asociacion.Property(x => x.Version).IsConcurrencyToken();
        asociacion.HasOne<Distrito>().WithMany().HasForeignKey(x => x.DistritoId).OnDelete(DeleteBehavior.Restrict);

        var material = model.Entity<MaterialAceptado>();
        material.ToTable("materiales_aceptados"); material.HasKey(x => new { x.AsociacionId, x.Material });
        material.Property(x => x.Material).HasConversion<string>().HasMaxLength(30);
        asociacion.HasMany(x => x.Materiales).WithOne().HasForeignKey(x => x.AsociacionId).OnDelete(DeleteBehavior.Cascade);

        var punto = model.Entity<PuntoAcopio>();
        punto.ToTable("puntos_acopio"); punto.HasKey(x => x.Id);
        punto.Property(x => x.Nombre).HasMaxLength(120); punto.Property(x => x.Direccion).HasMaxLength(250);
    }

    private static void ConfigurarContenedores(ModelBuilder model)
    {
        var contenedor = model.Entity<Contenedor>();
        contenedor.ToTable("contenedores"); contenedor.HasKey(x => x.Id);
        contenedor.Property(x => x.Nombre).HasMaxLength(120); contenedor.Property(x => x.Fuente).HasMaxLength(50);
        contenedor.Property(x => x.Version).IsConcurrencyToken();
        contenedor.HasOne<Distrito>().WithMany().HasForeignKey(x => x.DistritoId).OnDelete(DeleteBehavior.Restrict);

        var dispositivo = model.Entity<DispositivoIoT>();
        dispositivo.ToTable("dispositivos_iot"); dispositivo.HasKey(x => x.Id);
        dispositivo.Property(x => x.ClaveHash).HasMaxLength(200); dispositivo.Property(x => x.Version).IsConcurrencyToken();
        dispositivo.HasIndex(x => x.ContenedorId).IsUnique();
        dispositivo.HasOne<Contenedor>().WithMany().HasForeignKey(x => x.ContenedorId).OnDelete(DeleteBehavior.Cascade);

        var lectura = model.Entity<LecturaNivel>();
        lectura.ToTable("lecturas_nivel"); lectura.HasKey(x => x.Id);
        lectura.HasIndex(x => new { x.DispositivoId, x.Secuencia }).IsUnique();
        lectura.HasIndex(x => new { x.DispositivoId, x.MedidaEn });
        lectura.HasOne<DispositivoIoT>().WithMany().HasForeignKey(x => x.DispositivoId).OnDelete(DeleteBehavior.Cascade);

        var reporte = model.Entity<ReporteCiudadano>();
        reporte.ToTable("reportes_ciudadanos"); reporte.HasKey(x => x.Id);
        reporte.HasIndex(x => new { x.ContenedorId, x.UsuarioId, x.CreadoEn });
        reporte.HasOne<Contenedor>().WithMany().HasForeignKey(x => x.ContenedorId).OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigurarRutas(ModelBuilder model)
    {
        var ruta = model.Entity<RutaRecoleccion>();
        ruta.ToTable("rutas_recoleccion"); ruta.HasKey(x => x.Id);
        ruta.Property(x => x.Nombre).HasMaxLength(120); ruta.Property(x => x.Zona).HasMaxLength(150);
        ruta.Property(x => x.Dias).HasMaxLength(30);
        ruta.HasOne<Distrito>().WithMany().HasForeignKey(x => x.DistritoId).OnDelete(DeleteBehavior.Restrict);

        var suscripcion = model.Entity<SuscripcionRuta>();
        suscripcion.ToTable("suscripciones_ruta"); suscripcion.HasKey(x => x.Id);
        suscripcion.Property(x => x.Version).IsConcurrencyToken();
        suscripcion.HasIndex(x => new { x.RutaId, x.UsuarioId }).IsUnique(); suscripcion.HasIndex(x => x.ProximoAviso);
        suscripcion.HasOne<RutaRecoleccion>().WithMany().HasForeignKey(x => x.RutaId).OnDelete(DeleteBehavior.Cascade);

        var reporte = model.Entity<ReportePasoCamion>();
        reporte.ToTable("reportes_paso_camion"); reporte.HasKey(x => x.Id);
        reporte.HasIndex(x => new { x.RutaId, x.UsuarioId, x.ReportadoEn });
        reporte.HasOne<RutaRecoleccion>().WithMany().HasForeignKey(x => x.RutaId).OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigurarOperaciones(ModelBuilder model)
    {
        var incidencia = model.Entity<Incidencia>();
        incidencia.ToTable("incidencias"); incidencia.HasKey(x => x.Id);
        incidencia.Property(x => x.Tipo).HasMaxLength(60); incidencia.Property(x => x.Descripcion).HasMaxLength(1000);
        incidencia.Property(x => x.Resolucion).HasMaxLength(1000);
        incidencia.Property(x => x.Estado).HasConversion<string>().HasMaxLength(30);
        incidencia.Property(x => x.Version).IsConcurrencyToken(); incidencia.HasIndex(x => new { x.ContenedorId, x.Estado });
        incidencia.HasOne<Contenedor>().WithMany().HasForeignKey(x => x.ContenedorId).OnDelete(DeleteBehavior.Restrict);

        var notificacion = model.Entity<Notificacion>();
        notificacion.ToTable("notificaciones"); notificacion.HasKey(x => x.Id);
        notificacion.Property(x => x.ClaveUnica).HasMaxLength(150); notificacion.Property(x => x.Titulo).HasMaxLength(150);
        notificacion.Property(x => x.Mensaje).HasMaxLength(1000);
        notificacion.Property(x => x.Tipo).HasConversion<string>().HasMaxLength(30);
        notificacion.HasIndex(x => new { x.UsuarioId, x.ClaveUnica }).IsUnique();
        notificacion.HasIndex(x => new { x.UsuarioId, x.CreadaEn });

        var auditoria = model.Entity<EventoAuditoria>();
        auditoria.ToTable("eventos_auditoria"); auditoria.HasKey(x => x.Id);
        auditoria.Property(x => x.Actor).HasMaxLength(100); auditoria.Property(x => x.Entidad).HasMaxLength(100);
        auditoria.Property(x => x.Referencia).HasMaxLength(100); auditoria.Property(x => x.Accion).HasMaxLength(100);
        auditoria.Property(x => x.Campos).HasMaxLength(2000);
        auditoria.HasIndex(x => new { x.Entidad, x.Referencia, x.CreadoEn });
    }
}
