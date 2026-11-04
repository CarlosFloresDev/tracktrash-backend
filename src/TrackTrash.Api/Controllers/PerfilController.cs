using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Api.Contracts;
using TrackTrash.Application.DTOs.Perfiles;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.MovimientosPuntos.Queries.ObtenerImpacto;
using TrackTrash.Application.UseCases.Perfiles.Commands.ActualizarPerfil;
using TrackTrash.Application.UseCases.Perfiles.Queries.ObtenerPerfil;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/perfil")]
public sealed class PerfilController(ISender sender) : ControllerBase
{
    [HttpGet]
    public IActionResult Obtener([FromServices] IUsuarioActualService usuario) =>
        Ok(new { id = usuario.Id, rol = usuario.Rol.ComoClaim() });

    [HttpGet("impacto")]
    public async Task<IActionResult> Impacto(CancellationToken ct) =>
        Ok(await sender.Send(new ObtenerImpactoQuery(), ct));

    [HttpGet("detalle")]
    public async Task<ActionResult<PerfilDto>> ObtenerDetalle(CancellationToken ct) =>
        Ok(await sender.Send(new ObtenerPerfilQuery(), ct));

    [HttpPut]
    public async Task<IActionResult> Actualizar(ActualizarPerfilRequest request, CancellationToken ct)
    {
        await sender.Send(new ActualizarPerfilCommand
        {
            Nombre = request.Nombre,
            Telefono = request.Telefono,
            Direccion = request.Direccion,
            Latitud = request.Latitud,
            Longitud = request.Longitud,
            RecibirAvisos = request.RecibirAvisos,
            PreferenciasAvisos = request.PreferenciasAvisos
        }, ct);
        return NoContent();
    }
}
