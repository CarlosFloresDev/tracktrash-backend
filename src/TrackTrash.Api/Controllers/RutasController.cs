using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Api.Contracts;
using TrackTrash.Application.DTOs.Rutas;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.Rutas.Commands.CancelarSuscripcion;
using TrackTrash.Application.UseCases.Rutas.Commands.ReportarPaso;
using TrackTrash.Application.UseCases.Rutas.Commands.SuscribirRuta;
using TrackTrash.Application.UseCases.Rutas.Queries.ListarRutas;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/rutas")]
public sealed class RutasController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RutaDto>>> Listar(CancellationToken ct) =>
        Ok(await sender.Send(new ListarRutasQuery(), ct));

    [HttpPost("{id:guid}/suscripcion")]
    public async Task<IActionResult> Suscribir(Guid id, SuscribirRutaRequest request, CancellationToken ct)
    {
        await sender.Send(new SuscribirRutaCommand { RutaId = id, MinutosAntes = request.MinutosAntes }, ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}/suscripcion")]
    public async Task<IActionResult> CancelarSuscripcion(Guid id, CancellationToken ct)
    {
        await sender.Send(new CancelarSuscripcionCommand { RutaId = id }, ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/pasos"), Authorize(Policy = PoliticasTrackTrash.Operadores)]
    public async Task<IActionResult> ReportarPaso(Guid id, CancellationToken ct)
    {
        await sender.Send(new ReportarPasoCommand { RutaId = id }, ct);
        return NoContent();
    }
}
