using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.UseCases.Contenedores.Commands;
namespace TrackTrash.Api.Controllers;

[ApiController, Route("api/v1/iot")]
public sealed class IoTController(ISender sender) : ControllerBase
{
    [HttpPost("dispositivos"), Authorize(Roles = "admin")]
    public async Task<IActionResult> Registrar(RegistrarDispositivoRequest r, CancellationToken ct) =>
        Ok(await sender.Send(new RegistrarDispositivoCommand { ContenedorId = r.ContenedorId, AlturaCm = r.AlturaCm }, ct));

    [HttpPost("dispositivos/{id:guid}/lecturas"), AllowAnonymous]
    public async Task<IActionResult> Lectura(Guid id, LecturaRequest r, CancellationToken ct) =>
        Ok(await sender.Send(new RegistrarLecturaCommand
        {
            DispositivoId = id,
            ClaveDispositivo = Request.Headers["X-Device-Key"].ToString(),
            Secuencia = r.Secuencia,
            DistanciasCm = r.DistanciasCm,
            Bateria = r.Bateria,
            MedidaEn = r.MedidaEn
        }, ct));
}
