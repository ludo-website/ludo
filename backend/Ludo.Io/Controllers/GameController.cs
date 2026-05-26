using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class GameController(ILogger<GameController> logger, IGameService gameService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] GameCreateRecord game)
    {
        return FromServiceResponse(await gameService.Add(game));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<GameRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await gameService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameRecord>>>> FilterByTitle([FromQuery] PaginationStringSearchQueryParams pagination)
    {
        return FromServiceResponse(await gameService.FilterByTitle(pagination));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameRecord>>>> FilterByAccount([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await gameService.FilterByAccount(pagination));
    }
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] GameUpdateRecord game)
    {
        return FromServiceResponse(await gameService.Update(game));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await gameService.Delete(id));
    }
}
