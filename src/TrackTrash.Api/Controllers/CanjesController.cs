using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Api.Contracts;
using TrackTrash.Application.DTOs.Canjes;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.Canjes.Commands.CanjearBeneficio;
using TrackTrash.Application.UseCases.Canjes.Queries.ListarCanjes;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize(Policy = PoliticasTrackTrash.SoloVecinos), Route("api/v1/canjes")]
public sealed class CanjesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CanjeDto>>> Listar(CancellationToken ct) =>
        Ok(await sender.Send(new ListarCanjesQuery(), ct));

    [HttpPost]
    public async Task<ActionResult<CanjeDto>> Canjear(CanjearBeneficioRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new CanjearBeneficioCommand
        { BeneficioId = request.BeneficioId, Clave = request.Clave }, ct);
        return Created($"api/v1/canjes/{result.Id}", result);
    }
}
