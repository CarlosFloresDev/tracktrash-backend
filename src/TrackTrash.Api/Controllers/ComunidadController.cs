using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.UseCases.Notificaciones;
using TrackTrash.Application.UseCases.Recompensas;
using TrackTrash.Application.UseCases.Operacion;
namespace TrackTrash.Api.Controllers;
[ApiController,Authorize,Route("api/v1")]
public sealed class ComunidadController:ControllerBase
{
    [HttpGet("notificaciones")]public async Task<IActionResult> Avisos([FromServices] ListarNotificacionesUseCase c,CancellationToken ct)=>Ok(await c.ExecuteAsync(ct));
    [HttpPost("notificaciones/{id:guid}/lectura")]public async Task<IActionResult> Leer(Guid id,[FromServices] LeerNotificacionUseCase c,CancellationToken ct){await c.ExecuteAsync(id,ct);return NoContent();}
    [HttpGet("beneficios")]public async Task<IActionResult> Beneficios([FromServices] ListarBeneficiosUseCase c,CancellationToken ct)=>Ok(await c.ExecuteAsync(ct));
    [HttpPost("beneficios"),Authorize(Roles="admin")]public async Task<IActionResult> Crear(CrearBeneficioRequest r,[FromServices] CrearBeneficioUseCase c,CancellationToken ct)=>Ok(new{id=await c.ExecuteAsync(r,ct)});
    [HttpGet("canjes")]public async Task<IActionResult> Canjes([FromServices] ListarCanjesUseCase c,CancellationToken ct)=>Ok(await c.ExecuteAsync(ct));
    [HttpPost("canjes"),Authorize(Roles="vecino")]public async Task<IActionResult> Canjear(CanjearRequest r,[FromServices] CanjearBeneficioUseCase c,CancellationToken ct)=>Ok(await c.ExecuteAsync(r,ct));
    [HttpGet("operacion/indicadores"),Authorize(Roles="admin")]public async Task<IActionResult> Indicadores([FromServices] ObtenerIndicadoresUseCase c,CancellationToken ct)=>Ok(await c.ExecuteAsync(ct));
}
