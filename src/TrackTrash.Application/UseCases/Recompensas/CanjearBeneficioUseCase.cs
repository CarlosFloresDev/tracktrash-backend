using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Recompensas;

public sealed class CanjearBeneficioUseCase(IBeneficioRepository beneficios, IPerfilRepository perfiles, IMovimientoPuntosRepository puntos,
    INotificadorUsuarioService avisos, IUsuarioActualService usuario, IUnitOfWork uow, ICodigoEntregaService codigos, TimeProvider reloj)
{
    public async Task<CanjeDto> ExecuteAsync(CanjearRequest r, CancellationToken ct)
    {
        if (usuario.EsAdministrador || usuario.EsReciclador) throw new AccesoDenegadoException();
        var existente = await beneficios.PorClaveAsync(usuario.Id, r.Clave, ct);
        if (existente is not null) { if (existente.BeneficioId != r.BeneficioId) throw new Domain.Exceptions.ReglaNegocioException("La clave ya se utilizó para otro beneficio."); return existente.Dto(); }
        var b = await beneficios.ObtenerAsync(r.BeneficioId, ct) ?? throw new RecursoNoEncontradoException();
        var p = await perfiles.ObtenerAsync(usuario.Id, ct);
        if (p is null) { p = PerfilUsuario.Crear(usuario.Id); perfiles.Agregar(p); }
        b.Reservar(await puntos.ObtenerTotalAsync(usuario.Id, ct)); p.RegistrarOperacionSaldo();
        var c = Canje.Crear(usuario.Id, b, r.Clave, codigos.Generar(), reloj.GetUtcNow()); beneficios.Agregar(c); puntos.Agregar(MovimientoPuntos.PorCanje(c));
        await avisos.AgregarAsync(Notificacion.Crear(usuario.Id, $"canje:{c.Id}", "Canje registrado", $"Tu código de canje para {b.Nombre} está en Mis canjes.", reloj.GetUtcNow(), TipoAviso.Recompensas), ct);
        await uow.GuardarCambiosAsync(ct); return c.Dto();
    }
}
