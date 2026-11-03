using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Infrastructure.Persistence.Configurations;

internal sealed class SolicitudRecojoConfiguration : IEntityTypeConfiguration<SolicitudRecojo>
{
    public void Configure(EntityTypeBuilder<SolicitudRecojo> s)
    {
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
    }
}
