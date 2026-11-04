using MediatR;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.DTOs.Asociaciones;
using TrackTrash.Application.DTOs.Canjes;
using TrackTrash.Application.DTOs.Contenedores;
using TrackTrash.Application.DTOs.Incidencias;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Canjes.Commands.CanjearBeneficio;

internal sealed class CanjearBeneficioCommandHandler(
    IBeneficioRepository beneficios,
    IPerfilRepository perfiles,
    IMovimientoPuntosRepository puntos,
    INotificadorUsuarioService avisos,
    IUsuarioActualService usuario,
    IUnitOfWork unitOfWork,
    ICodigoEntregaService codigos,
    TimeProvider reloj) : IRequestHandler<CanjearBeneficioCommand, CanjeDto>
{
    public async Task<CanjeDto> Handle(CanjearBeneficioCommand request, CancellationToken cancellationToken)
    {
        if (usuario.EsAdministrador || usuario.EsReciclador) throw new AccesoDenegadoException();
        var existente = await beneficios.PorClaveAsync(usuario.Id, request.Clave, cancellationToken);
        if (existente is not null) { if (existente.BeneficioId != request.BeneficioId) throw new Domain.Exceptions.ReglaNegocioException("La clave ya se utilizó para otro beneficio."); return existente.ToDto(); }
        var b = await beneficios.ObtenerAsync(request.BeneficioId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        var p = await perfiles.ObtenerAsync(usuario.Id, cancellationToken);
        if (p is null) { p = PerfilUsuario.Crear(usuario.Id); perfiles.Agregar(p); }
        b.Reservar(await puntos.ObtenerTotalAsync(usuario.Id, cancellationToken)); p.RegistrarOperacionSaldo();
        var c = Canje.Crear(usuario.Id, b, request.Clave, codigos.Generar(), reloj.GetUtcNow()); beneficios.Agregar(c); puntos.Agregar(MovimientoPuntos.PorCanje(c));
        await avisos.AgregarAsync(Notificacion.Crear(usuario.Id, $"canje:{c.Id}", "Canje registrado", $"Tu código de canje para {b.Nombre} está en Mis canjes.", reloj.GetUtcNow(), TipoAviso.Recompensas), cancellationToken);
        await unitOfWork.GuardarCambiosAsync(cancellationToken); return c.ToDto();
    }
}
