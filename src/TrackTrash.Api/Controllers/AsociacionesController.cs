using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Api.Contracts;
using TrackTrash.Application.DTOs.Asociaciones;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.Asociaciones.Commands.CrearAsociacion;
using TrackTrash.Application.UseCases.Asociaciones.Commands.VerificarAsociacion;
using TrackTrash.Application.UseCases.Asociaciones.Queries.BuscarCandidatos;
using TrackTrash.Application.UseCases.Asociaciones.Queries.ListarAsociaciones;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/asociaciones")]
public sealed class AsociacionesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AsociacionDto>>> Listar(CancellationToken ct) =>
        Ok(await sender.Send(new ListarAsociacionesQuery(), ct));

    [HttpGet("candidatas")]
    public async Task<ActionResult<IReadOnlyList<AsociacionDto>>> BuscarCandidatas(
        [FromQuery] MaterialReciclable material, [FromQuery] decimal kg, [FromQuery] double latitud,
        [FromQuery] double longitud, CancellationToken ct) => Ok(await sender.Send(new BuscarCandidatosQuery
        { Material = material, Kg = kg, Latitud = latitud, Longitud = longitud }, ct));

    [HttpPost, Authorize(Policy = PoliticasTrackTrash.SoloAdministradores)]
    public async Task<ActionResult<AsociacionDto>> Crear(CrearAsociacionRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new CrearAsociacionCommand
        {
            DistritoId = request.DistritoId, Nombre = request.Nombre, Direccion = request.Direccion,
            Latitud = request.Latitud, Longitud = request.Longitud, CapacidadKg = request.CapacidadKg,
            Materiales = request.Materiales
        }, ct);
        return Created($"api/v1/asociaciones/{result.Id}", result);
    }

    [HttpPost("{id:guid}/verificacion"), Authorize(Policy = PoliticasTrackTrash.SoloAdministradores)]
    public async Task<IActionResult> Verificar(Guid id, CancellationToken ct)
    {
        await sender.Send(new VerificarAsociacionCommand { AsociacionId = id }, ct);
        return NoContent();
    }
}
