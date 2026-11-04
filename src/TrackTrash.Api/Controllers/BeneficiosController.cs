using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Api.Contracts;
using TrackTrash.Application.DTOs.Beneficios;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.Beneficios.Commands.CrearBeneficio;
using TrackTrash.Application.UseCases.Beneficios.Queries.ListarBeneficios;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/beneficios")]
public sealed class BeneficiosController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BeneficioDto>>> Listar(CancellationToken ct) =>
        Ok(await sender.Send(new ListarBeneficiosQuery(), ct));

    [HttpPost, Authorize(Policy = PoliticasTrackTrash.SoloAdministradores)]
    public async Task<ActionResult> Crear(CrearBeneficioRequest request, CancellationToken ct)
    {
        var id = await sender.Send(new CrearBeneficioCommand
        { Nombre = request.Nombre, Descripcion = request.Descripcion, CostoPuntos = request.CostoPuntos, Stock = request.Stock }, ct);
        return Created($"api/v1/beneficios/{id}", new { id });
    }
}
