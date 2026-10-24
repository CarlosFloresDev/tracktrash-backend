using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.UseCases.Contenedores;
using TrackTrash.Application.UseCases.Catalogo;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1")]
public sealed class MapaController : ControllerBase
{
    [HttpGet("mapa")]
    public async Task<IActionResult> Mapa([FromServices] ConsultarMapaUseCase caso, CancellationToken ct, double lat = -16.3988, double lng = -71.5369, double radioKm = 10) => Ok(await caso.ExecuteAsync(lat, lng, radioKm, ct));
    [HttpGet("asociaciones")]
    public async Task<IActionResult> Asociaciones([FromServices] ConsultarAliadosUseCase caso, CancellationToken ct) => Ok(await caso.ExecuteAsync(ct));
    [HttpGet("asociaciones/candidatos")]
    public async Task<IActionResult> Candidatos(MaterialReciclable material, decimal kg, double lat, double lng, [FromServices] BuscarCandidatosUseCase caso, CancellationToken ct) => Ok(await caso.ExecuteAsync(material, kg, lat, lng, ct));
    [HttpGet("distritos")]
    public async Task<IActionResult> Distritos([FromServices] ListarDistritosUseCase caso, CancellationToken ct) => Ok(await caso.ExecuteAsync(ct));
    [HttpPost("asociaciones"), Authorize(Roles = "admin")]
    public async Task<IActionResult> Crear(CrearAsociacionRequest r, [FromServices] CrearAsociacionUseCase caso, CancellationToken ct) => Ok(await caso.ExecuteAsync(r, ct));
    [HttpPost("asociaciones/{id:guid}/verificacion"), Authorize(Roles = "admin")]
    public async Task<IActionResult> Verificar(Guid id, [FromServices] VerificarAsociacionUseCase caso, CancellationToken ct) { await caso.ExecuteAsync(id, ct); return NoContent(); }
    [HttpPost("contenedores"), Authorize(Roles = "admin")]
    public async Task<IActionResult> CrearContenedor(CrearContenedorRequest r, [FromServices] CrearContenedorUseCase caso, CancellationToken ct) => Ok(await caso.ExecuteAsync(r, ct));
    [HttpPost("contenedores/{id:guid}/reporte-nivel")]
    public async Task<IActionResult> ReportarNivel(Guid id, ReportarNivelRequest r, [FromServices] ReportarNivelUseCase caso, CancellationToken ct) => Ok(await caso.ExecuteAsync(id, r.Porcentaje, ct));
}
