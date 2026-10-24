using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.UseCases.Rutas;
namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/rutas")]
public sealed class RutasController : ControllerBase
{
    [HttpGet] public async Task<IActionResult> Listar([FromServices] ListarRutasUseCase c, CancellationToken ct) => Ok(await c.ExecuteAsync(ct));
    [HttpPost("{id:guid}/suscripcion")] public async Task<IActionResult> Suscribir(Guid id, SuscribirRutaRequest r, [FromServices] SuscribirRutaUseCase c, CancellationToken ct) { await c.ExecuteAsync(id, r.MinutosAntes, ct); return NoContent(); }
    [HttpDelete("{id:guid}/suscripcion")] public async Task<IActionResult> Cancelar(Guid id, [FromServices] CancelarSuscripcionUseCase c, CancellationToken ct) { await c.ExecuteAsync(id, ct); return NoContent(); }
    [HttpPost("{id:guid}/paso")] public async Task<IActionResult> Paso(Guid id, [FromServices] ReportarPasoUseCase c, CancellationToken ct) { await c.ExecuteAsync(id, ct); return NoContent(); }
}
