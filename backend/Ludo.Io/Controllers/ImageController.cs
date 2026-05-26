using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Ludo.Infrastructure.DataTransferObjects;
using Ludo.Infrastructure.Handlers;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Io.Controllers;

[ApiController]
[Route("io/[controller]/[action]")]
public class ImageController(ILogger<ImageController> logger, IImageService imageService) : BaseResponseController(logger)
{
    private const long MaxFileSize = 128 * 1024 * 1024;
    [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
    [RequestSizeLimit(MaxFileSize)]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromForm] ImageCreateRecord form)
    {
        return FromServiceResponse(await imageService.Add(form));
    }
    [HttpGet("{id:guid}")]
    [Produces(MediaTypeNames.Application.Octet, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(Stream), StatusCodes.Status200OK)]
    public async Task<ActionResult<RequestResponse<FileRecord>>> Download([FromRoute] Guid id)
    {
        var image = await imageService.Download(id);
        return FromServiceResponse(image);
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<ImageRecord>>> Get([FromRoute] Guid id)
    {
        return FromServiceResponse(await imageService.Get(id));
    }
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ImageRecord>>>> FilterByPost([FromQuery] PaginationIdSearchQueryParams pagination)
    {
        return FromServiceResponse(await imageService.FilterByPost(pagination));
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        return FromServiceResponse(await imageService.Delete(id));
    }
}
