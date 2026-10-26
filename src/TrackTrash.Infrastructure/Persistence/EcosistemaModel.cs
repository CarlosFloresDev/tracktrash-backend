using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Infrastructure.Persistence;

public static class EcosistemaModel
{
    public static void ConfigurarEcosistema(this ModelBuilder m)
    {
        m.HasPostgresExtension("postgis");
        m.Entity<EventoAuditoria>().ToTable("eventos_auditoria").HasKey(x => x.Id);
        m.Entity<EventoAuditoria>().HasIndex(x => new { x.Entidad, x.CreadoEn });
        m.Entity<ReporteCiudadano>().ToTable("reportes_ciudadanos").HasKey(x => x.Id);
        m.Entity<ReporteCiudadano>().HasOne<Contenedor>().WithMany().HasForeignKey(x => x.ContenedorId);
        m.Entity<ReporteCiudadano>().HasIndex(x => new { x.ContenedorId, x.UsuarioId, x.CreadoEn });
        m.Entity<RutaRecoleccion>().ToTable("rutas_recoleccion").HasKey(x => x.Id);
        m.Entity<RutaRecoleccion>().HasOne<Distrito>().WithMany().HasForeignKey(x => x.DistritoId);
        m.Entity<SuscripcionRuta>().ToTable("suscripciones_ruta").HasKey(x => x.Id);
        m.Entity<SuscripcionRuta>().HasIndex(x => new { x.UsuarioId, x.RutaId }).IsUnique();
        m.Entity<SuscripcionRuta>().HasIndex(x => x.ProximoAviso);
        m.Entity<SuscripcionRuta>().Property(x => x.Version).IsConcurrencyToken();
        m.Entity<SuscripcionRuta>().HasOne<RutaRecoleccion>().WithMany().HasForeignKey(x => x.RutaId);
        m.Entity<ReportePasoCamion>().ToTable("reportes_paso").HasKey(x => x.Id);
        m.Entity<ReportePasoCamion>().HasOne<RutaRecoleccion>().WithMany().HasForeignKey(x => x.RutaId);
        m.Entity<ReportePasoCamion>().HasIndex(x => new { x.RutaId, x.UsuarioId, x.ReportadoEn });
        m.Entity<Distrito>().ToTable("distritos").HasKey(x => x.Id);
        m.Entity<PerfilUsuario>().ToTable("perfiles").HasKey(x => x.Id);
        m.Entity<PerfilUsuario>().Property(x => x.Version).IsConcurrencyToken();
        m.Entity<PerfilUsuario>().Property(x => x.AvisosHorarios).HasDefaultValue(true);
        m.Entity<PerfilUsuario>().Property(x => x.AvisosRecojos).HasDefaultValue(true);
        m.Entity<PerfilUsuario>().Property(x => x.AvisosIncidencias).HasDefaultValue(true);
        m.Entity<PerfilUsuario>().Property(x => x.AvisosRecompensas).HasDefaultValue(true);
        m.Entity<PerfilUsuario>().HasOne<AsociacionReciclaje>().WithMany().HasForeignKey(x => x.AsociacionId).OnDelete(DeleteBehavior.Restrict);
        m.Entity<AsociacionReciclaje>().ToTable("asociaciones").HasKey(x => x.Id);
        m.Entity<AsociacionReciclaje>().Property(x => x.CapacidadKg).HasPrecision(12, 2);
        m.Entity<AsociacionReciclaje>().Property(x => x.ReservadoKg).HasPrecision(12, 2);
        m.Entity<AsociacionReciclaje>().Property(x => x.Version).IsConcurrencyToken();
        m.Entity<SolicitudRecojo>().HasOne<AsociacionReciclaje>().WithMany().HasForeignKey(x => x.AsociacionId).OnDelete(DeleteBehavior.Restrict);
        m.Entity<AsociacionReciclaje>().HasOne<Distrito>().WithMany().HasForeignKey(x => x.DistritoId);
        m.Entity<AsociacionReciclaje>().HasMany(x => x.Materiales).WithOne().HasForeignKey(x => x.AsociacionId);
        m.Entity<MaterialAceptado>().ToTable("materiales_aceptados").HasKey(x => new { x.AsociacionId, x.Material });
        m.Entity<MaterialAceptado>().Property(x => x.Material).HasConversion<string>();
        m.Entity<PuntoAcopio>().ToTable("puntos_acopio").HasKey(x => x.Id);
        m.Entity<Contenedor>().ToTable("contenedores").HasKey(x => x.Id);
        m.Entity<Contenedor>().HasOne<Distrito>().WithMany().HasForeignKey(x => x.DistritoId);
        m.Entity<Contenedor>().Property(x => x.Version).IsConcurrencyToken();
        m.Entity<DispositivoIoT>().ToTable("dispositivos_iot").HasKey(x => x.Id);
        m.Entity<DispositivoIoT>().HasOne<Contenedor>().WithMany().HasForeignKey(x => x.ContenedorId);
        m.Entity<DispositivoIoT>().HasIndex(x => x.ContenedorId).IsUnique();
        m.Entity<DispositivoIoT>().Property(x => x.Version).IsConcurrencyToken();
        m.Entity<LecturaNivel>().ToTable("lecturas_nivel").HasKey(x => x.Id);
        m.Entity<LecturaNivel>().HasOne<DispositivoIoT>().WithMany().HasForeignKey(x => x.DispositivoId);
        m.Entity<LecturaNivel>().HasIndex(x => new { x.DispositivoId, x.Secuencia }).IsUnique();
        m.Entity<LecturaNivel>().HasIndex(x => new { x.DispositivoId, x.MedidaEn });
        m.Entity<Incidencia>().ToTable("incidencias").HasKey(x => x.Id);
        m.Entity<Incidencia>().Property(x => x.Estado).HasConversion<string>();
        m.Entity<Incidencia>().Property(x => x.Version).IsConcurrencyToken();
        m.Entity<Incidencia>().HasOne<Contenedor>().WithMany().HasForeignKey(x => x.ContenedorId);
        m.Entity<Incidencia>().HasIndex(x => new { x.ContenedorId, x.Estado });
        m.Entity<Notificacion>().ToTable("notificaciones").HasKey(x => x.Id);
        m.Entity<Notificacion>().Property(x => x.Tipo).HasConversion<string>().HasMaxLength(30).HasDefaultValue(TipoAviso.Sistema);
        m.Entity<Notificacion>().HasIndex(x => new { x.UsuarioId, x.ClaveUnica }).IsUnique();
        m.Entity<Notificacion>().HasIndex(x => new { x.UsuarioId, x.CreadaEn });
        m.Entity<Beneficio>().ToTable("beneficios").HasKey(x => x.Id);
        m.Entity<Beneficio>().Property(x => x.Version).IsConcurrencyToken();
        m.Entity<Canje>().ToTable("canjes").HasKey(x => x.Id);
        m.Entity<Canje>().HasIndex(x => new { x.UsuarioId, x.Clave }).IsUnique();
        m.Entity<Canje>().HasOne<Beneficio>().WithMany().HasForeignKey(x => x.BeneficioId).OnDelete(DeleteBehavior.Restrict);
        m.Entity<HistoricoSolicitud>().ToTable("historial_solicitudes").HasKey(x => x.Id);
        m.Entity<HistoricoSolicitud>().HasOne<SolicitudRecojo>().WithMany().HasForeignKey(x => x.SolicitudId);
        m.Entity<HistoricoSolicitud>().HasIndex(x => new { x.SolicitudId, x.CreadoEn });
        Espacial<Contenedor>(m); Espacial<AsociacionReciclaje>(m); Espacial<PuntoAcopio>(m);
        foreach (var e in m.Model.GetEntityTypes())
            foreach (var p in e.GetProperties().Where(p => p.ClrType == typeof(string) && p.GetMaxLength() == null)) p.SetMaxLength(1500);
    }
    private static void Espacial<T>(ModelBuilder m) where T : class
    {
        m.Entity<T>().Property<Point>("Ubicacion").HasColumnType("geography (point,4326)")
            .HasComputedColumnSql("ST_SetSRID(ST_MakePoint(\"Longitud\", \"Latitud\"),4326)::geography", stored: true);
        m.Entity<T>().HasIndex("Ubicacion").HasMethod("gist");
    }
}
