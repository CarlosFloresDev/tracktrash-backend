using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.UseCases.Entregas.Commands;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Commands;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Queries;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/solicitudes")]
public sealed class SolicitudesRecojoController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SolicitudDto>>> Listar(CancellationToken ct, [FromQuery] int pagina = 1) =>
        Ok(await sender.Send(new ListarSolicitudesRecojoQuery { Pagina = pagina }, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SolicitudDto>> Obtener(Guid id, CancellationToken ct) =>
        Ok(await sender.Send(new ObtenerSolicitudRecojoQuery { SolicitudId = id }, ct));

    [HttpPost, Authorize(Roles = "vecino")]
    public async Task<ActionResult<SolicitudDto>> Crear(CrearSolicitudRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new CrearSolicitudRecojoCommand
        {
            ClaveSolicitud = request.ClaveSolicitud,
            Material = request.Material,
            CantidadEstimadaKg = request.CantidadEstimadaKg,
            Direccion = request.Direccion,
            ProgramadaPara = request.ProgramadaPara,
            Latitud = request.Latitud,
            Longitud = request.Longitud,
            AutoAsignar = request.AutoAsignar
        }, ct);
        return CreatedAtAction(nameof(Obtener), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/aceptacion"), Authorize(Roles = "reciclador")]
    public async Task<ActionResult<SolicitudDto>> Aceptar(Guid id, CancellationToken ct) =>
        Ok(await sender.Send(new AceptarSolicitudRecojoCommand { SolicitudId = id }, ct));

    [HttpPost("{id:guid}/confirmacion"), Authorize(Roles = "reciclador")]
    public async Task<ActionResult<SolicitudDto>> Confirmar(Guid id, ConfirmarEntregaRequest request, CancellationToken ct) =>
        Ok(await sender.Send(new ConfirmarEntregaCommand { SolicitudId = id, Codigo = request.Codigo }, ct));

    [HttpPost("{id:guid}/viaje"), Authorize(Roles = "reciclador")]
    public async Task<IActionResult> Viaje(Guid id, CancellationToken ct) =>
        Ok(await sender.Send(new IniciarViajeCommand { SolicitudId = id }, ct));

    [HttpPost("{id:guid}/cancelacion")]
    public async Task<IActionResult> Cancelar(Guid id, CancelarSolicitudRequest r, CancellationToken ct) =>
        Ok(await sender.Send(new CancelarSolicitudCommand { SolicitudId = id, Motivo = r.Motivo }, ct));

    [HttpPost("{id:guid}/reprogramacion"), Authorize(Roles = "vecino")]
    public async Task<IActionResult> Reprogramar(Guid id, ReprogramarSolicitudRequest r, CancellationToken ct) =>
        Ok(await sender.Send(new ReprogramarSolicitudCommand { SolicitudId = id, ProgramadaPara = r.ProgramadaPara }, ct));

    [HttpPost("{id:guid}/asignacion"), Authorize(Roles = "vecino")]
    public async Task<IActionResult> Asignar(Guid id, CancellationToken ct) =>
        Ok(await sender.Send(new AsignarSolicitudCommand { SolicitudId = id }, ct));

    [HttpGet("{id:guid}/historial")]
    public async Task<IActionResult> Historial(Guid id, CancellationToken ct) =>
        Ok(await sender.Send(new ConsultarHistorialQuery { SolicitudId = id }, ct));
}
