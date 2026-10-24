using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Infrastructure.Persistence.Repositories;
public sealed class BeneficioRepository(TrackTrashDbContext db):IBeneficioRepository
{
    public async Task<IReadOnlyList<Beneficio>> ListarAsync(CancellationToken ct)=>await db.Beneficios.AsNoTracking().Where(x=>x.Activo).OrderBy(x=>x.CostoPuntos).ToListAsync(ct);
    public Task<Beneficio?> ObtenerAsync(Guid id,CancellationToken ct)=>db.Beneficios.SingleOrDefaultAsync(x=>x.Id==id,ct);
    public Task<Canje?> PorClaveAsync(Guid usuario,Guid clave,CancellationToken ct)=>db.Canjes.SingleOrDefaultAsync(x=>x.UsuarioId==usuario&&x.Clave==clave,ct);
    public async Task<IReadOnlyList<Canje>> CanjesAsync(Guid usuario,CancellationToken ct)=>await db.Canjes.AsNoTracking().Where(x=>x.UsuarioId==usuario).OrderByDescending(x=>x.CreadoEn).Take(100).ToListAsync(ct);
    public void Agregar(Beneficio b)=>db.Beneficios.Add(b);
    public void Agregar(Canje c)=>db.Canjes.Add(c);
}
