using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.Recompensas;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.UseCases.Perfiles;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/perfil")]
public sealed class PerfilController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Obtener([FromServices] ObtenerPerfilUseCase caso,CancellationToken ct) => Ok(await caso.ExecuteAsync(ct));

    [HttpPut]
    public async Task<IActionResult> Actualizar(ActualizarPerfilRequest r,[FromServices] ActualizarPerfilUseCase caso,CancellationToken ct)
    {await caso.ExecuteAsync(r,ct);return NoContent();}

    [HttpGet("impacto")]
    public async Task<IActionResult> Impacto([FromServices] ObtenerImpactoUseCase useCase, CancellationToken ct) =>
        Ok(await useCase.ExecuteAsync(ct));
}
