using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.UseCases.Notificaciones.Commands;
using TrackTrash.Application.UseCases.Notificaciones.Queries;
using TrackTrash.Application.UseCases.Operacion.Queries;
using TrackTrash.Application.UseCases.Recompensas.Commands;
using TrackTrash.Application.UseCases.Recompensas.Queries;
namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1")]
public sealed class ComunidadController(ISender sender) : ControllerBase
{
    [HttpGet("notificaciones")]
    public async Task<IActionResult> Avisos(CancellationToken ct) => Ok(await sender.Send(new ListarNotificacionesQuery(), ct));

    [HttpPost("notificaciones/{id:guid}/lectura")]
    public async Task<IActionResult> Leer(Guid id, CancellationToken ct)
    { await sender.Send(new LeerNotificacionCommand { NotificacionId = id }, ct); return NoContent(); }

    [HttpGet("beneficios")]
    public async Task<IActionResult> Beneficios(CancellationToken ct) => Ok(await sender.Send(new ListarBeneficiosQuery(), ct));

    [HttpPost("beneficios"), Authorize(Roles = "admin")]
    public async Task<IActionResult> Crear(CrearBeneficioRequest r, CancellationToken ct) =>
        Ok(new { id = await sender.Send(new CrearBeneficioCommand { Nombre = r.Nombre, Descripcion = r.Descripcion, CostoPuntos = r.CostoPuntos, Stock = r.Stock }, ct) });

    [HttpGet("canjes")]
    public async Task<IActionResult> Canjes(CancellationToken ct) => Ok(await sender.Send(new ListarCanjesQuery(), ct));

    [HttpPost("canjes"), Authorize(Roles = "vecino")]
    public async Task<IActionResult> Canjear(CanjearRequest r, CancellationToken ct) =>
        Ok(await sender.Send(new CanjearBeneficioCommand { BeneficioId = r.BeneficioId, Clave = r.Clave }, ct));

    [HttpGet("operacion/indicadores"), Authorize(Roles = "admin")]
    public async Task<IActionResult> Indicadores(CancellationToken ct) => Ok(await sender.Send(new ObtenerIndicadoresQuery(), ct));
}
