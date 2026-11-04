using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Api.Contracts;
using TrackTrash.Application.DTOs.DispositivosIoT;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.DispositivosIoT.Commands.RegistrarDispositivo;
using TrackTrash.Application.UseCases.DispositivosIoT.Commands.RegistrarLectura;

namespace TrackTrash.Api.Controllers;

[ApiController, Route("api/v1/iot")]
public sealed class IoTController(ISender sender) : ControllerBase
{
    [HttpPost("dispositivos"), Authorize(Policy = PoliticasTrackTrash.SoloAdministradores)]
    public async Task<ActionResult<DispositivoRegistradoDto>> RegistrarDispositivo(
        RegistrarDispositivoRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new RegistrarDispositivoCommand
        { ContenedorId = request.ContenedorId, AlturaCm = request.AlturaCm }, ct);
        return Created($"api/v1/iot/dispositivos/{result.Id}", result);
    }

    [HttpPost("lecturas"), AllowAnonymous]
    public async Task<ActionResult<LecturaResultadoDto>> RegistrarLectura(RegistrarLecturaRequest request, CancellationToken ct) =>
        Ok(await sender.Send(new RegistrarLecturaCommand
        {
            DispositivoId = request.DispositivoId, ClaveDispositivo = request.ClaveDispositivo,
            Secuencia = request.Secuencia, DistanciasCm = request.DistanciasCm,
            Bateria = request.Bateria, MedidaEn = request.MedidaEn
        }, ct));
}
