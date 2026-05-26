using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class GenreController(ILogger<GenreController> logger, IGenreService genreService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] GenreCreateRecord genre)
    {
        return FromServiceResponse(await genreService.Add(genre));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<GenreRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await genreService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<GenreRecord>>>> FilterByName([FromQuery] PaginationStringSearchQueryParams pagination)
    {
        return FromServiceResponse(await genreService.FilterByName(pagination));
    }
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] GenreUpdateRecord genre)
    {
        return FromServiceResponse(await genreService.Update(genre));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await genreService.Delete(id));
    }
}
