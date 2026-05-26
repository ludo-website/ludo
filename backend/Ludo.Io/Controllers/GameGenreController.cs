using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class GameGenreController(ILogger<GameGenreController> logger, IGameGenreService gameGenreService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] GameGenreCreateRecord gameGenre)
    {
        return FromServiceResponse(await gameGenreService.Add(gameGenre));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<GameGenreRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await gameGenreService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameGenreRecord>>>> FilterByGame([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await gameGenreService.FilterByGame(pagination));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GameGenreRecord>>>> FilterByGenre([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await gameGenreService.FilterByGenre(pagination));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await gameGenreService.Delete(id));
    }
}
