using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.UseCases.Entregas;
using TrackTrash.Application.UseCases.SolicitudesRecojo;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/solicitudes")]
public sealed class SolicitudesRecojoController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SolicitudDto>>> Listar(
        [FromServices] ListarSolicitudesRecojoUseCase useCase, CancellationToken ct, [FromQuery] int pagina = 1) =>
        Ok(await useCase.ExecuteAsync(pagina, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SolicitudDto>> Obtener(Guid id,
        [FromServices] ObtenerSolicitudRecojoUseCase useCase, CancellationToken ct) => Ok(await useCase.ExecuteAsync(id, ct));

    [HttpPost, Authorize(Roles = "vecino")]
    public async Task<ActionResult<SolicitudDto>> Crear(CrearSolicitudRequest request,
        [FromServices] CrearSolicitudRecojoUseCase useCase, CancellationToken ct)
    {
        var result = await useCase.ExecuteAsync(request, ct);
        return CreatedAtAction(nameof(Obtener), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/aceptacion"), Authorize(Roles = "reciclador")]
    public async Task<ActionResult<SolicitudDto>> Aceptar(Guid id,
        [FromServices] AceptarSolicitudRecojoUseCase useCase, CancellationToken ct) => Ok(await useCase.ExecuteAsync(id, ct));

    [HttpPost("{id:guid}/confirmacion"), Authorize(Roles = "reciclador")]
    public async Task<ActionResult<SolicitudDto>> Confirmar(Guid id, ConfirmarEntregaRequest request,
        [FromServices] ConfirmarEntregaUseCase useCase, CancellationToken ct) => Ok(await useCase.ExecuteAsync(id, request, ct));

    [HttpPost("{id:guid}/viaje"), Authorize(Roles = "reciclador")]
    public async Task<IActionResult> Viaje(Guid id, [FromServices] IniciarViajeUseCase c, CancellationToken ct) => Ok(await c.ExecuteAsync(id, ct));
    [HttpPost("{id:guid}/cancelacion")]
    public async Task<IActionResult> Cancelar(Guid id, CancelarSolicitudRequest r, [FromServices] CancelarSolicitudUseCase c, CancellationToken ct) => Ok(await c.ExecuteAsync(id, r.Motivo, ct));
    [HttpPost("{id:guid}/reprogramacion"), Authorize(Roles = "vecino")]
    public async Task<IActionResult> Reprogramar(Guid id, ReprogramarSolicitudRequest r, [FromServices] ReprogramarSolicitudUseCase c, CancellationToken ct) => Ok(await c.ExecuteAsync(id, r.ProgramadaPara, ct));
    [HttpPost("{id:guid}/asignacion"), Authorize(Roles = "vecino")]
    public async Task<IActionResult> Asignar(Guid id, [FromServices] AsignarSolicitudUseCase c, CancellationToken ct) => Ok(await c.ExecuteAsync(id, ct));
    [HttpGet("{id:guid}/historial")]
    public async Task<IActionResult> Historial(Guid id, [FromServices] ConsultarHistorialUseCase c, CancellationToken ct) => Ok(await c.ExecuteAsync(id, ct));
}
