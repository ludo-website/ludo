using Microsoft.AspNetCore.Mvc;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class CommentController(ILogger<CommentController> logger, ICommentService commentService) : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] CommentCreateRecord comment)
    {
        return FromServiceResponse(await commentService.Add(comment));
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<CommentRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await commentService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<CommentRecord>>>> FilterByAccount([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await commentService.FilterByAccount(pagination));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<CommentRecord>>>> FilterByPost([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await commentService.FilterByPost(pagination));
    }
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] CommentUpdateRecord comment)
    {
        return FromServiceResponse(await commentService.Update(comment));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await commentService.Delete(id));
    }
}
