using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackTrash.Application.DTOs.Distritos;
using TrackTrash.Application.UseCases.Distritos.Queries.ListarDistritos;

namespace TrackTrash.Api.Controllers;

[ApiController, Authorize, Route("api/v1/distritos")]
public sealed class DistritosController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DistritoDto>>> Listar(CancellationToken ct) =>
        Ok(await sender.Send(new ListarDistritosQuery(), ct));
}
