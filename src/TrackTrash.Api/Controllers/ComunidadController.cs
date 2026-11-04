using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.DTOs.Indicadores;
using TrackTrash.Application.DTOs.MovimientosPuntos;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.Indicadores.Queries.ObtenerIndicadores;
using TrackTrash.Application.UseCases.MovimientosPuntos.Queries.ListarMovimientos;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/comunidad")]
public sealed class ComunidadController(ISender sender) : ControllerBase
{
    [HttpGet("movimientos")]
    public async Task<ActionResult<IReadOnlyList<MovimientoDto>>> Movimientos(CancellationToken ct) =>
        Ok(await sender.Send(new ListarMovimientosQuery(), ct));

    [HttpGet("indicadores"), Authorize(Policy = PoliticasTrackTrash.SoloAdministradores)]
    public async Task<ActionResult<IndicadoresDto>> Indicadores(CancellationToken ct) =>
        Ok(await sender.Send(new ObtenerIndicadoresQuery(), ct));
}
