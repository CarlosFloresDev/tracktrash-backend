using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Api.Contracts;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.AceptarSolicitudRecojo;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.AsignarSolicitud;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.CancelarSolicitud;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.ConfirmarEntrega;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.CrearSolicitudRecojo;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.IniciarViaje;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.ReprogramarSolicitud;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Queries.ConsultarHistorial;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Queries.ListarSolicitudesRecojo;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Queries.ObtenerSolicitudRecojo;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/solicitudes")]
public sealed class SolicitudesRecojoController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SolicitudDto>>> Listar(CancellationToken ct,
        [FromQuery] int pagina = 1) => Ok(await sender.Send(new ListarSolicitudesRecojoQuery { Pagina = pagina }, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SolicitudDto>> Obtener(Guid id, CancellationToken ct) =>
        Ok(await sender.Send(new ObtenerSolicitudRecojoQuery { SolicitudId = id }, ct));

    [HttpPost, Authorize(Policy = PoliticasTrackTrash.SoloVecinos)]
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

    [HttpPost("{id:guid}/aceptacion"), Authorize(Policy = PoliticasTrackTrash.SoloRecicladores)]
    public async Task<ActionResult<SolicitudDto>> Aceptar(Guid id, CancellationToken ct) =>
        Ok(await sender.Send(new AceptarSolicitudRecojoCommand { SolicitudId = id }, ct));

    [HttpPost("{id:guid}/confirmacion"), Authorize(Policy = PoliticasTrackTrash.SoloRecicladores)]
    public async Task<ActionResult<SolicitudDto>> Confirmar(Guid id, ConfirmarEntregaRequest request, CancellationToken ct) =>
        Ok(await sender.Send(new ConfirmarEntregaCommand { SolicitudId = id, Codigo = request.Codigo }, ct));

    [HttpPost("{id:guid}/asignacion"), Authorize(Policy = PoliticasTrackTrash.SoloVecinos)]
    public async Task<ActionResult<SolicitudDto>> Asignar(Guid id, CancellationToken ct) =>
        Ok(await sender.Send(new AsignarSolicitudCommand { SolicitudId = id }, ct));

    [HttpPost("{id:guid}/viaje"), Authorize(Policy = PoliticasTrackTrash.SoloRecicladores)]
    public async Task<ActionResult<SolicitudDto>> IniciarViaje(Guid id, CancellationToken ct) =>
        Ok(await sender.Send(new IniciarViajeCommand { SolicitudId = id }, ct));

    [HttpPost("{id:guid}/cancelacion")]
    public async Task<ActionResult<SolicitudDto>> Cancelar(Guid id, CancelarSolicitudRequest request, CancellationToken ct) =>
        Ok(await sender.Send(new CancelarSolicitudCommand { SolicitudId = id, Motivo = request.Motivo }, ct));

    [HttpPost("{id:guid}/reprogramacion"), Authorize(Policy = PoliticasTrackTrash.SoloVecinos)]
    public async Task<ActionResult<SolicitudDto>> Reprogramar(Guid id, ReprogramarSolicitudRequest request, CancellationToken ct) =>
        Ok(await sender.Send(new ReprogramarSolicitudCommand { SolicitudId = id, ProgramadaPara = request.ProgramadaPara }, ct));

    [HttpGet("{id:guid}/historial")]
    public async Task<ActionResult<IReadOnlyList<HistoricoDto>>> Historial(Guid id, CancellationToken ct) =>
        Ok(await sender.Send(new ConsultarHistorialQuery { SolicitudId = id }, ct));
}
