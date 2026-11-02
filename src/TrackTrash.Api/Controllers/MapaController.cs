using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.UseCases.Catalogo.Commands;
using TrackTrash.Application.UseCases.Catalogo.Queries;
using TrackTrash.Application.UseCases.Contenedores.Commands;
using TrackTrash.Application.UseCases.Contenedores.Queries;
using TrackTrash.Domain.Entities;
namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1")]
public sealed class MapaController(ISender sender) : ControllerBase
{
    [HttpGet("mapa")]
    public async Task<IActionResult> Mapa(CancellationToken ct, double lat = -16.3988, double lng = -71.5369, double radioKm = 10) =>
        Ok(await sender.Send(new ConsultarMapaQuery { Latitud = lat, Longitud = lng, RadioKm = radioKm }, ct));

    [HttpGet("asociaciones")]
    public async Task<IActionResult> Asociaciones(CancellationToken ct) => Ok(await sender.Send(new ConsultarAliadosQuery(), ct));

    [HttpGet("asociaciones/candidatos")]
    public async Task<IActionResult> Candidatos(MaterialReciclable material, decimal kg, double lat, double lng, CancellationToken ct) =>
        Ok(await sender.Send(new BuscarCandidatosQuery { Material = material, Kg = kg, Latitud = lat, Longitud = lng }, ct));

    [HttpGet("distritos")]
    public async Task<IActionResult> Distritos(CancellationToken ct) => Ok(await sender.Send(new ListarDistritosQuery(), ct));

    [HttpPost("asociaciones"), Authorize(Roles = "admin")]
    public async Task<IActionResult> Crear(CrearAsociacionRequest r, CancellationToken ct) =>
        Ok(await sender.Send(new CrearAsociacionCommand
        {
            DistritoId = r.DistritoId,
            Nombre = r.Nombre,
            Direccion = r.Direccion,
            Latitud = r.Latitud,
            Longitud = r.Longitud,
            CapacidadKg = r.CapacidadKg,
            Materiales = r.Materiales
        }, ct));

    [HttpPost("asociaciones/{id:guid}/verificacion"), Authorize(Roles = "admin")]
    public async Task<IActionResult> Verificar(Guid id, CancellationToken ct)
    { await sender.Send(new VerificarAsociacionCommand { AsociacionId = id }, ct); return NoContent(); }

    [HttpPost("contenedores"), Authorize(Roles = "admin")]
    public async Task<IActionResult> CrearContenedor(CrearContenedorRequest r, CancellationToken ct) =>
        Ok(await sender.Send(new CrearContenedorCommand { DistritoId = r.DistritoId, Nombre = r.Nombre, Latitud = r.Latitud, Longitud = r.Longitud }, ct));

    [HttpPost("contenedores/{id:guid}/reporte-nivel")]
    public async Task<IActionResult> ReportarNivel(Guid id, ReportarNivelRequest r, CancellationToken ct) =>
        Ok(await sender.Send(new ReportarNivelCommand { ContenedorId = id, Porcentaje = r.Porcentaje }, ct));
}
