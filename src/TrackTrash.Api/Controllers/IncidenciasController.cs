using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Api.Contracts;
using TrackTrash.Application.DTOs.Incidencias;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.Incidencias.Commands.AtenderIncidencia;
using TrackTrash.Application.UseCases.Incidencias.Commands.CrearIncidencia;
using TrackTrash.Application.UseCases.Incidencias.Commands.ResolverIncidencia;
using TrackTrash.Application.UseCases.Incidencias.Queries.ListarIncidencias;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/incidencias")]
public sealed class IncidenciasController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<IncidenciaDto>>> Listar(CancellationToken ct) =>
        Ok(await sender.Send(new ListarIncidenciasQuery(), ct));

    [HttpPost]
    public async Task<ActionResult<IncidenciaDto>> Crear(CrearIncidenciaRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new CrearIncidenciaCommand
        { ContenedorId = request.ContenedorId, Tipo = request.Tipo, Descripcion = request.Descripcion }, ct);
        return Created($"api/v1/incidencias/{result.Id}", result);
    }

    [HttpPost("{id:guid}/atencion"), Authorize(Policy = PoliticasTrackTrash.SoloAdministradores)]
    public async Task<IActionResult> Atender(Guid id, CancellationToken ct)
    {
        await sender.Send(new AtenderIncidenciaCommand { IncidenciaId = id }, ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/resolucion"), Authorize(Policy = PoliticasTrackTrash.SoloAdministradores)]
    public async Task<IActionResult> Resolver(Guid id, ResolverIncidenciaRequest request, CancellationToken ct)
    {
        await sender.Send(new ResolverIncidenciaCommand { IncidenciaId = id, Resolucion = request.Resolucion }, ct);
        return NoContent();
    }
}
