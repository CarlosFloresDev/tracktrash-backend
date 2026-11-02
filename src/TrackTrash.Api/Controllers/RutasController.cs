using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.UseCases.Rutas.Commands;
using TrackTrash.Application.UseCases.Rutas.Queries;
namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/rutas")]
public sealed class RutasController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken ct) => Ok(await sender.Send(new ListarRutasQuery(), ct));

    [HttpPost("{id:guid}/suscripcion")]
    public async Task<IActionResult> Suscribir(Guid id, SuscribirRutaRequest r, CancellationToken ct)
    { await sender.Send(new SuscribirRutaCommand { RutaId = id, MinutosAntes = r.MinutosAntes }, ct); return NoContent(); }

    [HttpDelete("{id:guid}/suscripcion")]
    public async Task<IActionResult> Cancelar(Guid id, CancellationToken ct)
    { await sender.Send(new CancelarSuscripcionCommand { RutaId = id }, ct); return NoContent(); }

    [HttpPost("{id:guid}/paso")]
    public async Task<IActionResult> Paso(Guid id, CancellationToken ct)
    { await sender.Send(new ReportarPasoCommand { RutaId = id }, ct); return NoContent(); }
}
