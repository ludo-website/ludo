using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class GamePerspectiveController(ILogger<GamePerspectiveController> logger, IGamePerspectiveService gamePerspectiveService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] GamePerspectiveCreateRecord gamePerspective)
    {
        return FromServiceResponse(await gamePerspectiveService.Add(gamePerspective));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<GamePerspectiveRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await gamePerspectiveService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GamePerspectiveRecord>>>> FilterByGame([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await gamePerspectiveService.FilterByGame(pagination));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GamePerspectiveRecord>>>> FilterByPerspective([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await gamePerspectiveService.FilterByPerspective(pagination));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await gamePerspectiveService.Delete(id));
    }
}
