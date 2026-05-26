using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;
using Ludo.Services.Clients.Abstractions;
using Ludo.Services.Authorization;

namespace Ludo.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class GameThemeController(ILogger<GameThemeController> logger, IAccountClient accountClient, IGameThemeClient gameThemeClient) : AuthorizedController(logger, accountClient)
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] GameThemeCreateRecord gameTheme)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameThemeClient.Add(gameTheme, currentAccount.Result));
    }
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<GameThemeRecord>>> Get([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameThemeClient.Get(id, currentAccount.Result));
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameThemeRecord>>>> FilterByGame([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameThemeClient.FilterByGame(pagination, currentAccount.Result));
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameThemeRecord>>>> FilterByTheme([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameThemeClient.FilterByTheme(pagination, currentAccount.Result));
    }
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameThemeClient.Delete(id, currentAccount.Result));
    }
}
