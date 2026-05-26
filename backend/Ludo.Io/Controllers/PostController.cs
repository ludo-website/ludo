using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class PostController(ILogger<PostController> logger, IPostService postService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] PostCreateRecord post)
    {
        return FromServiceResponse(await postService.Add(post));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<PostRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await postService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<PostRecord>>>> FilterByGame([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await postService.FilterByGame(pagination));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<PostRecord>>>> FilterByTitle([FromQuery] PaginationStringSearchQueryParams pagination)
    {
        return FromServiceResponse(await postService.FilterByTitle(pagination));
    }
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] PostUpdateRecord post)
    {
        return FromServiceResponse(await postService.Update(post));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await postService.Delete(id));
    }
}
