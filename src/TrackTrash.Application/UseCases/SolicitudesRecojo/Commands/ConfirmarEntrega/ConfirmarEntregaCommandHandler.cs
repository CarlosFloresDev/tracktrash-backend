using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.ConfirmarEntrega;

internal sealed class ConfirmarEntregaCommandHandler(
    ISolicitudRecojoRepository solicitudes,
    IMovimientoPuntosRepository puntos,
    IUnitOfWork unitOfWork,
    IUsuarioActualService usuario,
    TimeProvider reloj,
    ICatalogoRepository catalogo,
    IHistoricoRepository historial,
    INotificadorUsuarioService avisos,
    IPerfilRepository perfiles) : IRequestHandler<ConfirmarEntregaCommand, SolicitudDto>
{
    public async Task<SolicitudDto> Handle(ConfirmarEntregaCommand request, CancellationToken cancellationToken)
    {
        if (!usuario.EsReciclador) throw new AccesoDenegadoException();
        var s = await solicitudes.ObtenerAsync(request.SolicitudId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        if (s.RecicladorId != usuario.Id) throw new AccesoDenegadoException();
        var ahora = reloj.GetUtcNow();
        if (s.Confirmar(usuario.Id, request.Codigo, ahora))
        {
            var fecha = ahora.ToOffset(TimeSpan.FromHours(-5));
            var inicio = new DateTimeOffset(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0, TimeSpan.FromHours(-5)).ToUniversalTime();
            var perfil = await perfiles.ObtenerAsync(s.VecinoId, cancellationToken);
            if (perfil is null) { perfil = PerfilUsuario.Crear(s.VecinoId); perfiles.Agregar(perfil); }
            // La versión del perfil se lee antes del saldo para proteger el límite diario concurrente.
            var premiar = PoliticaRecompensas.PuedeOtorgar(await puntos.OtorgadosEntreAsync(s.VecinoId, inicio, inicio.AddDays(1), cancellationToken));
            perfil.RegistrarEntrega(ahora);
            if (premiar) puntos.Agregar(MovimientoPuntos.PorEntrega(s, ahora));
            if (s.AsociacionId is Guid asociacion) { var a = await catalogo.AsociacionAsync(asociacion, cancellationToken); a?.Liberar(s.CantidadEstimadaKg); }
            historial.Agregar(HistoricoSolicitud.Crear(s.Id, usuario.Id, premiar ? "Entrega confirmada · 20 puntos" : "Entrega confirmada · límite diario de puntos alcanzado", ahora));
            await avisos.AgregarAsync(Notificacion.Crear(s.VecinoId, $"confirmada:{s.Id}", "¡Una nueva entrega lograda!", premiar ? "Tu entrega fue confirmada y recibiste 20 puntos." : "Tu entrega fue confirmada. Ya alcanzaste los puntos disponibles por hoy.", ahora, TipoAviso.Recojos), cancellationToken);
            await unitOfWork.GuardarCambiosAsync(cancellationToken);
        }
        return SolicitudDto.Desde(s, usuario.Id);
    }
}
