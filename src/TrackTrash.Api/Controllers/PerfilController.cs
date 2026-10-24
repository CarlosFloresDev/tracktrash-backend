using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.Recompensas;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/perfil")]
public sealed class PerfilController : ControllerBase
{
    [HttpGet]
    public IActionResult Obtener([FromServices] IUsuarioActualService usuario) =>
        Ok(new { id = usuario.Id, rol = usuario.EsReciclador ? "reciclador" : "vecino" });

    [HttpGet("impacto")]
    public async Task<IActionResult> Impacto([FromServices] ObtenerImpactoUseCase useCase, CancellationToken ct) =>
        Ok(await useCase.ExecuteAsync(ct));
}
