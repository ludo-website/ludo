using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Ludo.Infrastructure.DataTransferObjects;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.DataTransferObjects;
using Ludo.Services.Clients.Abstractions;
using Ludo.Services.Authorization;

namespace Ludo.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ImageController(ILogger<ImageController> logger, IAccountClient accountClient, IImageClient imageClient) : AuthorizedController(logger, accountClient)
{
    private const long MaxFileSize = 128 * 1024 * 1024;
    [Authorize]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
    [RequestSizeLimit(MaxFileSize)]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromForm] ImageCreateRecord form)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await imageClient.Add(form, currentAccount.Result));
    }
    [Authorize]
    [HttpGet("{id:guid}")]
    [Produces(MediaTypeNames.Application.Octet, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(Stream), StatusCodes.Status200OK)]
    public async Task<ActionResult<RequestResponse<FileRecord>>> Download([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await imageClient.Download(id, currentAccount.Result));
    }
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<ImageRecord>>> Get([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await imageClient.Get(id, currentAccount.Result));
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ImageRecord>>>> FilterByPost([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await imageClient.FilterByPost(pagination, currentAccount.Result));
    }
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        return FromServiceResponse(await imageClient.Delete(id, currentAccount.Result));
    }
}
