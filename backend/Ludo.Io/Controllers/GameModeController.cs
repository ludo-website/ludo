using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class GameModeController(ILogger<GameModeController> logger, IGameModeService gameModeService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] GameModeCreateRecord gameMode)
    {
        return FromServiceResponse(await gameModeService.Add(gameMode));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<GameModeRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await gameModeService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameModeRecord>>>> FilterByGame([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await gameModeService.FilterByGame(pagination));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameModeRecord>>>> FilterByMode([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await gameModeService.FilterByMode(pagination));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await gameModeService.Delete(id));
    }
}
