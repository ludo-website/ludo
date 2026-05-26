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
public class GameController(ILogger<GameController> logger, IAccountClient accountClient, IGameClient gameClient) : AuthorizedController(logger, accountClient)
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] GameCreateRecord game)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameClient.Add(game, currentAccount.Result));
    }
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<GameRecord>>> Get([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameClient.Get(id, currentAccount.Result));
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameRecord>>>> FilterByTitle([FromQuery] PaginationStringSearchQueryParams pagination)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameClient.FilterByTitle(pagination, currentAccount.Result));
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameRecord>>>> FilterByAccount([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameClient.FilterByAccount(pagination, currentAccount.Result));
    }
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] GameUpdateRecord game)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameClient.Update(game, currentAccount.Result));
    }
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await gameClient.Delete(id, currentAccount.Result));
    }
}
