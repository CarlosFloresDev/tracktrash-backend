using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.MovimientosPuntos.Queries.ObtenerImpacto;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/perfil")]
public sealed class PerfilController(ISender sender) : ControllerBase
{
    [HttpGet]
    public IActionResult Obtener([FromServices] IUsuarioActualService usuario) =>
        Ok(new { id = usuario.Id, rol = usuario.EsAdministrador ? "administrador" : usuario.EsReciclador ? "reciclador" : "vecino" });

    [HttpGet("impacto")]
    public async Task<IActionResult> Impacto(CancellationToken ct) =>
        Ok(await sender.Send(new ObtenerImpactoQuery(), ct));
}
