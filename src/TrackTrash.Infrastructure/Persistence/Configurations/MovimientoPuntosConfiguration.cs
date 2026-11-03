using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Infrastructure.Persistence.Configurations;

internal sealed class MovimientoPuntosConfiguration : IEntityTypeConfiguration<MovimientoPuntos>
{
    public void Configure(EntityTypeBuilder<MovimientoPuntos> p)
    {
        p.ToTable("movimientos_puntos");
        p.HasKey(x => x.Id);
        p.HasIndex(x => x.SolicitudId).IsUnique();
        p.HasIndex(x => x.UsuarioId);
        p.HasOne<SolicitudRecojo>().WithMany().HasForeignKey(x => x.SolicitudId).OnDelete(DeleteBehavior.Restrict);
    }
}
