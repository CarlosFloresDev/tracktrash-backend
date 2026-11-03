using Microsoft.EntityFrameworkCore;
using Npgsql;
using TrackTrash.Application.Exceptions;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Infrastructure.Persistence;

namespace TrackTrash.Infrastructure.Adapters;

public sealed class UnitOfWork(TrackTrashDbContext db) : IUnitOfWork
{
    public async Task GuardarCambiosAsync(CancellationToken ct)
    {
        try { await db.SaveChangesAsync(ct); }
        catch (DbUpdateConcurrencyException) { throw new ConflictoPersistenciaException(); }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        { throw new ConflictoPersistenciaException(); }
    }
}
