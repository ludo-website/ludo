using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class GameAestheticController(ILogger<GameAestheticController> logger, IGameAestheticService gameAestheticService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] GameAestheticCreateRecord gameAesthetic)
    {
        return FromServiceResponse(await gameAestheticService.Add(gameAesthetic));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<GameAestheticRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await gameAestheticService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameAestheticRecord>>>> FilterByGame([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await gameAestheticService.FilterByGame(pagination));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameAestheticRecord>>>> FilterByAesthetic([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await gameAestheticService.FilterByAesthetic(pagination));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await gameAestheticService.Delete(id));
    }
}
