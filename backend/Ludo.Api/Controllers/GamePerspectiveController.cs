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
public class GamePerspectiveController(ILogger<GamePerspectiveController> logger, IAccountClient accountClient, IGamePerspectiveClient gamePerspectiveClient) : AuthorizedController(logger, accountClient)
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] GamePerspectiveCreateRecord gamePerspective)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gamePerspectiveClient.Add(gamePerspective, currentAccount.Result));
    }
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<GamePerspectiveRecord>>> Get([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gamePerspectiveClient.Get(id, currentAccount.Result));
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GamePerspectiveRecord>>>> FilterByGame([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gamePerspectiveClient.FilterByGame(pagination, currentAccount.Result));
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GamePerspectiveRecord>>>> FilterByPerspective([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gamePerspectiveClient.FilterByPerspective(pagination, currentAccount.Result));
    }
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gamePerspectiveClient.Delete(id, currentAccount.Result));
    }
}
