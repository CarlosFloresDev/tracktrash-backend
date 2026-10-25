using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Entregas;

public sealed class ConfirmarEntregaUseCase(ISolicitudRecojoRepository solicitudes,
    IMovimientoPuntosRepository puntos, IUnitOfWork uow, IUsuarioActualService usuario, TimeProvider reloj, ICatalogoRepository catalogo, IHistoricoRepository historial, INotificadorUsuarioService avisos, IPerfilRepository perfiles)
{
    public async Task<SolicitudDto> ExecuteAsync(Guid id, ConfirmarEntregaRequest request, CancellationToken ct)
    {
        if (!usuario.EsReciclador) throw new AccesoDenegadoException();
        var s = await solicitudes.ObtenerAsync(id, ct) ?? throw new RecursoNoEncontradoException();
        if (s.RecicladorId != usuario.Id) throw new AccesoDenegadoException();
        var ahora = reloj.GetUtcNow();
        if (s.Confirmar(usuario.Id, request.Codigo, ahora))
        {
            var fecha = ahora.ToOffset(TimeSpan.FromHours(-5));
            var inicio = new DateTimeOffset(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0, TimeSpan.FromHours(-5)).ToUniversalTime();
            var perfil = await perfiles.ObtenerAsync(s.VecinoId, ct);
            if (perfil is null) { perfil = PerfilUsuario.Crear(s.VecinoId); perfiles.Agregar(perfil); }
            // La versión del perfil se lee antes del saldo para proteger el límite diario concurrente.
            var premiar = PoliticaRecompensas.PuedeOtorgar(await puntos.OtorgadosEntreAsync(s.VecinoId, inicio, inicio.AddDays(1), ct));
            perfil.RegistrarEntrega(ahora);
            if (premiar) puntos.Agregar(MovimientoPuntos.PorEntrega(s, ahora));
            if (s.AsociacionId is Guid asociacion) { var a = await catalogo.AsociacionAsync(asociacion, ct); a?.Liberar(s.CantidadEstimadaKg); }
            historial.Agregar(HistoricoSolicitud.Crear(s.Id, usuario.Id, premiar ? "Entrega confirmada · 20 puntos" : "Entrega confirmada · límite diario de puntos alcanzado", ahora));
            await avisos.AgregarAsync(Notificacion.Crear(s.VecinoId, $"confirmada:{s.Id}", "¡Una nueva entrega lograda!", premiar ? "Tu entrega fue confirmada y recibiste 20 puntos." : "Tu entrega fue confirmada. Ya alcanzaste los puntos disponibles por hoy.", ahora, TipoAviso.Recojos), ct);
            await uow.GuardarCambiosAsync(ct);
        }
        return SolicitudDto.Desde(s, usuario.Id);
    }
}
