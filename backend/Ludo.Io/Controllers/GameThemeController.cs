using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class GameThemeController(ILogger<GameThemeController> logger, IGameThemeService gameThemeService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] GameThemeCreateRecord gameTheme)
    {
        return FromServiceResponse(await gameThemeService.Add(gameTheme));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<GameThemeRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await gameThemeService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameThemeRecord>>>> FilterByGame([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await gameThemeService.FilterByGame(pagination));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameThemeRecord>>>> FilterByTheme([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await gameThemeService.FilterByTheme(pagination));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await gameThemeService.Delete(id));
    }
}
