using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.UseCases.Incidencias.Commands;
using TrackTrash.Application.UseCases.Incidencias.Queries;
namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/incidencias")]
public sealed class IncidenciasController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken ct) => Ok(await sender.Send(new ListarIncidenciasQuery(), ct));

    [HttpPost]
    public async Task<IActionResult> Crear(CrearIncidenciaRequest r, CancellationToken ct) =>
        Ok(await sender.Send(new CrearIncidenciaCommand { ContenedorId = r.ContenedorId, Tipo = r.Tipo, Descripcion = r.Descripcion }, ct));

    [HttpPost("{id:guid}/atencion"), Authorize(Roles = "admin")]
    public async Task<IActionResult> Atender(Guid id, CancellationToken ct)
    { await sender.Send(new AtenderIncidenciaCommand { IncidenciaId = id }, ct); return NoContent(); }

    [HttpPost("{id:guid}/resolucion"), Authorize(Roles = "admin")]
    public async Task<IActionResult> Resolver(Guid id, ResolverIncidenciaRequest r, CancellationToken ct)
    { await sender.Send(new ResolverIncidenciaCommand { IncidenciaId = id, Resolucion = r.Resolucion }, ct); return NoContent(); }
}
