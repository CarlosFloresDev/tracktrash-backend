using TrackTrash.Application.DTOs.Perfiles;
using MediatR;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Perfiles.Queries.ObtenerPerfil;

internal sealed class ObtenerPerfilQueryHandler(
    IPerfilRepository perfiles,
    IUsuarioActualService usuario,
    TimeProvider reloj) : IRequestHandler<ObtenerPerfilQuery, PerfilDto>
{
    public async Task<PerfilDto> Handle(ObtenerPerfilQuery request, CancellationToken cancellationToken)
    {
        var p = await perfiles.ObtenerAsync(usuario.Id, cancellationToken) ?? PerfilUsuario.Crear(usuario.Id);
        return new(p.Id, p.Nombre, p.Telefono, p.Direccion, p.Latitud, p.Longitud, p.RecibirAvisos, p.AsociacionId, usuario.EsAdministrador ? "admin" : usuario.EsReciclador ? "reciclador" : "vecino", p.RachaVigente(reloj.GetUtcNow()), p.RachaMaxima,
            new(p.AvisosHorarios, p.AvisosRecojos, p.AvisosIncidencias, p.AvisosRecompensas));
    }
}
