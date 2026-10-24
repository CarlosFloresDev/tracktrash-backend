using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.UseCases.Incidencias;
namespace TrackTrash.Api.Controllers;
[ApiController,Authorize,Route("api/v1/incidencias")]
public sealed class IncidenciasController:ControllerBase
{
    [HttpGet]public async Task<IActionResult> Listar([FromServices] ListarIncidenciasUseCase caso,CancellationToken ct)=>Ok(await caso.ExecuteAsync(ct));
    [HttpPost]public async Task<IActionResult> Crear(CrearIncidenciaRequest r,[FromServices] CrearIncidenciaUseCase caso,CancellationToken ct)=>Ok(await caso.ExecuteAsync(r,ct));
    [HttpPost("{id:guid}/atencion"),Authorize(Roles="admin")]
    public async Task<IActionResult> Atender(Guid id,[FromServices] AtenderIncidenciaUseCase caso,CancellationToken ct){await caso.ExecuteAsync(id,ct);return NoContent();}
    [HttpPost("{id:guid}/resolucion"),Authorize(Roles="admin")]
    public async Task<IActionResult> Resolver(Guid id,ResolverIncidenciaRequest r,[FromServices] ResolverIncidenciaUseCase caso,CancellationToken ct){await caso.ExecuteAsync(id,r.Resolucion,ct);return NoContent();}
}
