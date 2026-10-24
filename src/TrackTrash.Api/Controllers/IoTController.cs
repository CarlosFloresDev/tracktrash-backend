using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.UseCases.Contenedores;
namespace TrackTrash.Api.Controllers;

[ApiController, Route("api/v1/iot")]
public sealed class IoTController : ControllerBase
{
    [HttpPost("dispositivos"), Authorize(Roles = "admin")]
    public async Task<IActionResult> Registrar(RegistrarDispositivoRequest r, [FromServices] RegistrarDispositivoUseCase c, CancellationToken ct) => Ok(await c.ExecuteAsync(r, ct));
    [HttpPost("dispositivos/{id:guid}/lecturas"), AllowAnonymous]
    public async Task<IActionResult> Lectura(Guid id, LecturaRequest r, [FromServices] RegistrarLecturaUseCase c, CancellationToken ct) => Ok(await c.ExecuteAsync(id, Request.Headers["X-Device-Key"].ToString(), r, ct));
}
