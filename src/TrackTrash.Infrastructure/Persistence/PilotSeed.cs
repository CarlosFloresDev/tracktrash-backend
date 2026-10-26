using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Infrastructure.Persistence;

public static class PilotSeed
{
    public static async Task InicializarAsync(TrackTrashDbContext db, IConfiguration config, CancellationToken ct = default)
    {
        if (!config.GetValue<bool>("Pilot:Enabled"))
            throw new InvalidOperationException("La carga del piloto requiere Pilot:Enabled explícito.");
        var actores = new[] { "Vecino", "Reciclador", "Admin" }.Select(rol =>
        {
            if (!Guid.TryParse(config[$"Pilot:{rol}Id"], out var id) || id == Guid.Empty)
                throw new InvalidOperationException($"Configura Pilot:{rol}Id con una identidad de Auth del proyecto de pruebas.");
            return (Rol: rol, Id: id);
        }).ToArray();
        if (actores.Select(a => a.Id).Distinct().Count() != actores.Length)
            throw new InvalidOperationException("Los actores de prueba deben tener identidades distintas.");

        await DemoSeed.InicializarAsync(db, ct);
        foreach (var actor in actores)
        {
            var perfil = await db.Perfiles.SingleOrDefaultAsync(p => p.Id == actor.Id, ct);
            if (perfil is null)
            {
                perfil = PerfilUsuario.Crear(actor.Id);
                perfil.Actualizar($"{actor.Rol} de prueba", "", "Referencia ficticia del piloto, Arequipa", -16.3988, -71.5369, true);
                db.Perfiles.Add(perfil);
            }
            if (actor.Rol == "Reciclador" && perfil.AsociacionId is null)
                perfil.VincularAsociacion(DemoSeed.AsociacionPrincipal);
        }
        await db.SaveChangesAsync(ct);
    }
}
