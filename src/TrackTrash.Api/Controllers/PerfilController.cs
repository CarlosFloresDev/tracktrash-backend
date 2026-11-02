using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.UseCases.Perfiles.Commands;
using TrackTrash.Application.UseCases.Perfiles.Queries;
using TrackTrash.Application.UseCases.Recompensas.Queries;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/perfil")]
public sealed class PerfilController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Obtener(CancellationToken ct) => Ok(await sender.Send(new ObtenerPerfilQuery(), ct));

    [HttpPut]
    public async Task<IActionResult> Actualizar(ActualizarPerfilRequest r, CancellationToken ct)
    {
        await sender.Send(new ActualizarPerfilCommand
        {
            Nombre = r.Nombre,
            Telefono = r.Telefono,
            Direccion = r.Direccion,
            Latitud = r.Latitud,
            Longitud = r.Longitud,
            RecibirAvisos = r.RecibirAvisos,
            PreferenciasAvisos = r.PreferenciasAvisos
        }, ct);
        return NoContent();
    }

    [HttpGet("impacto")]
    public async Task<IActionResult> Impacto(CancellationToken ct) => Ok(await sender.Send(new ObtenerImpactoQuery(), ct));

    [HttpGet("movimientos")]
    public async Task<IActionResult> Movimientos(CancellationToken ct) => Ok(await sender.Send(new ListarMovimientosQuery(), ct));
}
