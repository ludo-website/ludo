using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class GamePlatformController(ILogger<GamePlatformController> logger, IGamePlatformService gamePlatformService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] GamePlatformCreateRecord gamePlatform)
    {
        return FromServiceResponse(await gamePlatformService.Add(gamePlatform));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<GamePlatformRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await gamePlatformService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GamePlatformRecord>>>> FilterByGame([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await gamePlatformService.FilterByGame(pagination));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GamePlatformRecord>>>> FilterByPlatform([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await gamePlatformService.FilterByPlatform(pagination));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await gamePlatformService.Delete(id));
    }
}
