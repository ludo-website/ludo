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
public class GameModeController(ILogger<GameModeController> logger, IAccountClient accountClient, IGameModeClient gameModeClient) : AuthorizedController(logger, accountClient)
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] GameModeCreateRecord gameMode)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameModeClient.Add(gameMode, currentAccount.Result));
    }
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<GameModeRecord>>> Get([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameModeClient.Get(id, currentAccount.Result));
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameModeRecord>>>> FilterByGame([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameModeClient.FilterByGame(pagination, currentAccount.Result));
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameModeRecord>>>> FilterByMode([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameModeClient.FilterByMode(pagination, currentAccount.Result));
    }
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameModeClient.Delete(id, currentAccount.Result));
    }
}
