using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Application.UseCases.Perfiles;

public sealed class ObtenerPerfilUseCase(IPerfilRepository perfiles, IUsuarioActualService usuario, TimeProvider reloj)
{
    public async Task<PerfilDto> ExecuteAsync(CancellationToken ct)
    {
        var p = await perfiles.ObtenerAsync(usuario.Id, ct) ?? PerfilUsuario.Crear(usuario.Id);
        return new(p.Id, p.Nombre, p.Telefono, p.Direccion, p.Latitud, p.Longitud, p.RecibirAvisos, p.AsociacionId, usuario.EsAdministrador ? "admin" : usuario.EsReciclador ? "reciclador" : "vecino", p.RachaVigente(reloj.GetUtcNow()), p.RachaMaxima);
    }
}
