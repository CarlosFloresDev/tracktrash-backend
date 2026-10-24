using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Infrastructure.Persistence.Repositories;
public sealed class PerfilRepository(TrackTrashDbContext db):IPerfilRepository
{
    public Task<PerfilUsuario?> ObtenerAsync(Guid id,CancellationToken ct)=>db.Perfiles.SingleOrDefaultAsync(x=>x.Id==id,ct);
    public void Agregar(PerfilUsuario p)=>db.Perfiles.Add(p);
}
