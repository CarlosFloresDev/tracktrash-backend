using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Api.Contracts;
using TrackTrash.Application.DTOs.Contenedores;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.Contenedores.Commands.CrearContenedor;
using TrackTrash.Application.UseCases.Contenedores.Commands.ReportarNivel;
using TrackTrash.Application.UseCases.Contenedores.Queries.ConsultarMapa;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/mapa")]
public sealed class MapaController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<MapaDto>> Consultar([FromQuery] double latitud, [FromQuery] double longitud,
        [FromQuery] double radioKm = 20, CancellationToken ct = default) => Ok(await sender.Send(
            new ConsultarMapaQuery { Latitud = latitud, Longitud = longitud, RadioKm = radioKm }, ct));

    [HttpPost("contenedores"), Authorize(Policy = PoliticasTrackTrash.SoloAdministradores)]
    public async Task<ActionResult<ContenedorDto>> CrearContenedor(CrearContenedorRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new CrearContenedorCommand
        { DistritoId = request.DistritoId, Nombre = request.Nombre, Latitud = request.Latitud, Longitud = request.Longitud }, ct);
        return Created($"api/v1/mapa/contenedores/{result.Id}", result);
    }

    [HttpPost("contenedores/{id:guid}/nivel"), Authorize(Policy = PoliticasTrackTrash.Operadores)]
    public async Task<ActionResult<ContenedorDto>> ReportarNivel(Guid id, ReportarNivelRequest request, CancellationToken ct) =>
        Ok(await sender.Send(new ReportarNivelCommand { ContenedorId = id, Porcentaje = request.Porcentaje }, ct));
}
