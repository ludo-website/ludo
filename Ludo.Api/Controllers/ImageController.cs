using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Ludo.Infrastructure.DataTransferObjects;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.Authorization;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ImageController(ILogger<ImageController> logger, IAccountService accountService, IImageService imageService) : AuthorizedController(logger, accountService)
{
    private const long MaxFileSize = 128 * 1024 * 1024;
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ImageRecord>>>> GetPage([FromQuery] PaginationStringSearchQueryParams pagination)
    {
        var currentAccount = await GetCurrentAccount();
        return currentAccount.Result != null ?
            FromServiceResponse(await imageService.GetImages(pagination)) :
            ErrorMessageResult<PagedResponse<ImageRecord>>(currentAccount.Error);
    }

    [Authorize]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
    [RequestSizeLimit(MaxFileSize)]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromForm] ImageCreateRecord form)
    {
        var currentAccount = await GetCurrentAccount();
        return currentAccount.Result != null ?
            FromServiceResponse(await imageService.SaveImage(form, currentAccount.Result)) :
            ErrorMessageResult(currentAccount.Error);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [Produces(MediaTypeNames.Application.Octet, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(Stream), StatusCodes.Status200OK)]
    public async Task<ActionResult<RequestResponse<FileRecord>>> Download([FromRoute] Guid id)
    {
        var currentAccount = await GetCurrentAccount();
        if (currentAccount.Result == null)
        {
            return ErrorMessageResult<FileRecord>(currentAccount.Error);
        }
        var file = await imageService.GetImageDownload(id);
        return FromServiceResponse(file);
    }
}
