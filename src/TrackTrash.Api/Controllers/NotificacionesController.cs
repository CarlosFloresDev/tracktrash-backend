using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.DTOs.Notificaciones;
using TrackTrash.Application.UseCases.Notificaciones.Commands.LeerNotificacion;
using TrackTrash.Application.UseCases.Notificaciones.Queries.ListarNotificaciones;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/notificaciones")]
public sealed class NotificacionesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<NotificacionDto>>> Listar(CancellationToken ct) =>
        Ok(await sender.Send(new ListarNotificacionesQuery(), ct));

    [HttpPost("{id:guid}/lectura")]
    public async Task<IActionResult> Leer(Guid id, CancellationToken ct)
    {
        await sender.Send(new LeerNotificacionCommand { NotificacionId = id }, ct);
        return NoContent();
    }
}
